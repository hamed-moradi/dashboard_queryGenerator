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
using System.Net;
using Presentation.Panel.FilterAttributes;
using Asset.Infrastructure.Common;
using System.IO;

namespace Presentation.Panel.Controllers
{
    public class ContentsController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IContentService _contentService;
        private readonly IContentAttachmentService _contentAttachmentService;
        private readonly IDropDownService _dropDownService;
        private readonly IContentCategoryService _contentCategoryService;

        public ContentsController(IContentService contentService, IContentAttachmentService contentAttachmentService, IMapper mapper, ILog4Net logger, IDropDownService dropDownService, IContentCategoryService contentCategoryService)
        {
            _dropDownService = dropDownService;
            _contentService = contentService;
            _contentAttachmentService = contentAttachmentService;
            _mapper = mapper;
            _logger = logger;
            _contentCategoryService = contentCategoryService;
        }

        #endregion Constructor

        #region Private

        private bool CheckAttachments(List<ContentAttachmentViewModel> attachments)
        {
            foreach (var item in attachments)
            {
                if (attachments.Except(new List<ContentAttachmentViewModel> { item }).Any(a => a.PartNo == item.PartNo && (a.GroupTitle != item.GroupTitle || a.QualityId == item.QualityId)))
                    return false;
            }
            return true;
        }

        #endregion Private

