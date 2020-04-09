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
    public class TagController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly ITagService _tagService;
        private readonly IDropDownService _dropDownService;

        public TagController(ITagService tagService, IMapper mapper, ILog4Net logger, IDropDownService dropDownService)
        {
            _dropDownService = dropDownService;
            _tagService = tagService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Tag
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new TagViewModel();
                long totalCount;
                var request = new FilteredModel<Tag>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<TagViewModel>>(_tagService.GetPaging(new Tag(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<TagViewModel>(result, request.PageIndex, request.PageSize, (int) totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Tag
        [HttpPost]
        public ActionResult Index(TagViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<Tag>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
                var offset = (request.PageIndex - 1) * request.PageSize;
            var result =
                _mapper.Map<IList<TagViewModel>>(_tagService.GetPaging(_mapper.Map<Tag>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int) (totalCount/request.PageSize);
                if (totalCount%request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result =
                    _mapper.Map<IList<TagViewModel>>(_tagService.GetPaging(_mapper.Map<Tag>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<TagViewModel>(result, request.PageIndex, request.PageSize, (int) totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Tag/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<TagViewModel>(_tagService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // GET: Tag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(TagViewModel collection, string selectedTags)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Tag>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    _tagService.Insert(model);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                return View(collection);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.Duplicated);
                }
                else
                {
                    if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                }
                return View(collection);
            }
        }

        // GET: Tag/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<TagViewModel>(_tagService.GetById(id));
            return View(model);
        }

        // POST: Tag/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, TagViewModel collection, string selectedTags)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Tag>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _tagService.Update(model);
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

        // GET: Tag/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_mapper.Map<TagViewModel>(_tagService.GetById(id)));
        }

        // POST: Tag/Delete/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Delete(int id, TagViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _tagService.Delete(_mapper.Map<Tag>(collection));
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