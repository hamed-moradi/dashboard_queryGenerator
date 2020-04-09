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
    public class DiscountController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IDiscountService _discountService;
        private readonly IFacilityService _propertyService;

        public DiscountController(IDiscountService discountService, IFacilityService propertyService, IMapper mapper, ILog4Net logger)
        {
            _discountService = discountService;
            _propertyService = propertyService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Discount
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new DiscountViewModel();
                long totalCount;
                var request = new FilteredModel<Discount>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<DiscountViewModel>>(_discountService.GetPaging(new Discount(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<DiscountViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Discount
        [HttpPost]
        public ActionResult Index(DiscountViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<Discount>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<DiscountViewModel>>(_discountService.GetPaging(_mapper.Map<Discount>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<DiscountViewModel>>(_discountService.GetPaging(_mapper.Map<Discount>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<DiscountViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Discount/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<DiscountViewModel>(_discountService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Discount/Details/5
        [HttpPost]
        public ActionResult Details(int id, DiscountViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _discountService.Delete(_mapper.Map<Discount>(collection));
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

        // GET: Discount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discount/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(DiscountViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (collection.Properties != null && collection.Properties.Count > 0 && collection.Properties.Any(a => a.Id != null))
                    {
                        var model = _mapper.Map<Discount>(collection);
                        model.CreatorId = LogedInAdmin.Id;
                        foreach (var item in collection.Properties)
                        {
                            if (item.Id != null)
                            {
                                model.FacilityId = item.Id;
                                _discountService.Insert(model);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "هیچ ویژگی انتخاب نشده است");
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
            return View(collection);
        }

        // GET: Discount/Edit/5
        public ActionResult Edit(int id)
        {
            var viewModel = _mapper.Map<DiscountViewModel>(_discountService.GetById(id));
            return View(viewModel);
        }

        // POST: Discount/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, DiscountViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Discount>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _discountService.Update(model);
                        return RedirectToAction("Index");
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
            return View(collection);
        }
    }
}