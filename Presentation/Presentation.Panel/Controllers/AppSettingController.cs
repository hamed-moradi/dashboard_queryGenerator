using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using PagedList;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class AppSettingController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IAppSettingService _appSettingService;
        private readonly IAppSetting2AppSettingGroupService _appSetting2AppSettingGroupService;
        private readonly IAppSettingGroupService _appSettingGroupService;

        public AppSettingController(IAppSettingService appSettingService, IAppSetting2AppSettingGroupService appSetting2AppSettingGroupService, IAppSettingGroupService appSettingGroupService, IMapper mapper, ILog4Net logger)
        {
            _appSettingService = appSettingService;
            _appSetting2AppSettingGroupService = appSetting2AppSettingGroupService;
            _appSettingGroupService = appSettingGroupService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: AppSetting
        [HttpGet]
        public ActionResult Index()
        {
            List<AppSettingViewModel> model = new List<AppSettingViewModel>();
            
            try
            {
                ViewBag.SearchModel = new AppSettingViewModel { };
                ViewBag.Setting2Group = _mapper.Map<List<AppSetting2AppSettingGroupViewModel>>(_appSetting2AppSettingGroupService.Find(new AppSetting2AppSettingGroup()));
                ViewBag.Groups = _mapper.Map<List<AppSettingGroupViewModel>>(_appSettingGroupService.Find(new AppSettingGroup()));
                model = _mapper.Map<List<AppSettingViewModel>>(_appSettingService.Find(new AppSetting()));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(model);
        }

        // POST: AppSetting
        [HttpPost]
        public ActionResult Index(AppSettingViewModel collection)
        {
            ViewBag.SearchModel = new AppSettingViewModel { };
            var model = _mapper.Map<List<AppSettingViewModel>>(_appSettingService.Find(_mapper.Map<AppSetting>(collection)));
            ViewBag.Setting2Group = _mapper.Map<List<AppSetting2AppSettingGroupViewModel>>(_appSetting2AppSettingGroupService.Find(new AppSetting2AppSettingGroup()));
            ViewBag.Groups = _mapper.Map<List<AppSettingGroupViewModel>>(_appSettingGroupService.Find(new AppSettingGroup()));

            return View(model);
        }

        // GET: AppSetting/Details/5
        public ActionResult Details(string key)
        {
            AppSettingViewModel appSettingViewModel;
            try
            {
                appSettingViewModel = _mapper.Map<AppSettingViewModel>(_appSettingService.Single(new AppSetting { Key = key }));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }

            return View(appSettingViewModel);
        }
        
        // GET: AppSetting/Edit/5
        public ActionResult Edit(string key)
        {
            var model = _mapper.Map<AppSettingViewModel>(_appSettingService.Single(new AppSetting { Key = key }));
            return View(model);
        }

        // POST: AppSetting/Edit/5
        [HttpPost]
        public ActionResult Edit(AppSettingViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<AppSetting>(collection);
                    _appSettingService.UpdateByKey(model);
                    return RedirectToAction("Index");
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

        // GET: AppSetting/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppSetting/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
