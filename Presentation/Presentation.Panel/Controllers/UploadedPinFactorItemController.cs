using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using PagedList;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class UploadedPinFactorItemController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IUploadedPinFactorItemService _uploadedPinFactorItemService;

        public UploadedPinFactorItemController(IUploadedPinFactorItemService uploadedPinFactorItemService, IMapper mapper, ILog4Net logger)
        {
            _uploadedPinFactorItemService = uploadedPinFactorItemService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new UploadedPinFactorItemViewModel();
                var request = new FilteredModel<UploadedPinFactorItemViewModel>();
                var result = _mapper.Map<IList<UploadedPinFactorItemViewModel>>(_uploadedPinFactorItemService.GetPaging(new UploadedPinFactorItem(), out long totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<UploadedPinFactorItemViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.Sum = _uploadedPinFactorItemService.GetSum();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(UploadedPinFactorItemViewModel model)
        {
            long totalCount;
            model.Id = null;
            ViewBag.SearchModel = model;
            var request = new FilteredModel<UploadedPinFactorItemViewModel>
            {
                PageIndex = model.ThisPageIndex,
                Order = model.PageOrder,
                OrderBy = model.PageOrderBy
            };
            var searchModel = _mapper.Map<UploadedPinFactorItem>(model);
            var result = _mapper.Map<IList<UploadedPinFactorItemViewModel>>(_uploadedPinFactorItemService.GetPaging(searchModel, out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<UploadedPinFactorItemViewModel>>(_uploadedPinFactorItemService.GetPaging(_mapper.Map<UploadedPinFactorItem>(model), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<UploadedPinFactorItemViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.Sum = _uploadedPinFactorItemService.GetSum();
            return View();
        }
    }
}