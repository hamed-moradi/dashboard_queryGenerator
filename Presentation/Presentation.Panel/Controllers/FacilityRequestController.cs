using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using MD.PersianDateTime;
using PagedList;
using Presentation.Panel.FilterAttributes;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class FacilityRequestController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IFacilityRequestService _facilityRequestService;
        private readonly IFacilityRequestItemService _facilityRequestItemService;
        private readonly IFacilityRequestItem2BusinessService _facilityRequestItem2BusinessService;
        private readonly ICustomerService _customerService;
        private readonly IBusinessService _businessService;

        public FacilityRequestController(IFacilityRequestItem2BusinessService facilityRequestItem2BusinessService,IFacilityRequestItemService facilityRequestItemService,IFacilityRequestService facilityRequestService, IMapper mapper, ILog4Net logger, ICustomerService customerService, IBusinessService businessService)
        {
            _facilityRequestItemService = facilityRequestItemService;
            _facilityRequestService = facilityRequestService;
            _customerService = customerService;
            _mapper = mapper;
            _logger = logger;
            _businessService = businessService;
            _facilityRequestItem2BusinessService = facilityRequestItem2BusinessService;
        }

        #endregion

        // GET: Property
        [HttpGet]
        public ActionResult Index(int? customerId)
        {
            ViewBag.CustomerId = customerId;
            try
            {
                ViewBag.SearchModel = new FacilityRequestViewModel();
                var request = new FilteredModel<ServiceRequest>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var t = _facilityRequestService.GetPaging(new ServiceRequest { CustomerId = customerId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize);
                var result = _mapper.Map<IList<FacilityRequestViewModel>>(t);
                ViewBag.OnePageOfEntries = new StaticPagedList<FacilityRequestViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Property
        [HttpPost]
        public ActionResult Index(int? customerId,FacilityRequestViewModel collection)
        {
            var request = new FilteredModel<ServiceRequest>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var mappedModel = _mapper.Map<ServiceRequest>(collection);
            if(!collection.Latitude.HasValue || !collection.Longitude.HasValue)
            {
                mappedModel.Location = null;
            }
            var offset = (request.PageIndex - 1) * request.PageSize;
            mappedModel.StartTime = mappedModel.EndTime = null;
            mappedModel.Location = null;
            var result = _mapper.Map<IList<FacilityRequestViewModel>>(_facilityRequestService.GetPaging(mappedModel, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
       
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<FacilityRequestViewModel>>(_facilityRequestService.GetPaging(mappedModel, out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<FacilityRequestViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            ViewBag.CustomerId = collection.CustomerId;
            return View();
        }

        // GET: Property/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            ViewBag.FacilityRequestId = id;
            ViewBag.SearchModel = new BusinessViewModel { };
            var data = DetailsData(id, null);
            ViewBag.Dates = data.Dates;
            ViewBag.Customer = data.Customer;
            ViewBag.OnePageOfEntries = data.Businesses;
            ViewBag.FacilityRequestItems = data.FacilityRequestItems;
            return View(data.Model);
        }

        [HttpPost]
        public ActionResult Details(int id, BusinessViewModel collection)
        {
            ViewBag.FacilityRequestId = id;
            ViewBag.SearchModel = collection;
            var data = DetailsData(id, collection);
            ViewBag.Dates = data.Dates;
            ViewBag.Customer = data.Customer;
            ViewBag.OnePageOfEntries = data.Businesses;
            ViewBag.FacilityRequestItems = data.FacilityRequestItems;
            return View(data.Model);
        }

        private (FacilityRequestViewModel Model, SelectList Dates, CustomerViewModel Customer, StaticPagedList<BusinessViewModel> Businesses, List<FacilityRequestItemViewModel> FacilityRequestItems) DetailsData(int id, BusinessViewModel collection)
        {
            var req = _facilityRequestService.GetById(id);
            var model = _mapper.Map<FacilityRequestViewModel>(req);

            var dates = new List<SelectListItem>();
            var days = (req.EndDate - req.StartDate).Value.TotalDays;
            for (int i = 0; i <= days; i++)
            {
                var date = new PersianDateTime(req.StartDate.Value.AddDays(i)).ToShortDateString();
                dates.Add(new SelectListItem { Text = date, Value = date, Selected = (i == 0) });
            }
            var request = (collection == null ? new FilteredModel<Business>() : new FilteredModel<Business>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            });
            CustomerViewModel customer = null;
            if (model != null && model.CustomerId.HasValue)
            {
                var c = _customerService.GetById(model.CustomerId.Value);
                if (c != null)
                {
                    customer = _mapper.Map<CustomerViewModel>(c);
                }
            }

            long totalCount;
            var items = _facilityRequestItemService.GetPaging(new ServiceRequestItem { ServiceRequestId = id }, out totalCount, take: int.MaxValue);

            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<BusinessViewModel>>(_businessService.GetPagingByServiceRequestId(id, new Business(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<BusinessViewModel>>(_businessService.GetPagingByServiceRequestId(id, new Business(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            
            return (model, new SelectList(dates, "Value", "Text"), customer, new StaticPagedList<BusinessViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount), _mapper.Map<List<FacilityRequestItemViewModel>>(items));
        }

        public ActionResult Edit(int id)
        {
            var viewModel = _mapper.Map<FacilityRequestViewModel>(_facilityRequestService.GetById(id));
            return View(viewModel);
        }

        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, FacilityRequestViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<ServiceRequest>(collection);
                        _facilityRequestService.Update(model);
                        return RedirectToAction("Index", new { customerId = collection.CustomerId });
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

        [HttpPost]
        public ActionResult CancelCustomer(HandleFacilityRequestViewModel model)
        {
            //var result = _facilityRequestService.HandleFacilityRequestByIdForBusiness(LogedInAdmin.Id, model.BusinessId, model.ProviderId, model.ServiceRequest2BusinessId, (int)GeneralEnums.FacilityRequestProviderResponseStatus.Reject);
            //if (!Enum.IsDefined(typeof(GeneralEnums.FacilityRequestServiceResponseStatus), result))
            //{
            //    return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = "خطای نامشخصی رخ داده است." });
            //}
            //switch (result)
            //{
            //    case (int)GeneralEnums.FacilityRequestServiceResponseStatus.Success:
                    return Json(new JsonViewModel { status = HttpStatusCode.OK, message = EnumHelper<GeneralEnums.ServiceRequest2BusinessStatusType>.GetDisplayValue(GeneralEnums.ServiceRequest2BusinessStatusType.RejectedByProvider), data = model });
            //    default:
            //        return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = EnumHelper<GeneralEnums.FacilityRequestServiceResponseStatus>.GetDisplayValue((GeneralEnums.FacilityRequestServiceResponseStatus)result) });
            //}
        }

        [HttpPost]
        public ActionResult Reject(HandleFacilityRequestViewModel model)
        {
            var result = _facilityRequestService.HandleFacilityRequestByIdForBusiness(LogedInAdmin.Id, model.BusinessId, model.ProviderId, model.ServiceRequest2BusinessId, (int)GeneralEnums.FacilityRequestProviderResponseStatus.Reject);
            if (!Enum.IsDefined(typeof(GeneralEnums.FacilityRequestServiceResponseStatus), result))
            {
                return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = "خطای نامشخصی رخ داده است." });
            }
            switch (result)
            {
                case (int)GeneralEnums.FacilityRequestServiceResponseStatus.Success:
                    return Json(new JsonViewModel { status = HttpStatusCode.OK, message = EnumHelper<GeneralEnums.ServiceRequest2BusinessStatusType>.GetDisplayValue(GeneralEnums.ServiceRequest2BusinessStatusType.RejectedByProvider),data=model });
                default:
                    return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = EnumHelper<GeneralEnums.FacilityRequestServiceResponseStatus>.GetDisplayValue((GeneralEnums.FacilityRequestServiceResponseStatus)result) });
            }
        }
        [HttpPost]
        public ActionResult Accept(HandleFacilityRequestViewModel model)
        {
            var result = _facilityRequestService.HandleFacilityRequestByIdForBusiness(LogedInAdmin.Id, model.BusinessId, model.ProviderId, model.ServiceRequest2BusinessId, (int)GeneralEnums.FacilityRequestProviderResponseStatus.Accept);
            if (!Enum.IsDefined(typeof(GeneralEnums.FacilityRequestServiceResponseStatus), result))
            {
                return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = "خطای نامشخصی رخ داده است." });
            }
            switch (result)
            {
                case (int)GeneralEnums.FacilityRequestServiceResponseStatus.Success:
                    return Json(new JsonViewModel { status = HttpStatusCode.OK, message = EnumHelper<GeneralEnums.ServiceRequest2BusinessStatusType>.GetDisplayValue(GeneralEnums.ServiceRequest2BusinessStatusType.AcceptedByProvider), data = model });
                default:
                    return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = EnumHelper<GeneralEnums.FacilityRequestServiceResponseStatus>.GetDisplayValue((GeneralEnums.FacilityRequestServiceResponseStatus)result) });
            }
        }
        [HttpPost]
        public ActionResult CreatePreInvoice(CreatePreInvoiceViewModel model)
        {
            if (!DateTime.TryParse(model.DueTime, out DateTime dueDate))
            {
                return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = "ساعت وارد شده معتبر نیست" });
            }
            if (!PersianDateTime.TryParse(model.DueDate, out PersianDateTime persianDueDate, @"/"))
            {
                return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = "تاریخ وارد شده معتبر نیست" });
            }
            dueDate = persianDueDate.ToDateTime().Add(dueDate.TimeOfDay);

            var table = new DataTable("ParentTable");
            table.Columns.Add(new DataColumn("FacilityRequest2BusinessItemId", typeof(long)));
            table.Columns.Add(new DataColumn("Price", typeof(decimal)));
            table.Columns.Add(new DataColumn("Description", typeof(string)));
            var serviceRequestItem2Business = _facilityRequestItem2BusinessService.GetPaging(new ServiceRequestItem2Business { Status=3, BusinessId = model.BusinessId, ServiceRequestId = model.ServiceRequestId }, out long totalCount, take: int.MaxValue);
            foreach (var invoiceItem in model.InvoiceItems)
            {
                if (!serviceRequestItem2Business.Any(x => x.ServiceRequestItemId == invoiceItem.FacilityRequest2BusinessItemId))
                {
                    return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = "فاکتور شامل سرویس هایی است که مورد درخواست نبوده اند." });
                }
                var row = table.NewRow();
                row["FacilityRequest2BusinessItemId"] = serviceRequestItem2Business.First(x => x.ServiceRequestItemId.Value == invoiceItem.FacilityRequest2BusinessItemId).Id.Value;
                row["Price"] = invoiceItem.Price;
                row["Description"] = null;
                table.Rows.Add(row);
            }
            var result = _facilityRequestService.AddInvoice(LogedInAdmin.Id, model.BusinessId, model.ProviderId, model.ServiceRequest2BusinessId , model.Description, dueDate, table, model.Fee);
            if (!Enum.IsDefined(typeof(GeneralEnums.AddInvoiceResponseStatus), result))
            {
                return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = "خطای نامشخصی رخ داده است." });
            }
            switch (result)
            {
                case (int)GeneralEnums.AddInvoiceResponseStatus.Success:
                    return Json(new JsonViewModel { status = HttpStatusCode.OK, message = EnumHelper<GeneralEnums.ServiceRequest2BusinessStatusType>.GetDisplayValue(GeneralEnums.ServiceRequest2BusinessStatusType.SentPreInvoice), data = model });
                default:
                    return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = EnumHelper<GeneralEnums.AddInvoiceResponseStatus>.GetDisplayValue((GeneralEnums.AddInvoiceResponseStatus)result) });
            }
        }
    }
}