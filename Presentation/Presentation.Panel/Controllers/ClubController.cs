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
using System.Net;

namespace Presentation.Panel.Controllers
{
    public class ClubController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IClubService _clubService;
        private readonly IDropDownService _dropDownService;


        public ClubController(IMapper mapper, ILog4Net logger, IClubService clubService, IDropDownService dropDownService)
        {
            _mapper = mapper;
            _logger = logger;
            _clubService = clubService;
            _dropDownService = dropDownService;
        }
        #endregion

        // GET: Club
        [HttpGet]
        public ActionResult Index(ClubViewModel model)
        {
            try
            {
                ViewBag.SearchModel = model;
                var request = new FilteredModel<Club>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<ClubViewModel>>(_clubService.GetPaging(_mapper.Map<Club>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ClubViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
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

        // POST: Club
        [HttpPost]
        public ActionResult List(ClubViewModel collection)
        {
            try
            {
                var request = new FilteredModel<ClubViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ClubViewModel>>(_clubService.GetPaging(_mapper.Map<Club>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<IList<ClubViewModel>>(_clubService.GetPaging(_mapper.Map<Club>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<ClubViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_ClubGrid", ViewBag.OnePageOfEntries)
                : View();

            //var request = new FilteredModel<Club>
            //{
            //    PageIndex = collection.ThisPageIndex,
            //    Order = collection.PageOrder,
            //    OrderBy = collection.PageOrderBy
            //};
            //var offset = (request.PageIndex - 1) * request.PageSize;
            //var result = _mapper.Map<IList<ClubViewModel>>(_clubService.GetPaging(_mapper.Map<Club>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            //if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            //{
            //    request.PageIndex = (int)(totalCount / request.PageSize);
            //    if (totalCount % request.PageSize > 0)
            //    {
            //        request.PageIndex++;
            //    }
            //    result = _mapper.Map<IList<ClubViewModel>>(_clubService.GetPaging(_mapper.Map<Club>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            //}
            //ViewBag.OnePageOfEntries = new StaticPagedList<ClubViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            //ViewBag.SearchModel = collection;
            //return View();
        }

        // GET: Club/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var Club = _mapper.Map<ClubViewModel>(_clubService.GetById(id));
            return View(Club);
        }

        // POST: Club/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, ClubViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var model = _clubService.GetById(id);
                    if (model != null)
                    {
                        //model.UpdaterId = LogedInClub.Id;
                        _clubService.Delete(model);
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


        // GET: Club/Create
        public ActionResult Create()
        {
            var model = new ClubViewModel();
            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", null);
            return View(model);
        }

        // POST: Club/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(ClubViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Club>(collection);
                    _clubService.InsertWithLog(model, AdminHelper.AdminId);
                    if (!string.IsNullOrEmpty(collection.PreviousUrl))
                    {
                        return Redirect(collection.PreviousUrl);
                    }
                    return RedirectToAction("Index");
                }

                ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection.EventId);
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
                    if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                }
                ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection.EventId);
            }
            return View(collection);
        }

        // GET: Club/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var model = _mapper.Map<ClubViewModel>(_clubService.GetById(id));
                //model.PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", model.EventId);
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
        public ActionResult Edit(int id, ClubViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Club>(collection);
                        _clubService.UpdateWithLog(model, AdminHelper.AdminId);
                        if (!string.IsNullOrEmpty(collection.PreviousUrl))
                        {
                            return Redirect(collection.PreviousUrl);
                        }
                        return RedirectToAction("Index");
                    }
                    ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection.EventId);
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
                }
                ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection.EventId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection.EventId);
            }
            return View(collection);
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetClubsByEventId(int eventId, string key = "")
        {
            var response = new JsonViewModel { status = HttpStatusCode.InternalServerError, message = GeneralMessages.Error };
            try
            {
                response.data = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetClubs(eventId, key));
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