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

namespace Presentation.Panel.Controllers
{
    public class PageController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IPageService _pageService;
        private readonly IDropDownService _dropDownService;

        public PageController(IPageService pageService, IMapper mapper, ILog4Net logger, IDropDownService dropDownService)
        {
            _pageService = pageService;
            _mapper = mapper;
            _logger = logger;
            _dropDownService = dropDownService;
        }

        #endregion

        // GET: Page
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new PageViewModel();
                var request = new FilteredModel<Page>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<PageViewModel>>(_pageService.GetPaging(new Page(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<PageViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Page
        [HttpPost]
        public ActionResult Index(PageViewModel collection)
        {
            var request = new FilteredModel<Page>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<PageViewModel>>(_pageService.GetPaging(_mapper.Map<Page>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<PageViewModel>>(_pageService.GetPaging(_mapper.Map<Page>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<PageViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Page/Details/1
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<PageViewModel>(_pageService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }

        //POST: Page/Delete/1
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, PageViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (_pageService.Delete(new[] { id }) > 0)
                    {
                        return RedirectToAction("Index");
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

        // GET: Page/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //POST: Page/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(PageViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Page>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    _pageService.Insert(model);
                    return RedirectToAction("Index");
                }
                ViewBag.PositionsList = new SelectList(_dropDownService.GetPositions(), "id", "name", null);
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
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
            }
            return View(collection);
        }

        //GET: Page/Edit/1
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_mapper.Map<PageViewModel>(_pageService.GetById(id)));
            }
            catch (Exception exp)
            {
                _logger.Error(exp);
                ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        //POST: Page/Edit/1
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, PageViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Page>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _pageService.Update(model);
                        return RedirectToAction("Index");
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