        // GET: Content
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new ContentViewModel();
                long totalCount;
                var request = new FilteredModel<Content>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ContentViewModel>>(_contentService.GetPaging(new Content(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ContentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Content
        [HttpPost]
        public ActionResult Index(ContentViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<Content>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<ContentViewModel>>(_contentService.GetPaging(_mapper.Map<Content>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result =
                    _mapper.Map<IList<ContentViewModel>>(_contentService.GetPaging(_mapper.Map<Content>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ContentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Content/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            ContentViewModel model;
            try
            {
                long totalCount;
                model = _mapper.Map<ContentViewModel>(_contentService.GetById(id));
                var attachments = _mapper.Map<List<ContentAttachmentViewModel>>(_contentAttachmentService.GetPaging(new ContentAttachment() { ContentId = id }, out totalCount, "Id", "Asc", 0, 100));
                var parts = attachments.OrderBy(x => x.PartNo).ThenBy(x => x.QualityId).GroupBy(x => x.PartNo);
                var partsList = new List<ContentAttachmentViewModel>();
                foreach (var part in parts)
                {
                    var guid = Guid.NewGuid().ToString();
                    ContentAttachmentViewModel partModel = null;
                    foreach (var quality in part)
                    {
                        if (partModel != null)
                        {
                            break;
                        }
                        quality.UniqueId = guid;
                        partModel = quality;
                    }
                    partsList.Add(partModel);
                }

                ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_contentCategoryService.Find(new ContentCategory()));
                ViewBag.partsList = partsList;

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }

            return View(model);

        }

        // GET: Content/Create
        public ActionResult Create()
        {
            ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_contentCategoryService.Find(new ContentCategory()));
            return View();
        }

        // POST: Content/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(ContentViewModel content, List<ContentAttachmentViewModel> ContentAttachments)
        {
            content.CreatorId = LogedInAdmin.Id;
            content.CommentCount = 0;
            content.LikeCount = 0;
            content.ViewCount = 0;
            content.CreatedAt = "";
            try
            {
                if (!ModelState.IsValid)
                    return Json(new JsonViewModel { status = HttpStatusCode.InternalServerError, message = GeneralMessages.DefectiveEntry });

                var model = _mapper.Map<Content>(content);
                model.CreatedAt = DateTime.Now;
                List<ContentAttachment> attachmensList = new List<ContentAttachment>();
                if (ContentAttachments != null)
                    attachmensList = _mapper.Map<List<ContentAttachment>>(ContentAttachments);
                foreach (var item in attachmensList)
                {
                    FileInfo f2 = new FileInfo(Server.MapPath(item.Path));

                    item.Size = Convert.ToInt32(f2.Length);
                    item.Extension = f2.Extension;
                }

                var result = _contentService.Insert(model, attachmensList);
                return Json(new JsonViewModel { status = result > 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, message = result > 0 ? GeneralMessages.Ok : GeneralMessages.Error });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new JsonViewModel { status = HttpStatusCode.InternalServerError, message = ex.Message });
            }
        }

        // GET: Content/Edit/5
        public ActionResult Edit(int id)
        {
            long totalCount;
            var model = _mapper.Map<ContentViewModel>(_contentService.GetById(id));
            var attachments = _mapper.Map<List<ContentAttachmentViewModel>>(_contentAttachmentService.GetPaging(new ContentAttachment() { ContentId = id }, out totalCount, "Id", "Asc", 0, 100));
            var parts = attachments.OrderBy(x => x.PartNo).ThenBy(x => x.QualityId).GroupBy(x => x.PartNo);
            var partsList = new List<ContentAttachmentViewModel>();
            foreach (var part in parts)
            {
                var guid = Guid.NewGuid().ToString();
                ContentAttachmentViewModel partModel = null;
                foreach (var quality in part)
                {
                    if (partModel != null)
                    {
                        break;
                    }
                    quality.UniqueId = guid;
                    partModel = quality;
                }
                partsList.Add(partModel);
            }

            ViewBag.attachments = attachments;
            ViewBag.CategoryTree = _mapper.Map<List<TreeModel>>(_contentCategoryService.Find(new ContentCategory()));
            ViewBag.partsList = partsList;
            return View(model);
        }

        // POST: Content/Edit/5
        [HttpPost]
        [SecurityFilter]
        public JsonResult Edit(ContentViewModel content, List<ContentAttachmentViewModel> ContentAttachments)
        {
            try
            {
                content.UpdaterId = LogedInAdmin.Id;
                content.UpdatedAt = "";

                if (!ModelState.IsValid) return Json(new JsonViewModel { status = HttpStatusCode.InternalServerError, message = GeneralMessages.DefectiveEntry });
                var model = _mapper.Map<Content>(content);
                model.UpdaterId = LogedInAdmin.Id;
                model.UpdatedAt = DateTime.Now;
                var attachmensList = new List<ContentAttachment>();
                var attachments = _contentAttachmentService.GetByContentId(content.Id.Value);
                foreach (var item in attachments)
                {
                    if (!ContentAttachments.Where(w => w.Id != null).Select(s => s.Id).Contains(item.Id))
                    {
                        item.UpdatedAt = DateTime.Now;
                        item.UpdaterId = LogedInAdmin.Id;
                        item.Status = (byte)GeneralEnums.Status.Deleted;
                        attachmensList.Add(item);
                    }
                }
                List<ContentAttachment> attachmens = new List<ContentAttachment>();
                if (ContentAttachments != null)
                {
                    attachmens = _mapper.Map<List<ContentAttachment>>(ContentAttachments);
                }

                foreach (var item in attachmens)
                {
                    if (!attachments.Where(w => w.Id == item.Id & item.Status == 3).Any())
                    {
                        item.CreatorId = LogedInAdmin.Id;
                        item.CreatedAt = DateTime.Now;
                        item.Status = 1;
                        FileInfo f2 = new FileInfo(Server.MapPath(item.Path));
                        item.Size = Convert.ToInt32(f2.Length);
                        item.Extension = f2.Extension;

                        attachmensList.Add(item);
                    }
                    else
                    {
                        attachmensList.Add(attachments.Where(w => w.Id == item.Id & item.Status == 3).First());
                    }
                }

                var result = _contentService.Update(model, attachmensList);//content.Tags, content.Positions,
                return Json(new JsonViewModel { status = result > 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, message = result > 0 ? GeneralMessages.Ok : GeneralMessages.Error });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new JsonViewModel { status = HttpStatusCode.InternalServerError, message = ex.Message });
            }
        }

        // GET: Content/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_mapper.Map<ContentViewModel>(_contentService.GetById(id)));
        }

        // POST: Content/Delete/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Delete(int id, ContentViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _contentService.Delete(_mapper.Map<Content>(collection));
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
    }
}