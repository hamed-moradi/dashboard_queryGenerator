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
    public class BusinessReviewController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IBusinessReviewService _reviewService;
        private readonly IBusinessService _businessService;

        public BusinessReviewController(IBusinessReviewService reviewService, IMapper mapper, ILog4Net logger, IBusinessService businessService)
        {
            _reviewService = reviewService;
            _mapper = mapper;
            _logger = logger;
            _businessService = businessService;
        }

        #endregion

        [HttpGet]
        public ActionResult Index(int? businessId)
        {
            try
            {
                if (businessId.HasValue)
                {
                    var business = _businessService.GetById(businessId.Value);
                    if (business != null)
                    {
                        ViewBag.BusinessTitle = business.Title;
                    }
                }
                ViewBag.SearchModel = new ReviewViewModel { BusinessId = businessId };
                long totalCount;
                var request = new FilteredModel<BusinessReview>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ReviewViewModel>>(_reviewService.GetPaging(new BusinessReview { BusinessId = businessId }, out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ReviewViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(ReviewViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<BusinessReview>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<ReviewViewModel>>(_reviewService.GetPaging(_mapper.Map<BusinessReview>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<ReviewViewModel>>(_reviewService.GetPaging(_mapper.Map<BusinessReview>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ReviewViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<ReviewViewModel>(_reviewService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, ReviewViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _reviewService.Delete(_mapper.Map<BusinessReview>(collection));
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

        public ActionResult Edit(int id)
        {
            var viewModel = _mapper.Map<ReviewViewModel>(_reviewService.GetById(id));
            return View(viewModel);
        }

        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, ReviewViewModel collection)
        {
            try
            {
                if (id <= 0)
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
                    return View(collection);
                }
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
                    return View(collection);
                }
                var model = _mapper.Map<BusinessReview>(collection);
                _reviewService.Update(model);
                return RedirectToAction("Index");
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
                return View(collection);
            }

        }
    }
}