using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using PagedList;
using Presentation.Panel.FilterAttributes;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class ServiceRequestReviewController : BaseController
    {

        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IServiceRequestReviewService _serviceRequestReviewService;

        public ServiceRequestReviewController(IServiceRequestReviewService serviceRequestReviewService, IMapper mapper, ILog4Net logger)
        {
            _serviceRequestReviewService = serviceRequestReviewService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: ServiceRequestReview
        public ActionResult Index()
        {
            //ViewBag.commentEntityId = commentEntityId;
            try
            {
                ViewBag.SearchModel = new ServiceRequestReviewViewModel();
                var request = new FilteredModel<ServiceRequestReview>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ServiceRequestReviewViewModel>>(_serviceRequestReviewService.GetPaging(new ServiceRequestReview () , out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ServiceRequestReviewViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: ServiceRequestReview
        [HttpPost]
        public ActionResult Index(ServiceRequestReviewViewModel collection)
        {
            var request = new FilteredModel<ServiceRequestReview>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<ServiceRequestReviewViewModel>>(_serviceRequestReviewService.GetPaging(_mapper.Map<ServiceRequestReview>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<ServiceRequestReviewViewModel>>(_serviceRequestReviewService.GetPaging(_mapper.Map<ServiceRequestReview>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ServiceRequestReviewViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: ServiceRequestReview/Details/5
        public ActionResult Details(int id)
        {
            ServiceRequestReviewViewModel serviceRequestReviewViewModel;
            try
            {
                serviceRequestReviewViewModel = _mapper.Map<ServiceRequestReviewViewModel>(_serviceRequestReviewService.GetById(id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }

            return View(serviceRequestReviewViewModel);
        }


        // GET: ServiceRequestReview/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequestReview/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ServiceRequestReview/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<ServiceRequestReviewViewModel>(_serviceRequestReviewService.GetById(id));
            return View(model);
        }

        // POST: ServiceRequestReview/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(ServiceRequestReviewViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<ServiceRequestReview>(collection);
                    _serviceRequestReviewService.Update(model);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                return View(collection);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View(collection);
            }
        }
        
    }
}
