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
using Presentation.Panel.Helpers;

namespace Presentation.Panel.Controllers
{
    public class ModuleController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IModuleService _ModuleService;

        public ModuleController(IModuleService ModuleService, IMapper mapper, ILog4Net logger)
        {
            _ModuleService = ModuleService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Module
        [HttpGet]
        public ActionResult Index(ModuleViewModel model)
        {
            try
            {
                ViewBag.SearchModel = model;
                var request = new FilteredModel<Module>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<ModuleViewModel>>(_ModuleService.GetPaging(_mapper.Map<Module>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ModuleViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Module
        [HttpPost]
        public ActionResult List(ModuleViewModel collection)
        {
            try
            {
                var request = new FilteredModel<ModuleViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ModuleViewModel>>(_ModuleService.GetPaging(_mapper.Map<Module>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<IList<ModuleViewModel>>(_ModuleService.GetPaging(_mapper.Map<Module>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<ModuleViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_ModuleGrid", ViewBag.OnePageOfEntries)
                : View();
        }

        // GET: Module/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<ModuleViewModel>(_ModuleService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Module/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, ModuleViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    _ModuleService.Delete(_mapper.Map<Module>(collection));
                    if (!string.IsNullOrEmpty(collection.PreviousUrl))
                    {
                        return Redirect(collection.PreviousUrl);
                    }
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

        // GET: Module/Create
        public ActionResult Create()
        {
            ViewBag.ParentsList = new SelectList(_ModuleService.GetParents(), "Id", "Title", null);
            return View(new ModuleViewModel());
        }

        // POST: Module/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(ModuleViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Module>(collection);

                    _ModuleService.InsertWithLog(model,AdminHelper.AdminId);
                    if (!string.IsNullOrEmpty(collection.PreviousUrl))
                    {
                        return Redirect(collection.PreviousUrl);
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            ViewBag.ParentsList = new SelectList(_ModuleService.GetParents(), "Id", "Title", collection.ParentId);
            return View(collection);
        }

        // GET: Module/Edit/5
        public ActionResult Edit(int id)
        {
            var parents = _ModuleService.GetParents().Where(m => m.Id != id);
            ViewBag.ParentsList = new SelectList(parents, "Id", "Title", null);
            return View(_mapper.Map<ModuleViewModel>(_ModuleService.GetById(id)));
        }

        // POST: Module/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, ModuleViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Module>(collection);
                        _ModuleService.UpdateWithLog(model,AdminHelper.AdminId);
                        if (!string.IsNullOrEmpty(collection.PreviousUrl))
                        {
                            return Redirect(collection.PreviousUrl);
                        }
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
                }
                var modules = _ModuleService.GetParents().Where(m => m.Id != id);
                ViewBag.ParentsList = new SelectList(modules, "Id", "Title", collection.ParentId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            var parents = _ModuleService.GetParents().Where(m => m.Id != id);
            ViewBag.ParentsList = new SelectList(parents, "Id", "Title", null);
            return View(collection);
        }
    }
}