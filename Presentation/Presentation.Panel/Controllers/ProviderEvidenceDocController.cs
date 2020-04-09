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
    public class ProviderEvidenceDocController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IProviderEvidenceDocService _providerEvidenceDocService;
        private readonly IBusinessService _businessService;

        public ProviderEvidenceDocController(IProviderEvidenceDocService providerEvidenceDocService, IMapper mapper, ILog4Net logger, IBusinessService businessService)
        {
            _providerEvidenceDocService = providerEvidenceDocService;
            _mapper = mapper;
            _logger = logger;
            _businessService = businessService;
        }

        #endregion

        // GET: ProviderEvidenceDoc
        [HttpGet]
        public ActionResult Index(int? providerId, int? businessId)
        {
            ViewBag.ProviderId = providerId;
            ViewBag.BusinessId = businessId;
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
                ViewBag.SearchModel = new ProviderEvidenceDocViewModel();
                var request = new FilteredModel<ProviderEvidenceDoc>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ProviderEvidenceDocViewModel>>(_providerEvidenceDocService.GetPaging(new ProviderEvidenceDoc { ProviderId = providerId, BusinessId = businessId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ProviderEvidenceDocViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: ProviderEvidenceDoc
        [HttpPost]
        public ActionResult Index(ProviderEvidenceDocViewModel collection)
        {
            var request = new FilteredModel<User>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<ProviderEvidenceDocViewModel>>(_providerEvidenceDocService.GetPaging(_mapper.Map<ProviderEvidenceDoc>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result =
                    _mapper.Map<IList<ProviderEvidenceDocViewModel>>(_providerEvidenceDocService.GetPaging(_mapper.Map<ProviderEvidenceDoc>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ProviderEvidenceDocViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            ViewBag.ProviderId = collection.ProviderId;
            ViewBag.BusinessId = collection.BusinessId;
            return View();
        }

        // GET: ProviderEvidenceDoc/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(_mapper.Map<ProviderEvidenceDocViewModel>(_providerEvidenceDocService.GetById(id)));
        }

        // POST: ProviderEvidenceDoc/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, ProviderEvidenceDocViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    _providerEvidenceDocService.Delete(_mapper.Map<ProviderEvidenceDoc>(collection));
                    return RedirectToAction("Index", new { providerId = collection.ProviderId, businessId = collection.BusinessId });
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

        // GET: ProviderEvidenceDoc/Create/5
        public ActionResult Create(int providerId, int? businessId)
        {
            return View(new ProviderEvidenceDocViewModel { ProviderId = providerId, BusinessId = businessId, Priority = 1 });
        }

        // POST: ProviderEvidenceDoc/Create/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(ProviderEvidenceDocViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<ProviderEvidenceDoc>(collection);
                    _providerEvidenceDocService.Insert(model);
                    return RedirectToAction("Index", new { providerId = collection.ProviderId, businessId = collection.BusinessId });
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            ViewBag.ProviderId = collection.ProviderId;
            return View(collection);
        }

        // GET: ProviderEvidenceDoc/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<ProviderEvidenceDocViewModel>(_providerEvidenceDocService.GetById(id));
            return View(model);
        }

        // POST: ProviderEvidenceDoc/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, ProviderEvidenceDocViewModel collection, string selectedTags)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<ProviderEvidenceDoc>(collection);
                        _providerEvidenceDocService.Update(model);
                        return RedirectToAction("Index", new { providerId = collection.ProviderId, businessId = collection.BusinessId });
                    }
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
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