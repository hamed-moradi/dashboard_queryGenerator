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
    public class CommentController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService, IMapper mapper, ILog4Net logger)
        {
            _commentService = commentService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Comment
        [HttpGet]
        public ActionResult Index(int? commentEntityId, byte? commentEntityTypeId)
        {
            ViewBag.commentEntityId = commentEntityId;
            try
            {
                ViewBag.SearchModel = new CommentViewModel { CommentEntityTypeId=(GeneralEnums.CommentEntityType?)commentEntityTypeId, CommentEntityId=commentEntityId };
                var request = new FilteredModel<Comment>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(new Comment { CommentEntityId = commentEntityId , CommentEntityTypeId = commentEntityTypeId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<CommentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Comment
        [HttpPost]
        public ActionResult Index(CommentViewModel collection)
        {
            var request = new FilteredModel<Comment>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(_mapper.Map<Comment>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(_mapper.Map<Comment>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<CommentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Comment/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            CommentViewModel comment;
            try
            {
                comment = _mapper.Map<CommentViewModel>(_commentService.GetById(id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }

            return View(comment);
        }

        // POST: Comment/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, CommentViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _commentService.Delete(_mapper.Map<Comment>(collection));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }

        // GET: Comment/Create/5
        public ActionResult Create(int businessId, int replyTo)
        {
            ViewBag.BusinessId = businessId;
            ViewBag.ReplyTo = replyTo;
            return View();
        }

        // POST: Comment/Create/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(CommentViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Comment>(collection);
                    model.IP = LogedInAdmin.IP;
                    model.CreatorId = LogedInAdmin.Id;
                    _commentService.Insert(model);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            ViewBag.ReplyTo = collection.ReplyTo;
            return View(collection);
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<CommentViewModel>(_commentService.GetById(id));
            return View(model);
        }

        // POST: Comment/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, CommentViewModel collection, string selectedTags)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Comment>(collection);
                        _commentService.UpdateCommentStatus(model);
                        return RedirectToAction("Index");
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