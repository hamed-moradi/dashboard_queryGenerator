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
    public class FacilityRequestItemController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IFacilityRequestItemService _facilityRequestItemService;

        public FacilityRequestItemController(IFacilityRequestItemService facilityRequestItemService, IMapper mapper, ILog4Net logger)
        {
            _facilityRequestItemService = facilityRequestItemService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Property
        [HttpGet]
        public ActionResult Index(int facilityRequestId)
        {
            ViewBag.FacilityRequestId = facilityRequestId;
            try
            {
                ViewBag.SearchModel = new FacilityRequestItemViewModel();
                var request = new FilteredModel<ServiceRequestItem>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<FacilityRequestItemViewModel>>(_facilityRequestItemService.GetPaging(new ServiceRequestItem { ServiceRequestId = facilityRequestId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<FacilityRequestItemViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Property
        [HttpPost]
        public ActionResult Index(FacilityRequestItemViewModel collection)
        {
            var request = new FilteredModel<ServiceRequestItem>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<FacilityRequestItemViewModel>>(_facilityRequestItemService.GetPaging(_mapper.Map<ServiceRequestItem>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<FacilityRequestItemViewModel>>(_facilityRequestItemService.GetPaging(_mapper.Map<ServiceRequestItem>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<FacilityRequestItemViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            ViewBag.FacilityRequestId = collection.FacilityRequestId;
            return View();
        }

        // GET: Property/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<FacilityRequestItemViewModel>(_facilityRequestItemService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Property/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, FacilityRequestItemViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    _facilityRequestItemService.Delete(_mapper.Map<ServiceRequestItem>(collection));
                    return RedirectToAction("Index", new { facilityRequestId = collection.FacilityRequestId });
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

        // GET: Property/Edit/5
        public ActionResult Edit(int id)
        {
            var viewModel = _mapper.Map<FacilityRequestItemViewModel>(_facilityRequestItemService.GetById(id));
            return View(viewModel);
        }

        // POST: Property/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, FacilityRequestItemViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<ServiceRequestItem>(collection);
                        _facilityRequestItemService.Update(model);
                        return RedirectToAction("Index", new { facilityRequestId = collection.FacilityRequestId });
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