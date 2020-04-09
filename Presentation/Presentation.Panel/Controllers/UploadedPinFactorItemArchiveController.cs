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
    public class UploadedPinFactorItemArchiveController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IUploadedPinFactorItemArchiveService _uploadedPinFactorItemArchiveService;

        public UploadedPinFactorItemArchiveController(IUploadedPinFactorItemArchiveService uploadedPinFactorItemArchiveService, IMapper mapper, ILog4Net logger)
        {
            _uploadedPinFactorItemArchiveService = uploadedPinFactorItemArchiveService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new UploadedPinFactorItemArchiveViewModel();
                var request = new FilteredModel<UploadedPinFactorItemArchiveViewModel>();
                var result = _mapper.Map<IList<UploadedPinFactorItemArchiveViewModel>>(_uploadedPinFactorItemArchiveService.GetPaging(new UploadedPinFactorItemArchive(), out long totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<UploadedPinFactorItemArchiveViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.Sum = _uploadedPinFactorItemArchiveService.GetSum();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(UploadedPinFactorItemArchiveViewModel model)
        {
            long totalCount;
            model.Id = null;
            ViewBag.SearchModel = model;
            var request = new FilteredModel<UploadedPinFactorItemArchiveViewModel>
            {
                PageIndex = model.ThisPageIndex,
                Order = model.PageOrder,
                OrderBy = model.PageOrderBy
            };
            var searchModel = _mapper.Map<UploadedPinFactorItemArchive>(model);
            var result = _mapper.Map<IList<UploadedPinFactorItemArchiveViewModel>>(_uploadedPinFactorItemArchiveService.GetPaging(searchModel, out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<UploadedPinFactorItemArchiveViewModel>>(_uploadedPinFactorItemArchiveService.GetPaging(_mapper.Map<UploadedPinFactorItemArchive>(model), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<UploadedPinFactorItemArchiveViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.Sum = _uploadedPinFactorItemArchiveService.GetSum();
            return View();
        }
    }
}