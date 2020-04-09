using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asset.Infrastructure._App;
using AutoMapper;
using Domain.Model.Entities;
using Domain.Application;
using Newtonsoft.Json;
using PagedList;
using Presentation.Panel.Models;
using Presentation.Panel.FilterAttributes;
using Asset.Infrastructure.Common;
using Presentation.Panel.Helpers;

namespace Presentation.Panel.Controllers
{
    public class RoleController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IRoleService _roleService;
        private readonly IModuleService _ModuleService;
        private readonly IDropDownService _dropDownService;

        public RoleController(IRoleService roleService, IModuleService ModuleService, IMapper mapper, ILog4Net logger, IDropDownService dropDownService)
        {
            _roleService = roleService;
            _ModuleService = ModuleService;
            _mapper = mapper;
            _logger = logger;
            _dropDownService = dropDownService;
        }
        #endregion

        // GET: Role
        [HttpGet]
        public ActionResult Index(RoleViewModel model)
        {
            try
            {
                ViewBag.SearchModel = model;
                var request = new FilteredModel<Role>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<RoleViewModel>>(_roleService.GetPaging(_mapper.Map<Role>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<RoleViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
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

        // POST: Role
        [HttpPost]
        public ActionResult List(RoleViewModel collection)
        {
            try
            {
                var request = new FilteredModel<RoleViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<RoleViewModel>>(_roleService.GetPaging(_mapper.Map<Role>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<IList<RoleViewModel>>(_roleService.GetPaging(_mapper.Map<Role>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<RoleViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_RoleGrid", ViewBag.OnePageOfEntries)
                : View();


            //var request = new FilteredModel<Role>
            //{
            //    PageIndex = collection.ThisPageIndex,
            //    Order = collection.PageOrder,
            //    OrderBy = collection.PageOrderBy
            //};
            //var offset = (request.PageIndex - 1) * request.PageSize;
            //var result = _mapper.Map<IList<RoleViewModel>>(_roleService.GetPaging(_mapper.Map<Role>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            //if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            //{
            //    request.PageIndex = (int)(totalCount / request.PageSize);
            //    if (totalCount % request.PageSize > 0)
            //    {
            //        request.PageIndex++;
            //    }
            //    result = _mapper.Map<IList<RoleViewModel>>(_roleService.GetPaging(_mapper.Map<Role>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            //}
            //ViewBag.OnePageOfEntries = new StaticPagedList<RoleViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            //ViewBag.SearchModel = collection;

            //return Request.IsAjaxRequest()
            //            ? (ActionResult)PartialView("_CategoryGrid", ViewBag.OnePageOfEntries)
            //            : View();
            //return View();
        }

        // GET: Role/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<RoleViewModel>(_roleService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Role/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, RoleViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _roleService.Delete(_mapper.Map<Role>(collection));
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

        // GET: Role/Create
        public ActionResult Create()
        {
            return View(new RoleViewModel());
        }

        // POST: Role/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(RoleViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Role>(collection);
                    _roleService.InsertWithLog(model, AdminHelper.AdminId);
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

        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var role = _mapper.Map<RoleViewModel>(_roleService.GetById(id));
                return View(role);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Role/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, RoleViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Role>(collection);
                        _roleService.UpdateWithLog(model, AdminHelper.AdminId);
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
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }

        public ActionResult Permission(int id)
        {
            var model = _mapper.Map<RoleViewModel>(_roleService.GetByIdIncludePermissions(id));

            var lstModules = _mapper.Map<IList<ModuleViewModel>>(_ModuleService.Find(new Module()));
            ViewBag.AllModules = lstModules;
            return View(model);
        }

        [HttpPost]
        public ActionResult Permission(int id, RoleViewModel collection, string SelectedRole2Modules)
        {
            try
            {
                if (id > 0)
                {
                    collection.Role2Modules = JsonConvert.DeserializeObject<List<Role2Module>>(SelectedRole2Modules);
                    _roleService.SavePermissions(collection.Id.GetValueOrDefault(0), collection.Role2Modules);
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
    }
}