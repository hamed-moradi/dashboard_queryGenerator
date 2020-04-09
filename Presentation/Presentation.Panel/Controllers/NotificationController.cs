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
    public class NotificationController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly INotificationService _notificationService;
        private readonly IDropDownService _dropDownService;

        public NotificationController(INotificationService notificationService, IMapper mapper, ILog4Net logger, IDropDownService dropDownService)
        {
            _dropDownService = dropDownService;
            _notificationService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Notification
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new NotificationViewModel();
                long totalCount;
                var request = new FilteredModel<Notification>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<NotificationViewModel>>(_notificationService.GetPaging(new Notification(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<NotificationViewModel>(result, request.PageIndex, request.PageSize, (int) totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Notification
        [HttpPost]
        public ActionResult Index(NotificationViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<Notification>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
                var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<NotificationViewModel>>(_notificationService.GetPaging(_mapper.Map<Notification>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int) (totalCount/request.PageSize);
                if (totalCount%request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<NotificationViewModel>>(_notificationService.GetPaging(_mapper.Map<Notification>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<NotificationViewModel>(result, request.PageIndex, request.PageSize, (int) totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Notification/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<NotificationViewModel>(_notificationService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // GET: Notification/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notification/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(NotificationViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    collection.CreatorId = LogedInAdmin.Id;
                    var model = _mapper.Map<Notification>(collection);
                    _notificationService.Insert(model);
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

        // GET: Notification/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<NotificationViewModel>(_notificationService.GetById(id));
            return View(model);
        }

        // POST: Notification/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, NotificationViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        //collection.UpdaterId = Admin.Id;
                        var model = _mapper.Map<Notification>(collection);
                        _notificationService.Update(model);
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

        // GET: Notification/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_mapper.Map<NotificationViewModel>(_notificationService.GetById(id)));
        }

        // POST: Notification/Delete/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Delete(int id, NotificationViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    //collection.UpdaterId = Admin.Id;
                    _notificationService.Delete(_mapper.Map<Notification>(collection));
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