using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asset.Infrastructure._App;
using AutoMapper;
using Domain.Model.Entities;
using Domain.Application;
using PagedList;
using Presentation.Panel.Models;
using Presentation.Panel.FilterAttributes;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Controllers
{
    public class Category2FacilityController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly ICategoryService _categoryService;
        private readonly IFacilityService _facilityService;
        private readonly ICategory2FacilityService _category2FacilityService;
        private readonly IDropDownService _dropDownService;

        public Category2FacilityController(IFacilityService facilityService,ICategoryService categoryService,ICategory2FacilityService category2FacilityService, IDropDownService dropDownService, IMapper mapper, ILog4Net logger)
        {
            _facilityService = facilityService;
            _categoryService = categoryService;
            _category2FacilityService = category2FacilityService;
            _dropDownService = dropDownService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Category2Property
        [HttpGet]
        public ActionResult Index(int categoryId)
        {
            ViewBag.CategoryId = categoryId;
            try
            {
                ViewBag.SearchModel = new Category2FacilityViewModel();
                var request = new FilteredModel<Category2Facility>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<Category2FacilityViewModel>>(_category2FacilityService.GetPaging(new Category2Facility { CategoryId = categoryId },
                    out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<Category2FacilityViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Category2Property
        [HttpPost]
        public ActionResult Index(Category2FacilityViewModel collection)
        {
            ViewBag.CategoryId = collection.CategoryId;
            var request = new FilteredModel<Category2Facility>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<Category2FacilityViewModel>>(_category2FacilityService.GetPaging(_mapper.Map<Category2Facility>(collection),
                out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<Category2FacilityViewModel>>(_category2FacilityService.GetPaging(_mapper.Map<Category2Facility>(collection),
                    out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<Category2FacilityViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Category2Property/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<Category2FacilityViewModel>(_category2FacilityService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Category2Property/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, Category2FacilityViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    _category2FacilityService.Delete(id);
                    return RedirectToAction("Index", new { categoryId = collection.CategoryId });
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
                return View(collection);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View(collection);
            }
        }

        // GET: Category2Property/Create
        public ActionResult Create(int categoryId)
        {
            ViewBag.Propertties = new SelectList(_dropDownService.GetProperties(), "id", "name", null);
            return View(new Category2FacilityViewModel { CategoryId = categoryId });
        }

        // POST: Category2Property/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(Category2FacilityViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Category2Facility>(collection);
                    _category2FacilityService.Insert(model);
                    ModelState.Clear();
                    TempData["Success"] = "با موفقیت ثبت شد.";
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
            ViewBag.Propertties = new SelectList(_dropDownService.GetProperties(), "id", "name", null);
            return View(collection);
        }
        // GET: Category2Facility/Manage
        public ActionResult Manage(int facilityId)
        {
            ViewBag.FacilityId = facilityId;
            ViewBag.Categories = _mapper.Map<List<Category2Facility>>(_category2FacilityService.FillJSTree(facilityId));
            ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category { Status = 1 }));
            return View();
        }

        // POST: Category2Facility/Manage
        [HttpPost]
        [SecurityFilter]
        public ActionResult Manage(int facilityId, List<Category2FacilityViewModel> collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var facilities = _facilityService.Find(new Facility { ParentId = facilityId }).ToList();
                    facilities.Insert(0, new Facility { Id = facilityId });
                    foreach (var f in facilities)
                    {
                        _category2FacilityService.DeleteByFacilityId(f.Id.Value);
                        if (collection != null && collection.Count > 0)
                        {
                            var model = _mapper.Map<List<Category2Facility>>(collection);
                            foreach (var item in model)
                            {
                                item.FacilityId = f.Id.Value;
                            }
                            _category2FacilityService.BulkInsert(model);
                        }
                    }
                    if (collection != null && collection.Count > 0)
                    {
                        return RedirectToAction("Index", "Facility");
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
            ViewBag.BusinessId = facilityId;
            ViewBag.Categories = _mapper.Map<List<Category2Business>>(_category2FacilityService.FillJSTree(facilityId));
            ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category()));
            return View();
        }
    }
}