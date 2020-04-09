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
using System.IO;
using Presentation.Panel.FilterAttributes;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Controllers
{
    public class BusinessAttchmentController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IBusinessAttachmentService _businessAttachmentService;
        private readonly IBusinessService _businessService;

        public BusinessAttchmentController(IBusinessAttachmentService businessAttachmentervice, IMapper mapper, ILog4Net logger, IBusinessService businessService)
        {
            _businessAttachmentService = businessAttachmentervice;
            _mapper = mapper;
            _logger = logger;
            _businessService = businessService;
        }

        #endregion Constructor

        // GET: ContentAttachment
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
                ViewBag.SearchModel = new BusinessAttachmentViewModels();
                long totalCount;
                var request = new FilteredModel<BusinessAttachment>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result =
                    _mapper.Map<IList<BusinessAttachmentViewModels>>(_businessAttachmentService.GetPaging(new BusinessAttachment() { BusinessId = businessId.Value },
                        out totalCount,
                        request.OrderBy,
                        request.Order,
                        offset,
                        request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<BusinessAttachmentViewModels>(result, request.PageIndex, request.PageSize, (int)totalCount);
                HttpContext.Cache["BusinessId"] = businessId;
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: ContentAttachment
        [HttpPost]
        public ActionResult Index(BusinessAttachmentViewModels collection)
        {
            long totalCount;
            var request = new FilteredModel<BusinessAttachment>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result =
                _mapper.Map<IList<BusinessAttachmentViewModels>>(_businessAttachmentService.GetPaging(_mapper.Map<BusinessAttachment>(collection),
                    out totalCount,
                    request.OrderBy,
                    request.Order,
                    offset,
                    request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result =
                    _mapper.Map<IList<BusinessAttachmentViewModels>>(_businessAttachmentService.GetPaging(_mapper.Map<BusinessAttachment>(collection),
                        out totalCount,
                        request.OrderBy,
                        request.Order,
                        offset,
                        request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<BusinessAttachmentViewModels>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: ContentAttachment/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<BusinessAttachmentViewModels>(_businessAttachmentService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: ContentAttachment/Details/5
        [HttpPost]
        public ActionResult Details(int id, BusinessAttachmentViewModels collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _businessAttachmentService.Delete(_mapper.Map<BusinessAttachment>(collection));
                    return RedirectToAction("Index", new { businessId = collection.BusinessId });
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

        //GET: ContentAttachment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContentAttachment/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(BusinessAttachmentViewModels collection)
        {
            try
            {
                collection.BusinessId = Convert.ToInt32(HttpContext.Cache["BusinessId"]);
                var info = new FileInfo(Server.MapPath(collection.Path));
                collection.Size = Convert.ToInt32(info.Length);
                collection.CreatedAt = "";

                if (ModelState.IsValid)
                {
                    collection.CreatorId = LogedInAdmin.Id;

                    var model = _mapper.Map<BusinessAttachment>(collection);
                    model.CreatedAt = DateTime.Now;
                    model.Size = collection.Size;
                    _businessAttachmentService.Insert(model);
                    return RedirectToAction("Index", new { businessId = Convert.ToInt32(HttpContext.Cache["BusinessId"]) });
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

        // GET: ContentAttachment/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<BusinessAttachmentViewModels>(_businessAttachmentService.GetById(id));
            return View(model);
        }

        // POST: ContentAttachment/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, BusinessAttachmentViewModels collection)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        var info = new FileInfo(Server.MapPath(collection.Path));
                        collection.Size = Convert.ToInt32(info.Length);
                    }
                    catch
                    {
                        collection.Size = 0;
                    }
                    
                    if (ModelState.IsValid)
                    {
                        collection.UpdaterId = LogedInAdmin.Id;
                        var model = _mapper.Map<BusinessAttachment>(collection);
                        model.UpdatedAt = DateTime.Now;
                        _businessAttachmentService.Update(model);
                        return RedirectToAction("Index", new { businessId = collection.BusinessId });
                    }
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
                }
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