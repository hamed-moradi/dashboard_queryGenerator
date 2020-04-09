using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class Business2FacilityController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IBusiness2FacilityService _business2FacilityService;
        private readonly IFacilityService _facilityService;
        private readonly IDropDownService _dropDownService;
        private readonly IBusinessService _businessService;
        private readonly IActivityService _activityService;
        private readonly IAdminService _adminService;

        public Business2FacilityController(IBusiness2FacilityService business2FacilityService, IFacilityService propertyService, IDropDownService dropDownService, IMapper mapper, ILog4Net logger, IBusinessService businessService, IActivityService activityService, IAdminService adminService)
        {
            _business2FacilityService = business2FacilityService;
            _facilityService = propertyService;
            _dropDownService = dropDownService;
            _mapper = mapper;
            _logger = logger;
            _businessService = businessService;
            _activityService = activityService;
            _adminService = adminService;
        }

        #endregion

        // GET: Business2Facility/Index
        [HttpGet]
        public ActionResult Index(int businessId)
        {
            var business = _businessService.GetById(businessId);
            if (business != null)
            {
                ViewBag.BusinessTitle = business.Title;
            }
            ViewBag.BusinessId = businessId;
            //var properties = _business2FacilityService.Find(new Business2Facility { BusinessId = businessId });
            ViewBag.Properties = _mapper.Map<List<Business2Facility>>(_business2FacilityService.FillJSTree(businessId));
            ViewBag.PropertyTree = _mapper.Map<List<TreeModel>>(_facilityService.GetAdmittables(businessId));
            return View();
        }

        // POST: Business2Facility/Index
        [HttpPost]
        public ActionResult Index(int businessId, List<Business2FacilityViewModel> collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _business2FacilityService.DeleteByBusinessId(businessId);
                    if (collection != null && collection.Count > 0)
                    {
                        foreach (var item in collection)
                        {
                            item.BusinessId = businessId;
                            item.Priority = item.Priority.HasValue ? item.Priority : 0;
                        }
                        var model = _mapper.Map<List<Business2Facility>>(collection);
                       
                        _business2FacilityService.BulkInsert(model);

                        var data = new Business2FacilityJson { Items = _mapper.Map<List<Business2FacilityItem>>(collection) };
                        var activity = new Activity
                        {
                            ActionTypeId = (byte)GeneralEnums.ActionType.Edit,
                            CreatorId = LogedInAdmin.Id,
                            EntityId = businessId,
                            EntityTypeId = (byte)GeneralEnums.EntityType.Business2Facility,
                            Data = "{\"Business2Facility\":" + JsonConvert.SerializeObject(data) + "}"
                        };
                        _activityService.Insert(activity);

                        return RedirectToAction("Index", "Business");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "تمام ویژگی ها حذف شدند");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.Duplicated);
                }
                else
                {
                    if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                }
            }
            ViewBag.BusinessId = businessId;
            ViewBag.PropertyTree = _mapper.Map<List<TreeModel>>(_facilityService.Find(new Facility()));
            return View();
        }
        [HttpGet]
        public ActionResult NewDetails(int id)
        {
            try
            {
                var model = _businessService.GetById(id);
                if (model == null)
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.NothingFound);
                    return View();
                }
                if (model.LastFacilityData == null)
                {
                    ModelState.AddModelError(string.Empty, "درخواست جدیدی برای ویرایش سرویس های این کسب و کار یافت نشد.");
                    return View();
                }
                var jobject = JObject.Parse(model.LastFacilityData);
                var newData = jobject["Business2Facility"].ToObject<Business2FacilityJson>();
                var Properties = _mapper.Map<List<Business2Facility>>(newData.Items);

                var admitables = _facilityService.GetAdmittables(id);
                if (Properties.Any(x => !admitables.Any(a => a.Id == x.FacilityId)))
                {
                    ModelState.AddModelError(string.Empty, "درخواست کاربر شامل سرویس های غیر مجاز است. شما می توانید این درخواست را رد کنید.");
                    ViewBag.JustReject = true;
                }
                var parents = admitables.Where(x => x.ParentId == null);
                var finalProperties = new List<Business2Facility>();
                foreach (var p in Properties)
                {
                    if (!parents.Any(x => x.Id == p.FacilityId))
                    {
                        finalProperties.Add(p);
                        continue;
                    }
                    var children = admitables.Where(x => x.ParentId == p.FacilityId);
                    var result = Properties.Where(p2 => children.Any(c => p2.FacilityId == c.Id)).ToList();
                    if (result.Count == 0)
                    {
                        finalProperties.Add(p);
                    }
                }
                

                ViewBag.PropertyTree = _mapper.Map<List<TreeModel>>(admitables); ;
                ViewBag.FinalProperties = finalProperties;
                ViewBag.Properties = Properties;
                ViewBag.ActivityId = newData.ActivityId;
                ViewBag.BusinessId = id;
                ViewBag.OldProperties = _mapper.Map<List<Business2Facility>>(_business2FacilityService.FillJSTree(id));
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                }
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult NewDetails(int id,long? activityId,string decision, List<Business2FacilityViewModel> collection)
        {
            if (string.IsNullOrWhiteSpace(decision))
            {
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                return View();
            }
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
                return View();
            }
            decision = decision.Trim().ToLower();
            if (decision != "accept" && decision != "reject")
            {
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                return View(collection);
            }
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    item.BusinessId = id;
                    item.Priority = item.Priority.HasValue ? item.Priority : 0;
                }
            }
            switch (decision)
            {
                case "accept":
                    return AcceptNewDetails(id, activityId, collection);
                case "reject":
                    return RejectNewDetails(id, activityId, collection);
                default:
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                    ViewBag.Properties = _mapper.Map<List<Business2Facility>>(collection);
                    ViewBag.PropertyTree = _mapper.Map<List<TreeModel>>(_facilityService.GetAdmittables(id));
                    ViewBag.ActivityId = activityId;
                    ViewBag.BusinessId = id;

                    return View();
            }
        }
        [HttpPost]
        private ActionResult RejectNewDetails(int id, long? activityId, List<Business2FacilityViewModel> collection)
        {
            var model = _businessService.GetById(id);
            model.LastFacilityData = null;
            _businessService.Update(model);

            var collectionModel = _mapper.Map<List<Business2FacilityItem>>(collection);

            var activity = new Activity
            {
                ActionTypeId = (byte)GeneralEnums.ActionType.Reject,
                CreatorId = LogedInAdmin.Id,
                EntityId = id,
                ParentId = activityId,
                EntityTypeId = (byte)GeneralEnums.EntityType.Business2Facility,
                Data = "{\"Business2Facility\":{\"Items\":" + JsonConvert.SerializeObject(collectionModel, Formatting.None) + "}}"
            };
            _activityService.Insert(activity);
            return RedirectToAction("Index", "Business");
        }

        [HttpPost]
        private ActionResult AcceptNewDetails(int id, long? activityId, List<Business2FacilityViewModel> collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            var t = error.ErrorMessage;
                            ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        }
                    }
                    ViewBag.BusinessId = id;
                    ViewBag.PropertyTree = _mapper.Map<List<TreeModel>>(_facilityService.Find(new Facility()));
                    return View("NewDetails");
                }

                _business2FacilityService.DeleteByBusinessId(id);

                var businessModel = _businessService.GetById(id);
                businessModel.LastFacilityData = null;
                businessModel.UpdaterId = LogedInAdmin.Id;
                _businessService.Update(businessModel);

                var collectionModel = _mapper.Map<List<Business2FacilityItem>>(collection);
                var activity = new Activity
                {
                    ActionTypeId = (byte)GeneralEnums.ActionType.Accept,
                    CreatorId = LogedInAdmin.Id,
                    EntityId = id,
                    ParentId = activityId,
                    EntityTypeId = (byte)GeneralEnums.EntityType.Business2Facility,
                    Data = "{\"Business2Facility\":{\"Items\":" + JsonConvert.SerializeObject(collectionModel, Formatting.None) + "}}"
                };
                _activityService.Insert(activity);

                if (collection == null || collection.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "تمام ویژگی ها حذف شدند");
                    ViewBag.BusinessId = id;
                    ViewBag.PropertyTree = _mapper.Map<List<TreeModel>>(_facilityService.Find(new Facility()));
                    return View("NewDetails");
                }
                var model = _mapper.Map<List<Business2Facility>>(collection);
                _business2FacilityService.BulkInsert(model);
                return RedirectToAction("Index", "Business");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.Duplicated);
                }
                else
                {
                    if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                }
                ViewBag.BusinessId = id;
                ViewBag.PropertyTree = _mapper.Map<List<TreeModel>>(_facilityService.Find(new Facility()));
                return View("NewDetails");
            }
        }
        [HttpGet]
        public ActionResult HistoryTimeLine(int id, int page = 1, int pageSize = 10)
        {
            ViewBag.Page = page;
            var request = new FilteredModel<Activity> { PageIndex = page, PageSize = pageSize };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var searchModel = new Activity
            {
                EntityTypeId = (byte)GeneralEnums.EntityType.Business2Facility,
                ActionTypeId = (byte)GeneralEnums.ActionType.EditRequest,
                EntityId = id
            };
            var result = _activityService.History(searchModel, out long totalCount, skip: offset, take: request.PageSize);
            var data = new List<Business2FacilityHistoryViewModel>();
            foreach (var activity in result)
            {
                try
                {
                    var jobject = JObject.Parse(activity.Data);
                    var newData = jobject["Business2Facility"]["Items"].ToObject<List<Business2FacilityViewModel>>();
                    //var xmlSerializer = new XmlSerializer(typeof(XmlBusiness2Facility), new XmlRootAttribute { ElementName = nameof(Business2Facility), IsNullable = true });
                    //var xmlBusiness = (XmlBusiness2Facility)xmlSerializer.Deserialize(activity.Data.CreateReader());
                    //var newData = _mapper.Map<List<Business2Facility>>(xmlBusiness.Business2Facility);
                    var history = new Business2FacilityHistoryViewModel
                    {
                        Items = _mapper.Map<List<Business2FacilityViewModel>>(newData),
                        ActionTypeId = (GeneralEnums.ActionType)activity.ActionTypeId,
                        CreatedAt = new PersianDateTime(activity.CreatedAt).ToString(),
                        CreatorId = activity.CreatorId


                    };
                    if (history.CreatorId != null)
                    {
                        var admin = _adminService.GetById(history.CreatorId.Value);
                        history.CreatorName = (admin != null ? admin.FullName : "نامشخص");
                    }
                    foreach (var item in history.Items)
                    {
                        var facility = _facilityService.GetById(item.FacilityId.Value);
                        item.FacilityTitle = (facility != null ? facility.Title : "داده نامعتبر");
                    }
                    data.Add(history);
                }
                catch
                {
                    data.Add(new Business2FacilityHistoryViewModel { Items = new List<Business2FacilityViewModel> { new Business2FacilityViewModel { Id = -1 } } });
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
    }
}