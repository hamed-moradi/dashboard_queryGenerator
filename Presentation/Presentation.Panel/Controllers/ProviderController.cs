using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using Presentation.Panel.FilterAttributes;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class ProviderController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IProviderService _providerService;
        private readonly IDropDownService _dropDownService;
        private readonly IActivityService _activityService;
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public ProviderController(IProviderService providerService, IDropDownService dropDownService, IMapper mapper, ILog4Net logger, IActivityService activityService, IUserService userService, IAdminService adminService)
        {
            _providerService = providerService;
            _dropDownService = dropDownService;
            _mapper = mapper;
            _logger = logger;
            _activityService = activityService;
            _userService = userService;
            _adminService = adminService;
        }

        #endregion

        // GET: Person
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new ProviderViewModel { };
                var request = new FilteredModel<User> { };
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ProviderViewModel>>(_providerService.GetPaging(new User(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ProviderViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Person
        [HttpPost]
        public ActionResult Index(ProviderViewModel collection)
        {
            if (!string.IsNullOrWhiteSpace(collection.CellPhone) && collection.CellPhone.StartsWith("0"))
            {
                collection.CellPhone=collection.CellPhone.Remove(0, 1);
            }
            var request = new FilteredModel<User>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var user = _mapper.Map<User>(collection);
            user.Password = null;
            var result = _mapper.Map<IList<ProviderViewModel>>(_providerService.GetPaging(user, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<ProviderViewModel>>(_providerService.GetPaging(user, out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ProviderViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Person/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(_mapper.Map<ProviderViewModel>(_providerService.GetById(id)));
        }

        //// POST: Person/Details/5
        //[HttpPost]
        //[SecurityFilter]
        //public ActionResult Details(int id, ProviderViewModel collection)
        //{
        //    try
        //    {
        //        if (id > 0)
        //        {
        //            collection.UpdaterId = LogedInAdmin.Id;
        //            _providerService.Delete(_mapper.Map<User>(collection));
        //            return RedirectToAction("Index");
        //        }
        //        ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex);
        //        if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
        //    }
        //    return View(collection);
        //}

        // GET: Provider/NewDetails/5
        [HttpGet]
        public ActionResult NewDetails(int id)
        {
            try
            {
                var model = _providerService.GetById(id);
                if (model != null)
                {
                    if (model.LastEntityData != null)
                    {
                        var jobject= JObject.Parse(model.LastEntityData);
                        var newData = jobject["User"].ToObject<User>();
                        var oldData = _mapper.Map<ProviderViewModel>(model);
                        ViewBag.NewData = _mapper.Map<ProviderViewModel>(newData);
                        ViewBag.OldData = _mapper.Map<ProviderViewModel>(oldData);

                        var myProviderInfo = newData.GetType().GetProperties();
                        foreach (var item in myProviderInfo)
                        {
                            var key = item.Name;
                            var value = item.GetValue(newData, null);
                            if (value != null)
                            {
                                var propertyInfo = model.GetType().GetProperty(key);
                                var setMethod = propertyInfo.GetSetMethod();
                                if (setMethod != null)
                                {
                                    propertyInfo.SetValue(model, value, null);
                                }
                            }
                        }
                        model.ActivityId = newData.ActivityId;
                        var viewModel = _mapper.Map<ProviderViewModel>(model);
                        return View(viewModel);
                    }
                    ModelState.AddModelError(string.Empty, "درخواست جدیدی برای ویرایش این خدمات دهنده یافت نشد.");
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
            return View(new ProviderViewModel());
        }

        // POST: Provider/NewDetails/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult NewDetails(int id, ProviderViewModel collection, string decision)
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
        private ActionResult AcceptNewDetails(int id, ProviderViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    var model = _mapper.Map<User>(collection);
                    model.LastEntityData = null;
                    _providerService.Update(model);

                    var activity = new Activity
                    {
                        ActionTypeId = (byte)GeneralEnums.ActionType.Accept,
                        CreatorId = LogedInAdmin.Id,
                        EntityId = id,
                        EntityTypeId = (byte)GeneralEnums.EntityType.User,
                        Data ="{\"User\":"+ JsonConvert.SerializeObject(model, Formatting.None)+"}"
                    };
                    foreach (var pid in model.ActivityId)
                    {
                        activity.ParentId = pid;
                        _activityService.Insert(activity);
                    }
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
        [HttpPost]
        private ActionResult RejectNewDetails(int id, ProviderViewModel collection)
        {
            var model = _providerService.GetById(id);
            model.LastEntityData = null;
            _providerService.Update(model);
            var activity = new Activity
            {
                ActionTypeId = (byte)GeneralEnums.ActionType.Reject,
                CreatorId = LogedInAdmin.Id,
                EntityId = id,
                EntityTypeId = (byte)GeneralEnums.EntityType.User,
                Data = "{\"User\":" + JsonConvert.SerializeObject(model, Formatting.None) + "}"
            };
            foreach (var pid in collection.ActivityId)
            {
                activity.ParentId = pid;
                _activityService.Insert(activity);
            }
            return RedirectToAction("Index");
        }

        // GET: Person/Create/5
        public ActionResult Create()
        {
            return View();
        }

        // POST: Person/Create/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(ProviderViewModel collection)
        {
            collection.IdentityProviderId = GeneralEnums.IdentityProvider.Dashboard;
            collection.UserTypeId = GeneralEnums.UserType.Provider;

            ModelState["UserTypeId"].Errors.Clear();
            UpdateModel(collection);
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<User>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    if (model.CellPhone != null && model.CellPhone.StartsWith("0"))
                    {
                        model.CellPhone = model.CellPhone.Remove(0, 1);
                    }
                    if (!string.IsNullOrWhiteSpace(model.CellPhone))
                    {
                        model.CellPhoneVerifiedAt = DateTime.Now;
                    }
                    if (!string.IsNullOrWhiteSpace(model.Email))
                    {
                        model.EmailVerifiedAt = DateTime.Now;
                    }
                    var result = _providerService.Insert(model,(byte)collection.EvidenceStatusId.GetValueOrDefault(GeneralEnums.EvidenceStatusType.Unknown));
                    if (result > 0)
                    {
                        model.Id = result;
                        var activity = new Activity
                        {
                            ActionTypeId = (byte)GeneralEnums.ActionType.Create,
                            CreatorId = LogedInAdmin.Id,
                            EntityId = result,
                            EntityTypeId = (byte)GeneralEnums.EntityType.User,
                            Data = "{\"User\":" + JsonConvert.SerializeObject(model, Formatting.None) + "}"
                        };
                        _activityService.Insert(activity);
                        return RedirectToAction("Index");
                    }
                    if(Enum.IsDefined(typeof(GeneralEnums.UserInsertError), result) == true)
                    {
                        ModelState.AddModelError(string.Empty, EnumHelper<GeneralEnums.UserInsertError>.GetDisplayValue((GeneralEnums.UserInsertError)result));
                        return View(collection);
                    }
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            var temp = _providerService.GetById(id);
            var model = _mapper.Map<ProviderViewModel>(temp);
            return View(model);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, ProviderViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<User>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _providerService.Update(model);
                        var activity = new Activity
                        {
                            ActionTypeId = (byte)GeneralEnums.ActionType.Edit,
                            CreatorId = LogedInAdmin.Id,
                            EntityId = model.Id,
                            EntityTypeId = (byte)GeneralEnums.EntityType.User,
                            Data = "{\"User\":" + JsonConvert.SerializeObject(model, Formatting.None) + "}"
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

        [HttpGet]
        [AllowAnonymous]
        public JsonResult DropDown(string key, int? p)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length < 3) return null;
            var result = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetProviders(key, (p.GetValueOrDefault(1) - 1) * 30, 30, out long totalCount));
            return Json(new DropDownModel { TotalCount = totalCount, Items = result }, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult Verify(int id)
        //{
        //    var user = _providerService.GetById(id);
        //    var model = _mapper.Map<ProviderViewModel>(user);
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult Verify(int id)
        //{
        //    var user = _providerService.GetById(id);
        //    var result = _userService.CheckEmailCellphoneForVerify(user);
        //    if (result != 1)
        //    {
        //        if (Enum.IsDefined(typeof(GeneralEnums.UserInsertError), result) == true)
        //        {
        //            ModelState.AddModelError(string.Empty, EnumHelper<GeneralEnums.UserInsertError>.GetDisplayValue((GeneralEnums.UserInsertError)result));
        //            return View("Details", _mapper.Map<ProviderViewModel>(user));
        //        }
        //        ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
        //        return View("Details", _mapper.Map<ProviderViewModel>(user));
        //    }
        //    var date = DateTime.Now;
        //    if (!string.IsNullOrWhiteSpace(user.CellPhone))
        //    {
        //        user.CellPhoneVerifiedAt = date;
        //    }
        //    if (!string.IsNullOrWhiteSpace(user.Email))
        //    {
        //        user.EmailVerifiedAt = date;
        //    }
        //    user.Status = (byte)GeneralEnums.Status.Active;
        //    user.UpdaterId = LogedInAdmin.Id;
        //    _providerService.Update(user);
        //    ModelState.AddModelError(string.Empty, "کاربر با موفقیت تایید شد");
        //    return View("Details", _mapper.Map<ProviderViewModel>(user));
        //}
        [HttpPost]
        public ActionResult ChangeActivationStatus(int id, UserEditStatus status)
        {
            var user = _providerService.GetById(id);
            if(status== UserEditStatus.Active)
            {
                user.Status = (byte)UserEditStatus.Inactive;
            }
            else
            {
                user.Status = (byte)UserEditStatus.Active;
            }
            user.UpdaterId = LogedInAdmin.Id;
            _providerService.Update(user);
            ModelState.AddModelError(string.Empty, $"کاربر با موفقیت {(status == UserEditStatus.Active ? "غیر فعال" : "فعال")} شد");
            return View("Details", _mapper.Map<ProviderViewModel>(user));
        }
        [HttpGet]
        public ActionResult HistoryTimeLine(int id,int page = 1,int pageSize=10)
        {
            ViewBag.Page = page;
            var request = new FilteredModel<Activity> { PageIndex = page, PageSize = pageSize };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var searchModel = new Activity {
                EntityTypeId = (byte)GeneralEnums.EntityType.User,
                EntityId = id
            };
            var result = _activityService.History(searchModel, out long totalCount, skip: offset,take:request.PageSize);
            var data = new List<ProviderViewModel>();
            foreach (var activity in result)
            {
                try
                {
                    var jobject = JObject.Parse(activity.Data);
                    var newData = jobject["User"].ToObject<User>();
                    newData.CreatedAt = activity.CreatedAt;
                    var p = _mapper.Map<ProviderViewModel>(newData);
                    p.ActionTypeId = (GeneralEnums.ActionType)activity.ActionTypeId;
                    p.CreatorId = activity.CreatorId;
                    if (p.CreatorId != null)
                    {
                         var admin = _adminService.GetById(p.CreatorId.Value);
                        p.CreatorName = (admin != null ? admin.FullName : "نامشخص");
                    }
                    data.Add(p);
                }
                catch
                {
                    data.Add(new ProviderViewModel { Id = -1 });
                }
            }
            ViewData["page"] = page;
            return PartialView("_HistoryTimeLine", data);
        }
        [HttpGet]
        public ActionResult History(int id)
        {
            ViewBag.Id = id;
            var user = _providerService.GetById(id);
            if (user == null)
            {
                return View();
            }
            var provider = _mapper.Map<ProviderViewModel>(user);
            return View(provider);
        }
    }
}