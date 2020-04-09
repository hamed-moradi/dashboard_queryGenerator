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
    public class SettingController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService, IMapper mapper, ILog4Net logger)
        {
            _settingService = settingService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Setting
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new SettingViewModel();
                long totalCount;
                var request = new FilteredModel<Setting>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<SettingViewModel>>(_settingService.GetPaging(new Setting(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<SettingViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Setting
        [HttpPost]
        public ActionResult Index(SettingViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<Setting>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
                var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<SettingViewModel>>(_settingService.GetPaging(_mapper.Map<Setting>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<SettingViewModel>>(_settingService.GetPaging(_mapper.Map<Setting>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<SettingViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Setting/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<SettingViewModel>(_settingService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // GET: Setting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Setting/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(SettingViewModel collection, string selectedTags)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Setting>(collection);
                    _settingService.Insert(model);
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

        // GET: Setting/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<SettingViewModel>(_settingService.GetById(id));
            return View(model);
        }

        // POST: Setting/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, SettingViewModel collection, string selectedTags)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Setting>(collection);
                        _settingService.Update(model);
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

        // GET: Setting/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_mapper.Map<SettingViewModel>(_settingService.GetById(id)));
        }

        // POST: Setting/Delete/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Delete(int id, SettingViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    _settingService.Delete(_mapper.Map<Setting>(collection));
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