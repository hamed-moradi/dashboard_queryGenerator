using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using Domain.Application;
using PagedList;
using Presentation.Panel.Models;
using Presentation.Panel.FilterAttributes;
using System.Xml.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Presentation.Panel.Controllers
{
    public class Category2BusinessController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly ICategory2BusinessService _category2BusinessService;
        private readonly ICategoryService _categoryService;
        private readonly IDropDownService _dropDownService;
        private readonly IBusinessService _businessService;
        private readonly IActivityService _activityService;

        public Category2BusinessController(ICategory2BusinessService category2BusinessService, ICategoryService categoryService, IDropDownService dropDownService, IMapper mapper, ILog4Net logger, IBusinessService businessService, IActivityService activityService)
        {
            _category2BusinessService = category2BusinessService;
            _categoryService = categoryService;
            _dropDownService = dropDownService;
            _mapper = mapper;
            _logger = logger;
            _businessService = businessService;
            _activityService = activityService;
        }

        #endregion

        // GET: Category2Business
        [HttpGet]
        public ActionResult Index(int categoryId)
        {
            ViewBag.CategoryId = categoryId;
            try
            {
                ViewBag.SearchModel = new Category2BusinessViewModel();
                var request = new FilteredModel<Category2Business>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<Category2BusinessViewModel>>(_category2BusinessService.GetPaging(new Category2Business { CategoryId = categoryId },
                    out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<Category2BusinessViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Category2Business
        [HttpPost]
        public ActionResult Index(Category2BusinessViewModel collection)
        {
            ViewBag.CategoryId = collection.CategoryId;
            var request = new FilteredModel<Category2Business>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<Category2BusinessViewModel>>(_category2BusinessService.GetPaging(_mapper.Map<Category2Business>(collection),
                out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<Category2BusinessViewModel>>(_category2BusinessService.GetPaging(_mapper.Map<Category2Business>(collection),
                    out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<Category2BusinessViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Category2Business/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(_mapper.Map<Category2BusinessViewModel>(_category2BusinessService.GetById(id)));
        }

        // POST: Category2Business/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, Category2BusinessViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    _category2BusinessService.Delete(id);
                    return RedirectToAction("Index", new { categoryId = collection.CategoryId });
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }

        // GET: Category2Business/Manage
        public ActionResult Manage(int businessId)
        {
            ViewBag.BusinessId = businessId;
            var business = _businessService.GetById(businessId);
            if (business != null)
            {
                ViewBag.BusinessTitle = business.Title;
            }
            //var categories = _category2BusinessService.Find(new Category2Business { ProviderId = providerId });
            ViewBag.Categories = _mapper.Map<List<Category2Business>>(_category2BusinessService.FillJSTree(businessId));
            ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.GetAdmitables());
            return View();
        }

        // POST: Category2Business/Manage
        [HttpPost]
        [SecurityFilter]
        public ActionResult Manage(int businessId, List<Category2BusinessViewModel> collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _category2BusinessService.DeleteByBusinessId(businessId);
                    if (collection != null && collection.Count > 0)
                    {
                        foreach (var item in collection)
                        {
                            item.BusinessId = businessId;
                        }
                        var model = _mapper.Map<List<Category2Business>>(collection);
                        
                        _category2BusinessService.BulkInsert(model);
                        var data = new Category2BusinessJson { Items = _mapper.Map<List<Category2BusinessItem>>(collection) };
                        var activity = new Activity
                        {
                            ActionTypeId = (byte)GeneralEnums.ActionType.Edit,
                            CreatorId = LogedInAdmin.Id,
                            EntityId = businessId,
                            EntityTypeId = (byte)GeneralEnums.EntityType.Category2Business,
                            Data = "{\"Category2Business\":" + JsonConvert.SerializeObject(data) + "}"
                        };
                        _activityService.Insert(activity);
                        return RedirectToAction("Index", "Business");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "تمام دسته بندی ها حذف شدند");
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
            ViewBag.Categories = _mapper.Map<List<Category2Business>>(_category2BusinessService.FillJSTree(businessId));
            ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.GetAdmitables());
            return View();
        }
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
                if (model.LastCategoryData == null)
                {
                    ModelState.AddModelError(string.Empty, "درخواست جدیدی برای ویرایش دسته بندی های این کسب و کار یافت نشد.");
                    return View();
                }
                var jobject = JObject.Parse(model.LastCategoryData);
                var newData = jobject["Category2Business"].ToObject<Category2BusinessJson>();
                var categories = _mapper.Map<List<Category2Business>>(newData.Items);

                var admitables = _categoryService.GetAdmitables();
                if (categories.Any(x => !admitables.Any(a => a.Id == x.CategoryId)))
                {
                    ModelState.AddModelError(string.Empty, "درخواست کاربر شامل دسته بندی های غیر مجاز است. شما می توانید این درخواست را رد کنید.");
                    ViewBag.JustReject = true;
                }
                var parents = admitables.Where(x => x.ParentId == null);
                var finalCategories = new List<Category2Business>();
                foreach (var p in categories)
                {
                    if (!parents.Any(x => x.Id == p.CategoryId))
                    {
                        finalCategories.Add(p);
                        continue;
                    }
                    var children = admitables.Where(x => x.ParentId == p.CategoryId);
                    var result = categories.Where(p2 => children.Any(c => p2.CategoryId == c.Id)).ToList();
                    if (result.Count == 0)
                    {
                        finalCategories.Add(p);
                    }
                }
                ViewBag.BusinessId = id;
                ViewBag.Categories = categories;
                ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(admitables);
                ViewBag.FinalCategories = finalCategories;
                ViewBag.ActivityId = newData.ActivityId;
                ViewBag.OldCategories = _mapper.Map<List<Category2Business>>(_category2BusinessService.FillJSTree(id));
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
        public ActionResult NewDetails(int id, long? activityId, string decision, List<Category2BusinessViewModel> collection)
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
                    ViewBag.Categories = _mapper.Map<List<Category2Business>>(collection);
                    ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category { Status = 1 }));
                    ViewBag.ActivityId = activityId;
                    ViewBag.BusinessId = id;
                    return View();
            }
        }
        [HttpPost]
        private ActionResult RejectNewDetails(int id, long? activityId, List<Category2BusinessViewModel> collection)
        {
            var model = _businessService.GetById(id);
            model.LastCategoryData = null;
            _businessService.Update(model);

            var collectionModel = _mapper.Map<List<Category2BusinessItem>>(collection);
            var activity = new Activity
            {
                ActionTypeId = (byte)GeneralEnums.ActionType.Reject,
                CreatorId = LogedInAdmin.Id,
                EntityId = id,
                ParentId = activityId,
                EntityTypeId = (byte)GeneralEnums.EntityType.Category2Business,
                Data = "{\"Category2Business\":{\"Items\":" + JsonConvert.SerializeObject(collectionModel, Formatting.None) + "}}"
            };
            _activityService.Insert(activity);
            return RedirectToAction("Index", "Business");
        }
        [HttpPost]
        private ActionResult AcceptNewDetails(int id, long? activityId, List<Category2BusinessViewModel> collection)
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
                    ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category { Status = 1 }));
                    return View("NewDetails");
                }
                _category2BusinessService.DeleteByBusinessId(id);

                var businessModel = _businessService.GetById(id);
                businessModel.LastCategoryData = null;
                _businessService.Update(businessModel);

                var collectionModel = _mapper.Map<List<Category2BusinessItem>>(collection);
                var activity = new Activity
                {
                    ActionTypeId = (byte)GeneralEnums.ActionType.Accept,
                    CreatorId = LogedInAdmin.Id,
                    EntityId = id,
                    ParentId = activityId,
                    EntityTypeId = (byte)GeneralEnums.EntityType.Category2Business,
                    Data = "{\"Category2Business\":{\"Items\":" + JsonConvert.SerializeObject(collectionModel, Formatting.None) + "}}"
                };
                _activityService.Insert(activity);

                if (collection == null || collection.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "تمام ویژگی ها حذف شدند");
                    ViewBag.BusinessId = id;
                    ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category { Status = 1 }));
                    return View("NewDetails");
                }
                var model = _mapper.Map<List<Category2Business>>(collection);
                _category2BusinessService.BulkInsert(model);
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
                ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category { Status = 1 }));
                return View("NewDetails");
            }
        }
    }
}