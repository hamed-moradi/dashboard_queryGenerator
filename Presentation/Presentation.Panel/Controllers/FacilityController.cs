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
using System.Net;
using Presentation.Panel.FilterAttributes;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Controllers
{
    public class FacilityController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IFacilityService _facilityService;
        private readonly ICategoryService _categoryService;
        private readonly IBusinessService _businessService;
        private readonly IDropDownService _dropDownService;

        public FacilityController(IFacilityService facilityService, ICategoryService categoryService, IBusinessService businessService, IDropDownService dropDownService, IMapper mapper, ILog4Net logger)
        {
            _facilityService = facilityService;
            _categoryService = categoryService;
            _businessService = businessService;
            _dropDownService = dropDownService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Facility
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new FacilityViewModel();
                var request = new FilteredModel<Facility>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<FacilityViewModel>>(_facilityService.GetPaging(new Facility(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<FacilityViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Facility
        [HttpPost]
        public ActionResult Index(FacilityViewModel collection)
        {
            var request = new FilteredModel<Facility>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<FacilityViewModel>>(_facilityService.GetPaging(_mapper.Map<Facility>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<FacilityViewModel>>(_facilityService.GetPaging(_mapper.Map<Facility>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<FacilityViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Facility/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<FacilityViewModel>(_facilityService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Facility/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, FacilityViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _facilityService.Delete(_mapper.Map<Facility>(collection));
                    return RedirectToAction("Index");
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

        // GET: Facility/Create
        public ActionResult Create()
        {
            ViewBag.Parents = new SelectList(_dropDownService.GetProperties(), "id", "name", null);
            return View();
        }

        // POST: Facility/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(FacilityViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Facility>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    _facilityService.Insert(model);
                    ModelState.Clear();
                    TempData["Success"] = "با موفقیت ثبت شد.";
                    ViewBag.Parents = new SelectList(_dropDownService.GetProperties(), "id", "name", null);
                    return View(new FacilityViewModel { ParentId = collection.ParentId });
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
            ViewBag.Parents = new SelectList(_dropDownService.GetProperties(), "id", "name", null);
            return View(collection);
        }

        // GET: Facility/Edit/5
        public ActionResult Edit(int id)
        {
            var viewModel = _mapper.Map<FacilityViewModel>(_facilityService.GetById(id));
            ViewBag.Parents = new SelectList(_dropDownService.GetProperties(), "id", "name", null);
            return View(viewModel);
        }

        // POST: Facility/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, FacilityViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        if (collection.Id == collection.ParentId)
                        {
                            ModelState.AddModelError(string.Empty, "شما نمیتوانید ویژگی مورد نظر را بعنوان والد خود انتخاب کنید");
                        }
                        else
                        {
                            var model = _mapper.Map<Facility>(collection);
                            model.UpdaterId = LogedInAdmin.Id;
                            _facilityService.Update(model);
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                    }
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
            ViewBag.Parents = new SelectList(_dropDownService.GetProperties(), "id", "name", null);
            return View(collection);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetByBusiness(int businessId)
        {
            var response = new JsonViewModel { status = HttpStatusCode.InternalServerError, message = GeneralMessages.Error };
            try
            {
                var properties= _facilityService.GetByBusinessId(businessId);
                foreach(var item in properties)
                {
                    if (item.ParentId == null)
                    {
                        item.Children = properties.Where(w => w.ParentId == item.Id);
                    }
                }
                properties = properties.Where(w => w.ParentId == null);
                response.data = properties;
                response.status = HttpStatusCode.OK;
                response.message = string.Empty;
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                //response.message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: Businesses
        [HttpGet]
        public ActionResult Businesses(int id)
        {
            ViewBag.Id = id;
            try
            {
                ViewBag.SearchModel = new BusinessViewModel();
                var request = new FilteredModel<Facility>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<BusinessViewModel>>(_businessService.GetPagingByFacilityId(id, new Business(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<BusinessViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Businesses
        [HttpPost]
        public ActionResult Businesses(int id, BusinessViewModel collection)
        {
            var request = new FilteredModel<Facility>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<BusinessViewModel>>(_businessService.GetPagingByFacilityId(id, _mapper.Map<Business>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<BusinessViewModel>>(_businessService.GetPagingByFacilityId(id, _mapper.Map<Business>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<BusinessViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            ViewBag.Id = id;
            return View();
        }

        public ActionResult SetPriority(int id)
        {
            var request = new FilteredModel<Facility>
            {
                PageSize = int.MaxValue,
                OrderBy = nameof(Facility.Priority),
                Order = "desc"
            };
            var result = _mapper.Map<IList<FacilityViewModel>>(_facilityService.GetPaging(new Facility { ParentId = id }, out long totalCount, request.OrderBy, request.Order, 0, request.PageSize));
            return View(result);
        }
        [HttpPost]
        public ActionResult SetPriority(int id, List<FacilityViewModel> data)
        {
            foreach (var item in data)
            {
                var facility = _facilityService.GetById(item.Id.Value);
                if (facility == null)
                {
                    continue;
                }
                facility.Priority = item.Priority;
                facility.UpdaterId = LogedInAdmin.Id;
                _facilityService.Update(facility);
            }
            ModelState.AddModelError("", "عملیات با موفقیت انجام شد");
            return View(data);
        }
    }
}