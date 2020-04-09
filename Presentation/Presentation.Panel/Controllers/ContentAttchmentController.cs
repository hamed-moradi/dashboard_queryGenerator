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
    public class ContentAttachmentController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IContentAttachmentService _contentAttachmentService;

        public ContentAttachmentController(IContentAttachmentService contentAttachmentervice, IMapper mapper, ILog4Net logger)
        {
            _contentAttachmentService = contentAttachmentervice;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion Constructor

        // GET: ContentAttachment
        [HttpGet]
        public ActionResult Index(int? contentId)
        {
            try
            {
                ViewBag.SearchModel = new ContentAttachmentViewModel();
                long totalCount;
                var request = new FilteredModel<ContentAttachment>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result =
                    _mapper.Map<IList<ContentAttachmentViewModel>>(_contentAttachmentService.GetPaging(new ContentAttachment() { ContentId = contentId.Value },
                        out totalCount,
                        request.OrderBy,
                        request.Order,
                        offset,
                        request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ContentAttachmentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                HttpContext.Cache["ContentId"] = contentId;
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
        public ActionResult Index(ContentAttachmentViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<ContentAttachment>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result =
                _mapper.Map<IList<ContentAttachmentViewModel>>(_contentAttachmentService.GetPaging(_mapper.Map<ContentAttachment>(collection),
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
                    _mapper.Map<IList<ContentAttachmentViewModel>>(_contentAttachmentService.GetPaging(_mapper.Map<ContentAttachment>(collection),
                        out totalCount,
                        request.OrderBy,
                        request.Order,
                        offset,
                        request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ContentAttachmentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: ContentAttachment/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<ContentAttachmentViewModel>(_contentAttachmentService.GetById(id)));
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
        public ActionResult Details(int id, ContentAttachmentViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _contentAttachmentService.Delete(_mapper.Map<ContentAttachment>(collection));
                    return RedirectToAction("Index", new { contentId = collection.ContentId });
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
        public ActionResult Create(ContentAttachmentViewModel collection)
        {
            try
            {
                collection.ContentId = Convert.ToInt32(HttpContext.Cache["ContentId"]);
                var info = new FileInfo(Server.MapPath(collection.Path));
                collection.FileSize = Convert.ToInt32(info.Length);
                collection.CreatedAt = "";

                if (ModelState.IsValid)
                {
                    collection.CreatorId = LogedInAdmin.Id;

                    var model = _mapper.Map<ContentAttachment>(collection);
                    model.CreatedAt = DateTime.Now;
                    model.Size = collection.FileSize;
                    _contentAttachmentService.Insert(model);
                    return RedirectToAction("Index", new { contentId = Convert.ToInt32(HttpContext.Cache["ContentId"]) });
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
            var model = _mapper.Map<ContentAttachmentViewModel>(_contentAttachmentService.GetById(id));
            return View(model);
        }

        // POST: ContentAttachment/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, ContentAttachmentViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var info = new FileInfo(Server.MapPath(collection.Path));
                    collection.FileSize = Convert.ToInt32(info.Length);
                    if (ModelState.IsValid)
                    {
                        collection.UpdaterId = LogedInAdmin.Id;
                        var model = _mapper.Map<ContentAttachment>(collection);
                        model.UpdatedAt = DateTime.Now;
                        _contentAttachmentService.Update(model);
                        return RedirectToAction("Index", new { contentId = collection.ContentId });
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