using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using PagedList;
using Presentation.Panel.FilterAttributes;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class AppVersionController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IAppVersionService _appVersionService;
        private readonly IRegisteredClientService _registeredClientService;

        public AppVersionController(IAppVersionService appVersionService, IRegisteredClientService registeredClientService, IMapper mapper, ILog4Net logger)
        {
            _appVersionService = appVersionService;
            _registeredClientService = registeredClientService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: AppVersion
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new AppVersionViewModel { };
                var request = new FilteredModel<AppVersion>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<AppVersionViewModel>>(_appVersionService.GetPaging(new AppVersion { }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<AppVersionViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: AppVersion
        [HttpPost]
        public ActionResult Index(AppVersionViewModel collection)
        {
            var request = new FilteredModel<AppVersion>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<AppVersionViewModel>>(_appVersionService.GetPaging(_mapper.Map<AppVersion>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<AppVersionViewModel>>(_appVersionService.GetPaging(_mapper.Map<AppVersion>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<AppVersionViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: AppVersion/Details/5
        public ActionResult Details(int id)
        {
            AppVersionViewModel appVersionViewModel;
            try
            {
                appVersionViewModel = _mapper.Map<AppVersionViewModel>(_appVersionService.GetById(id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }

            return View(appVersionViewModel);
        }

        // GET: AppVersion/Create
        public ActionResult Create()
        {
            var client = _mapper.Map<List<RegisteredClientViewModel>>(_registeredClientService.Find(new RegisteredClient()));
            ViewData["client"] = new SelectList(client, "Id", "Name");
            return View();
        }

        // POST: AppVersion/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(AppVersionViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<AppVersion>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    model.CreatedAt = DateTime.Now;
                    var id = _appVersionService.Insert(model);

                    if (collection.LastVersionCheck)
                    {
                        var registeredClient = _registeredClientService.Single(new RegisteredClient { Id = collection.ClientId });
                        registeredClient.LastStableVersion = id;
                        _registeredClientService.Update(registeredClient);
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

            var client = _mapper.Map<List<RegisteredClientViewModel>>(_registeredClientService.Find(new RegisteredClient()));
            ViewData["client"] = new SelectList(client, "Id", "Name");

            return View(collection);
        }

        // GET: AppVersion/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<AppVersionViewModel>(_appVersionService.GetById(id));
            if(model.LastStableVersionId == model.Id)
            {
                model.LastVersionCheck = true;
            }
            return View(model);
        }

        // POST: AppVersion/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(AppVersionViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<AppVersion>(collection);
                    model.UpdaterId = LogedInAdmin.Id;
                    model.LastUpdatedAt = DateTime.Now;
                    _appVersionService.Update(model);

                    if (collection.LastVersionCheck)
                    {
                        var registeredClient = _registeredClientService.Single(new RegisteredClient { Id = model.ClientId });
                        registeredClient.LastStableVersion = model.Id;
                        _registeredClientService.Update(registeredClient);
                    }

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

        // GET: AppVersion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppVersion/Delete/5
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

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LastVersionCheck(int id, int major,int minor, int patch, int clientId)
        {
            var response = new JsonViewModel { status = HttpStatusCode.InternalServerError, message = GeneralMessages.Error };
            try
            {
                //response.data = 
                var appVersion = _appVersionService.Find(new AppVersion { ClientId = clientId }).Where(w => w.Id != id);
                
                foreach(var item in appVersion)
                {
                    if (item.Major > major)
                    { response.data = true; }
                    else if (item.Major == major && item.Minor > minor)
                    { response.data = true; }
                    else if (item.Minor == minor && item.Patch > patch)
                    { response.data = true; }
                    else
                    { response.data = false; }
                    
                }

                response.status = HttpStatusCode.OK;
                response.message = GeneralMessages.Ok;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
