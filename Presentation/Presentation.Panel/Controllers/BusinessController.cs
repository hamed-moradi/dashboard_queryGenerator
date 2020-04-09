using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OfficeOpenXml;
using PagedList;
using Presentation.Panel.FilterAttributes;
using Presentation.Panel.Helpers;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Presentation.Panel.Helpers;

namespace Presentation.Panel.Controllers
{
    public class BusinessController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IBusinessService _businessService;
        private readonly IRateService _rateService;
        private readonly ICommentService _commentService;
        private readonly IDropDownService _dropDownService;
        private readonly IActivityService _activityService;
        private readonly ICategory2BusinessService _category2BusinessService;
        private readonly IGeoService _geoService;
        private readonly IAdminService _adminService;

        public BusinessController(IGeoService geoService, IBusinessService providerService, IRateService rateService, ICommentService commentService, IMapper mapper, ILog4Net logger, IDropDownService dropDownService, IActivityService activityService, ICategory2BusinessService category2BusinessService, IAdminService adminService)
        {
            _dropDownService = dropDownService;
            _rateService = rateService;
            _commentService = commentService;
            _businessService = providerService;
            _mapper = mapper;
            _logger = logger;
            _activityService = activityService;
            _geoService = geoService;
            _category2BusinessService = category2BusinessService;
            _adminService = adminService;
        }

        #endregion

