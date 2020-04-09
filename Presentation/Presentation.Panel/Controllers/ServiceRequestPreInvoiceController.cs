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
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class ServiceRequestPreInvoiceController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ILog4Net logger;
        private readonly IServiceRequestPreInvoiceService serviceRequestPreInvoiceService;
        private readonly IServiceRequestPreInvoiceItemService serviceRequestPreInvoiceItemService;
        private readonly IBusinessService businessService;
        public ServiceRequestPreInvoiceController(IMapper mapper, ILog4Net logger, IServiceRequestPreInvoiceService serviceRequestPreInvoiceService, IServiceRequestPreInvoiceItemService serviceRequestPreInvoiceItemService, IBusinessService businessService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.serviceRequestPreInvoiceService = serviceRequestPreInvoiceService;
            this.serviceRequestPreInvoiceItemService = serviceRequestPreInvoiceItemService;
            this.businessService = businessService;
        }
        public ActionResult Preview(long id)
        {
            var preinvoice = serviceRequestPreInvoiceService.GetByServiceRequest2BusinessId(id);
            var items = serviceRequestPreInvoiceItemService.GetByPrefactorId(preinvoice.Id ?? 0);
            var preinvoiceViewModel = mapper.Map<ServiceRequestPreInvoiceViewModel>(preinvoice);
            var itemsViewModel = mapper.Map<IEnumerable<ServiceRequestPreInvoiceItemViewModel>>(items);
            ViewBag.PreInvoiceItems = itemsViewModel;
            return PartialView(preinvoiceViewModel);
        }
        public ActionResult Index (int id)
        {
            try
            {
                var business = businessService.GetById(id);
                if (business != null)
                {
                    ViewBag.BusinessTitle = business.Title;
                }
                ViewBag.SearchModel = new ServiceRequestPreInvoiceViewModel { BusinessId = id };
                var request = new FilteredModel<ServiceRequestPreInvoice> { };
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = mapper.Map<IList<ServiceRequestPreInvoiceViewModel>>(serviceRequestPreInvoiceService.GetPaging(new ServiceRequestPreInvoice { BusinessId = id }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ServiceRequestPreInvoiceViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index (ServiceRequestPreInvoiceViewModel collection)
        {
            var request = new FilteredModel<ServiceRequestPreInvoice>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = mapper.Map<IList<ServiceRequestPreInvoiceViewModel>>(serviceRequestPreInvoiceService.GetPaging(mapper.Map<ServiceRequestPreInvoice>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = mapper.Map<IList<ServiceRequestPreInvoiceViewModel>>(serviceRequestPreInvoiceService.GetPaging(mapper.Map<ServiceRequestPreInvoice>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ServiceRequestPreInvoiceViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            ViewBag.ProviderId = collection.ProviderId;
            return View();
        }
    }
}