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
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class PreUploadedPinFactorItemController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IPreUploadedPinFactorItemService _preUploadedPinFactorItemService;

        public PreUploadedPinFactorItemController(IPreUploadedPinFactorItemService preUploadedPinFactorItemService, IMapper mapper, ILog4Net logger)
        {
            _preUploadedPinFactorItemService = preUploadedPinFactorItemService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new PreUploadedPinFactorItemViewModel();
                var request = new FilteredModel<PreUploadedPinFactorItemViewModel>();
                var result = _mapper.Map<IList<PreUploadedPinFactorItemViewModel>>(_preUploadedPinFactorItemService.GetPaging(new PreUploadedPinFactorItem(), out long totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<PreUploadedPinFactorItemViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.Sum = _preUploadedPinFactorItemService.GetSum();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(PreUploadedPinFactorItemViewModel model)
        {
            long totalCount;
            model.Id = null;
            ViewBag.SearchModel = model;
            var request = new FilteredModel<PreUploadedPinFactorItemViewModel>
            {
                PageIndex = model.ThisPageIndex,
                Order = model.PageOrder,
                OrderBy = model.PageOrderBy
            };
            var searchModel = _mapper.Map<PreUploadedPinFactorItem>(model);
            var result = _mapper.Map<IList<PreUploadedPinFactorItemViewModel>>(_preUploadedPinFactorItemService.GetPaging(searchModel, out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<PreUploadedPinFactorItemViewModel>>(_preUploadedPinFactorItemService.GetPaging(_mapper.Map<PreUploadedPinFactorItem>(model), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<PreUploadedPinFactorItemViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.Sum = _preUploadedPinFactorItemService.GetSum();
            return View();
        }
        [HttpPost]
        [SecurityFilter]
        public ActionResult Approve(int[] ids)
        {
            try
            {
                if (ids != null && ids.Any())
                {
                    if (_preUploadedPinFactorItemService.Approve(ids) > 0)
                    {
                        return Json(new { Status = 1, Message = $"شارژهای انتخابی تائید شد.", Sum = _preUploadedPinFactorItemService.GetSum().ToString("n0") });
                    }
                    else
                    {
                        return Json(new { Status = 0, Message = GeneralMessages.ErrorInSave });
                    }
                }
                else
                {
                    return Json(new { Status = 0, Message = GeneralMessages.EmptyId });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new { Status = 0, Message = GeneralMessages.UnexpectedError });
            }
        }
        [HttpPost]
        public ActionResult UpdateGrid(int? pageIndex)
        {
            var request = new FilteredModel<PreUploadedPinFactorItemViewModel>();
            var result = _mapper.Map<IList<PreUploadedPinFactorItemViewModel>>(_preUploadedPinFactorItemService.GetPaging(new PreUploadedPinFactorItem(), out long totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            ViewBag.OnePageOfEntries = new StaticPagedList<PreUploadedPinFactorItemViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_PreUploadedPinFactorItemsGrid", ViewBag.OnePageOfEntries)
                : View();
        }
    }
}