        // GET: Business
        [HttpGet]
        public ActionResult Index(int? providerId)
        {
            ViewBag.ProviderId = providerId;
            try
            {
                ViewBag.SearchModel = new BusinessViewModel { };
                var request = new FilteredModel<Business> { };
                var offset = (request.PageIndex - 1) * request.PageSize;
                var test = _businessService.GetPaging(new Business { ProviderId = providerId }, out long totalCount1, request.OrderBy, request.Order, offset, request.PageSize);
                var result = _mapper.Map<IList<BusinessViewModel>>(_businessService.GetPaging(new Business { ProviderId = providerId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<BusinessViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Business
        [HttpPost]
        public ActionResult Index(BusinessViewModel collection)
        {

            var request = new FilteredModel<Business>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var test = _businessService.GetPaging(_mapper.Map<Business>(collection), out long totalCount1, request.OrderBy, request.Order, offset, request.PageSize);
            var result = _mapper.Map<IList<BusinessViewModel>>(_businessService.GetPaging(_mapper.Map<Business>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<BusinessViewModel>>(_businessService.GetPaging(_mapper.Map<Business>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<BusinessViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            ViewBag.ProviderId = collection.ProviderId;
            return View();
        }

        // GET: Business/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(_mapper.Map<BusinessViewModel>(_businessService.GetById(id)));
        }

        // POST: Business/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, BusinessViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _businessService.Delete(_mapper.Map<Business>(collection));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }

        // GET: Business/NewDetails/5
        [HttpGet]
        public ActionResult NewDetails(int id)
        {
            try
            {
                var model = _businessService.GetById(id);
                if (model != null)
                {
                    if (model.LastEntityData != null)
                    {
                        var jobject = JObject.Parse(model.LastEntityData);
                        var newData = jobject["Business"].ToObject<Business>();
                        newData.Location = null;
                        var loc = jobject["Business"]["Location"];
                        if (loc != null)
                        {
                            try
                            {
                                var lat = float.Parse(loc["Lat"].ToString());
                                var lng = float.Parse(loc["Lng"].ToString());
                                newData.Location = SqlGeography.Point(lat, lng, 4326);
                            }
                            catch { }
                        }
                        if (newData.GeoId == 0)
                        {
                            newData.GeoId = null;
                        }
                        var oldData = _mapper.Map<Business>(model);
                        ViewBag.NewData = _mapper.Map<BusinessViewModel>(newData);
                        ViewBag.OldData = _mapper.Map<BusinessViewModel>(oldData);

                        var myBusinessInfo = newData.GetType().GetProperties();

                        foreach (var item in myBusinessInfo)
                        {
                            var key = item.Name;
                            var value = item.GetValue(newData, null);
                            if (value == null)
                            {
                                continue;
                            }
                            var propertyInfo = CommonHelper.GetLowestProperty(model.GetType(), key);
                            if (propertyInfo == null)
                            {
                                continue;
                            }
                            var setMethod = propertyInfo.GetSetMethod();
                            if (setMethod != null)
                            {
                                propertyInfo.SetValue(model, value, null);
                            }
                        }

                        var viewModel = _mapper.Map<BusinessViewModel>(model);
                        return View(viewModel);
                    }
                    ModelState.AddModelError(string.Empty, "درخواست جدیدی برای ویرایش این کسب و کار یافت نشد.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.NothingFound);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(new BusinessViewModel());
        }

        // POST: Business/NewDetails/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult NewDetails(int id, BusinessViewModel collection, string decision)
        {
            if (string.IsNullOrWhiteSpace(decision))
            {
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
            }
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
            }
            decision = decision.Trim().ToLower();
            switch (decision)
            {
                case "accept":
                    return AcceptNewDetails(id, collection);
                case "reject":
                    return RejectNewDetails(id, collection);
                default:
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                    return View(collection);
            }
        }
        [HttpPost]
        private ActionResult AcceptNewDetails(int id, BusinessViewModel collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        }
                    }
                    return View(collection);
                }
                collection.UpdaterId = LogedInAdmin.Id;

                var model = _mapper.Map<Business>(collection);
                model.LastEntityData = null;
                var updateResult = _businessService.Update(model);
                if (updateResult < 1)
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.ErrorInSave);
                    return View(collection);
                }
                var settings = new JsonSerializerSettings().AddSqlConverters();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var json = JsonConvert.SerializeObject(model, settings);
                if (model.Location != null)
                {
                    json = json.Insert(json.Length - 1, ",\"Location\":{\"Lat\":" + model.Location.Lat + ",\"Lng\":" + model.Location.Long + "}");
                }
                var activity = new Activity
                {
                    ActionTypeId = (byte)GeneralEnums.ActionType.Accept,
                    CreatorId = LogedInAdmin.Id,
                    EntityId = id,
                    ParentId = collection.ActivityId,
                    EntityTypeId = (byte)GeneralEnums.EntityType.Business,
                    Data = "{\"Business\":" + json + "}"
                };
                _activityService.Insert(activity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }
        private ActionResult RejectNewDetails(int id, BusinessViewModel collection)
        {
            var model = _businessService.GetById(id);
            model.LastEntityData = null;
            _businessService.Update(model);
            var settings = new JsonSerializerSettings().AddSqlConverters();
            settings.NullValueHandling = NullValueHandling.Ignore;
            var json = JsonConvert.SerializeObject(model, settings);
            if (model.Location != null)
            {
                json = json.Insert(json.Length - 1, ",\"Location\":{\"Lat\":" + model.Location.Lat + ",\"Lng\":" + model.Location.Long + "}");
            }
            var activity = new Activity
            {
                ActionTypeId = (byte)GeneralEnums.ActionType.Reject,
                CreatorId = LogedInAdmin.Id,
                EntityId = id,
                ParentId = collection.ActivityId,
                EntityTypeId = (byte)GeneralEnums.EntityType.Business,
                Data = "{\"Business\":" + json + "}"
            };
            _activityService.Insert(activity);
            return RedirectToAction("Index");
        }
        // GET: Business/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Business/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(BusinessViewModel collection)
        {
            try
            {
                collection.AvailabilityStatusId = GeneralEnums.AvilabilityStatus.NotAvailable;
                collection.EvidenceStatusId = GeneralEnums.EvidenceStatusType.Unknown;
                ModelState["AvailabilityStatusId"].Errors.Clear();
                ModelState["EvidenceStatusId"].Errors.Clear();
                UpdateModel(collection);
                if (ModelState.IsValid)
                {
                    collection.CreatorId = LogedInAdmin.Id;
                    var model = _mapper.Map<Business>(collection);
                    model.Location = SqlGeography.Point(collection.Latitude.Value, collection.Longitude.Value, 4326);
                    var id =_businessService.Insert(model);
                    model.Id = id;
                    var settings = new JsonSerializerSettings().AddSqlConverters();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    var json = JsonConvert.SerializeObject(model, settings);
                    if (model.Location != null)
                    {
                        json = json.Insert(json.Length - 1, ",\"Location\":{\"Lat\":" + model.Location.Lat + ",\"Lng\":" + model.Location.Long + "}");
                    }
                    var activity = new Activity
                    {
                        ActionTypeId = (byte)GeneralEnums.ActionType.Create,
                        CreatorId = LogedInAdmin.Id,
                        EntityId = model.Id,
                        EntityTypeId = (byte)GeneralEnums.EntityType.Business,
                        Data = "{\"Business\":" + json + "}"
                    };
                    _activityService.Insert(activity);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }

        // GET: Business/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _businessService.GetById(id);
            var viewModel = _mapper.Map<BusinessViewModel>(model);
            ///Mapper doesn't work for Location column again! Whats wrong with it?
            viewModel.Latitude = (double)model.Location.Lat;
            viewModel.Longitude = (double)model.Location.Long;
            return View(viewModel);
        }

        // POST: Business/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, BusinessViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        collection.UpdaterId = LogedInAdmin.Id;
                        var model = _mapper.Map<Business>(collection);
                        model.Location = SqlGeography.Point(collection.Latitude.Value, collection.Longitude.Value, 4326);
                        _businessService.Update(model);
                        var settings = new JsonSerializerSettings().AddSqlConverters();
                        settings.NullValueHandling = NullValueHandling.Ignore;
                        var json = JsonConvert.SerializeObject(model, settings);
                        if (model.Location != null)
                        {
                            json = json.Insert(json.Length - 1, ",\"Location\":{\"Lat\":" + model.Location.Lat + ",\"Lng\":" + model.Location.Long + "}");
                        }
                        var activity = new Activity
                        {
                            ActionTypeId = (byte)GeneralEnums.ActionType.Edit,
                            CreatorId = LogedInAdmin.Id,
                            EntityId = model.Id,
                            EntityTypeId = (byte)GeneralEnums.EntityType.Business,
                            Data = "{\"Business\":" + json + "}"
                        };
                        _activityService.Insert(activity);
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }

        // GET: Business/DropDown
        [HttpGet]
        [AllowAnonymous]
        public JsonResult DropDown(string key, int? p)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length < 3) return null;
            var result = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetBusinesses(key, (p.GetValueOrDefault(1) - 1) * 30, 30, out long totalCount));
            return Json(new DropDownModel { TotalCount = totalCount, Items = result }, JsonRequestBehavior.AllowGet);
        }

        // GET: Business/Rates
        [HttpGet]
        public ActionResult Rates(int BusinessId)
        {
            ViewBag.BusinessId = BusinessId;
            try
            {
                ViewBag.SearchModel = new RateViewModel();
                var request = new FilteredModel<Rate>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var rates = _rateService.GetPaging(new Rate { BusinessId = BusinessId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize);
                var result = _mapper.Map<IList<RateViewModel>>(rates);
                ViewBag.OnePageOfEntries = new StaticPagedList<RateViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Business/Rates
        [HttpPost]
        public ActionResult Rates(RateViewModel collection)
        {
            var request = new FilteredModel<Rate>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<RateViewModel>>(_rateService.GetPaging(_mapper.Map<Rate>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<RateViewModel>>(_rateService.GetPaging(_mapper.Map<Rate>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<RateViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Business/Comments
        [HttpGet]
        public ActionResult Comments(int providerId)
        {
            ViewBag.ProviderId = providerId;
            try
            {
                ViewBag.SearchModel = new CommentViewModel();
                var request = new FilteredModel<Comment>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                //var result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(new Comment { BusinessId = providerId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                var result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(new Comment { CommentEntityId = providerId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<CommentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Business/Comments
        [HttpPost]
        public ActionResult Comments(CommentViewModel collection)
        {
            var request = new FilteredModel<Comment>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(_mapper.Map<Comment>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(_mapper.Map<Comment>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<CommentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        public string getDistrict(string coordinate)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + coordinate + "&sensor=false&key=AIzaSyB9CKKDKAbsr83Xl5V9c3xPyVLhPb1JzI0");

            XmlNodeList xNodelst = xDoc.GetElementsByTagName("result");
            XmlNode xNode = xNodelst.Item(0);
            string adress = xNode.SelectSingleNode("formatted_address").InnerText;
            string mahalle = xNode.SelectSingleNode("address_component[3]/long_name").InnerText;
            string ilce = xNode.SelectSingleNode("address_component[4]/long_name").InnerText;
            string il = xNode.SelectSingleNode("address_component[5]/long_name").InnerText;
            return ilce;
        }
        [HttpGet]
        public ActionResult HistoryTimeLine(int id, int page = 1, int pageSize = 10)
        {
            ViewBag.Page = page;
            var request = new FilteredModel<Activity> { PageIndex = page, PageSize = pageSize };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var searchModel = new Activity
            {
                EntityTypeId = (byte)GeneralEnums.EntityType.Business,
                EntityId = id
            };
            var result = _activityService.History(searchModel, out long totalCount, skip: offset, take: request.PageSize);
            var data = new List<BusinessViewModel>();
            foreach (var activity in result)
            {
                try
                {
                    var jobject = JObject.Parse(activity.Data);
                    var settings = new JsonSerializerSettings().AddSqlConverters();
                    var newData = JsonConvert.DeserializeObject<Business>(jobject["Business"].ToString(),settings);
                    newData.Location = null;
                    var loc = jobject["Business"]["Location"];
                    if (loc != null)
                    {
                        try
                        {
                            var lat = float.Parse(loc["Lat"].ToString());
                            var lng = float.Parse(loc["Lng"].ToString());
                            newData.Location = SqlGeography.Point(lat, lng, 4326);
                        }
                        catch { }
                    }
                    newData.CreatedAt = activity.CreatedAt;
                    newData.CreatorId = activity.CreatorId;
                    if (newData.CreatorId != null)
                    {
                        var admin = _adminService.GetById(newData.CreatorId.Value);
                        newData.CreatorName = (admin != null ? admin.FullName : "نامشخص");
                    }
                    var p = _mapper.Map<BusinessViewModel>(newData);
                    p.ActionTypeId = (GeneralEnums.ActionType)activity.ActionTypeId;
                    if (p.GeoId.HasValue)
                    {
                        var geo = _geoService.GetById(p.GeoId.Value);
                        p.GeoName = (geo != null ? geo.Name : "داده نامعتبر");
                    }
                    data.Add(p);
                }
                catch(Exception ex)
                {
                    data.Add(new BusinessViewModel { Id = -1 });
                }
            }
            ViewData["page"] = page;
            return PartialView("_HistoryTimeLine", data);
        }
        [HttpGet]
        public ActionResult History(int id)
        {
            ViewBag.Id = id;
            var model = _businessService.GetById(id);
            if (model == null)
            {
                return View();
            }
            var business = _mapper.Map<BusinessViewModel>(model);
            return View(business);
        }

        [HttpGet]
        public ActionResult ImportFromExcel()
        {
            ViewBag.CategoryList = new SelectList(_dropDownService.GetCategories(), "id", "name", null);
            return View();
        }
        [HttpPost]
        public ActionResult ImportFromExcel(int providerId, int categoryId, byte status, HttpPostedFileBase file)
        {
            try
            {
                ViewBag.CategoryList = new SelectList(_dropDownService.GetCategories(), "id", "name", null);
                var errors = string.Empty;
                if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName) || !(file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx")))
                {
                    ViewBag.Error = "فایل نامعتبر است.";
                    return View();
                }
                var business = new Business();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var now = DateTime.Now;
                    for (var i = 1; i <= package.Workbook.Worksheets.Count; i++)
                    {
                        var workSheet = package.Workbook.Worksheets[i];
                        var header = new Dictionary<string, int>();
                        for (int rowIndex = workSheet.Dimension.Start.Row; rowIndex <= workSheet.Dimension.End.Row; rowIndex++)
                        {
                            if (rowIndex == 1)
                            {
                                header = ExcelHelper.GetExcelHeader(workSheet, rowIndex);
                                if (header.Count == 0)
                                {
                                    errors += $"برگه {workSheet.Name} - سطر {rowIndex} - ردیف اول خالی است. هیچ کدام از کسب و کارهای برگه خوانده نشدند.<br/>";
                                    break;
                                }
                                continue;
                            }
                            try
                            {
                                var title = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, nameof(Business.Title));
                                if (string.IsNullOrWhiteSpace(title))
                                {
                                    errors += $"برگه {workSheet.Name} - سطر {rowIndex} - نام کسب و کار مشخص نیست. این سطر خوانده نشد.<br/>";
                                    continue;
                                }
                                business = new Business
                                {
                                    Title = title,
                                    ProviderId = providerId,
                                    Address = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, nameof(Business.Address)),
                                    PhoneNo = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, nameof(Business.PhoneNo)),
                                    GenderTypeId = (byte)GeneralEnums.Gender.Undefined,
                                    Location = SqlGeography.Point(59.325, 18.070, 4326),
                                    AvailabilityStatusId = (byte)GeneralEnums.AvilabilityStatus.NotAvailable,
                                    EvidenceStatusId = (byte)GeneralEnums.EvidenceStatusType.Unknown,
                                    CreatorId = LogedInAdmin.Id,
                                    CreatedAt = now,
                                    Status = status
                                };
                                if (!string.IsNullOrWhiteSpace(business.PhoneNo))
                                {
                                    business.PhoneNo = business.PhoneNo.Trim().Split('-').FirstOrDefault(x=>x.Length>0);
                                    if(business.PhoneNo==null || !business.PhoneNo.All(char.IsDigit) || business.PhoneNo.Length>16)
                                    {
                                        errors += $"برگه {workSheet.Name} - کسب و کار {business.Title} - شماره تلفن مشکل داشت. تلفن خالی درج شد.<br/>";
                                        business.PhoneNo = string.Empty;
                                    }
                                }
                            }
                            catch
                            {
                                errors += $"برگه {workSheet.Name} - سطر {rowIndex} - خطایی در اطلاعات این سطر وجود دارد. سطر خوانده نشد.<br/>";
                                continue;
                            }
                            int businessId = 0;
                            try
                            {
                                businessId = _businessService.Insert(business);
                            }
                            catch (Exception ex)
                            {
                                errors += $"برگه {workSheet.Name} - خطایی در ذخیره کسب و کار «{business.Title}» رخ داده است.<br/>";
                                continue;
                            }
                            try
                            {
                                _category2BusinessService.Insert(new Category2Business { BusinessId = businessId, CategoryId = categoryId });
                            }
                            catch(Exception ex)
                            {
                                errors += $"برگه {workSheet.Name} - خطایی در ذخیره دسته بندی کسب و کار «{business.Title}» با شناسه {businessId} رخ داده است.<br/>";
                            }
                        }
                    }
                }
                ViewBag.Error = (errors.Length == 0 ? "عملیات با موفقیت انجام شد" : "عملیات کامل شد اما با این خطاها همراه بود:<br/>" + errors);
                return View();
            }
            catch(Exception ex)
            {
                ViewBag.Error = "خطای نامشخصی رخ داده است";
                return View();
            }
        }
    }
    //public class ShouldSerializeContractResolver : DefaultContractResolver
    //{
    //    public new static readonly ShouldSerializeContractResolver Instance = new ShouldSerializeContractResolver();

    //    //protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    //    //{
    //    //    JsonProperty property = base.CreateProperty(member, memberSerialization);

    //    //    if (property.DeclaringType == typeof(Employee) && property.PropertyName == "Manager")
    //    //    {
    //    //        property.ShouldSerialize =
    //    //            instance =>
    //    //            {
    //    //                Employee e = (Employee)instance;
    //    //                return e.Manager != e;
    //    //            };
    //    //    }

    //    //    return property;
    //    //}
    //}
}