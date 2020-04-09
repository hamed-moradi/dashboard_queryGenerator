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
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Controllers
{
    public class User2NotificationController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IUser2NotificationService _user2NotificationService;

        public User2NotificationController(IUser2NotificationService user2NotificationService, IMapper mapper, ILog4Net logger)
        {
            _user2NotificationService = user2NotificationService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: UserNotification
        [HttpGet]
        public ActionResult Index(int id)
        {
            try
            {
                ViewBag.SearchModel = new User2NotificationViewModel();
                long totalCount;
                var request = new FilteredModel<User2Notification>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<User2NotificationViewModel>>(_user2NotificationService.GetPaging(new User2Notification { NotificationId = id }, out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<User2NotificationViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: 
        [HttpPost]
        public ActionResult Index(int id, User2NotificationViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<User2Notification>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            collection.Id = null;
            collection.NotificationId = id;
                var offset = (request.PageIndex - 1) * request.PageSize;
            var result =
                _mapper.Map<IList<User2NotificationViewModel>>(_user2NotificationService.GetPaging(_mapper.Map<User2Notification>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result =
                    _mapper.Map<IList<User2NotificationViewModel>>(_user2NotificationService.GetPaging(_mapper.Map<User2Notification>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<User2NotificationViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: UserNotification/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<User2NotificationViewModel>(_user2NotificationService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // GET: UserNotification/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_mapper.Map<User2NotificationViewModel>(_user2NotificationService.GetById(id)));
        }

        // POST: UserNotification/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, User2NotificationViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    _user2NotificationService.Delete(_mapper.Map<User2Notification>(collection));
                    return RedirectToAction("Index", new { id = collection.UserId });
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