using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using PagedList;
using Presentation.Panel.FilterAttributes;
using Presentation.Panel.Helpers;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class UserController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IUserService _userService;
        private readonly IUserLeaderBoardService _userLeaderBoardService;

        public UserController(IUserService userService, IUserLeaderBoardService userLeaderBoardService, IMapper mapper, ILog4Net logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _userLeaderBoardService = userLeaderBoardService;
        }

        #endregion


        // GET: User
        [HttpGet]
        public ActionResult Index(UserViewModel model)
        {
            try
            {
                ViewBag.SearchModel = model;
                var request = new FilteredModel<User>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<UserViewModel>>(_userService.GetPaging(_mapper.Map<User>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<UserViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
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

        // POST: User
        [HttpPost]
        public ActionResult List(UserViewModel collection)
        {
            try
            {
                var request = new FilteredModel<UserViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<UserViewModel>>(_userService.GetPaging(_mapper.Map<User>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<IList<UserViewModel>>(_userService.GetPaging(_mapper.Map<User>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<UserViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_UserGrid", ViewBag.OnePageOfEntries)
                : View();
        }


        // GET: User/LeaderBoards
        [HttpGet]
        public ActionResult LeaderBoards(UserLeaderBoardViewModel model)
        {
            try
            {
                ViewBag.PostAction = "LeaderBoardsList";
                ViewBag.SearchModel = model;
                var request = new FilteredModel<User>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<UserLeaderBoardViewModel>>(_userLeaderBoardService.GetTotalPoints(_mapper.Map<UserLeaderBoard>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<UserLeaderBoardViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
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


        [HttpPost]
        public ActionResult LeaderBoardsList(UserLeaderBoardViewModel collection)
        {
            try
            {
                var request = new FilteredModel<UserLeaderBoardViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<List<UserLeaderBoardViewModel>>(_userLeaderBoardService.GetTotalPoints(_mapper.Map<UserLeaderBoard>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));

                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<List<UserLeaderBoardViewModel>>(_userLeaderBoardService.GetTotalPoints(_mapper.Map<UserLeaderBoard>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<UserLeaderBoardViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return (ActionResult)PartialView("_UserLeaderBoards", ViewBag.OnePageOfEntries);
        }

        // GET: User/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var User = _mapper.Map<UserViewModel>(_userService.GetById(id));
            return View(User);
        }

        // POST: User/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, UserViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var model = _userService.GetById(id);
                    if (model != null)
                    {
                        //model.UpdaterId = LogedInUser.Id;
                        _userService.Delete(model);
                        if (!string.IsNullOrEmpty(collection.PreviousUrl))
                        {
                            return Redirect(collection.PreviousUrl);
                        }
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


        // GET: Club/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var model = _mapper.Map<UserViewModel>(_userService.GetById(id));
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Club/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, UserViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var oldModel = _mapper.Map<UserViewModel>(_userService.GetById(id));
                    oldModel.Status = collection.Status;

                    var model = _mapper.Map<User>(oldModel);
                    _userService.UpdateWithLog(model, AdminHelper.AdminId);
                    if (!string.IsNullOrEmpty(collection.PreviousUrl))
                    {
                        return Redirect(collection.PreviousUrl);
                    }
                    return RedirectToAction("Index");
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