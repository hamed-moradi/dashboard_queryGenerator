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
    public class EventController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IEventService _eventService;
        private readonly IClubService _clubService;
        private readonly IMatchService _matchService;
        private readonly IDropDownService _dropDownService;

        public EventController(IMapper mapper, ILog4Net logger, IEventService eventService, IClubService clubService, IMatchService matchService, IDropDownService dropDownService)
        {
            _mapper = mapper;
            _logger = logger;
            _eventService = eventService;
            _matchService = matchService;
            _clubService = clubService;
            _dropDownService = dropDownService;
        }
        #endregion

        // GET: Event
        [HttpGet]
        public ActionResult Index(EventViewModel model)
        { 
            try
            {
                ViewBag.SearchModel = model;
                var request = new FilteredModel<Event>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<EventViewModel>>(_eventService.GetPaging(_mapper.Map<Event>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<EventViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
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

        // POST: Event
        [HttpPost]
        public ActionResult List(EventViewModel collection)
        {
            try
            {
                var request = new FilteredModel<EventViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<EventViewModel>>(_eventService.GetPaging(_mapper.Map<Event>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<IList<EventViewModel>>(_eventService.GetPaging(_mapper.Map<Event>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<EventViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_EventGrid", ViewBag.OnePageOfEntries)
                : View();
        }

        // GET: Event/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var Event = _mapper.Map<EventViewModel>(_eventService.GetById(id));
            return View(Event);
        }

        // POST: Event/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, EventViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var model = _eventService.GetById(id);
                    if (model != null)
                    {
                        //model.UpdaterId = LogedInEvent.Id;
                        _eventService.Delete(model);

                        var lstMatchsId = _matchService.Find(new Match() { EventId = collection.Id }).Select(x => (long)x.Id.Value).ToArray();
                        if (lstMatchsId.Any())
                        {
                            _matchService.Delete(lstMatchsId);
                        }

                        var lstClubsId = _clubService.Find(new Club() { EventId = collection.Id }).Select(x => (long)x.Id.Value).ToArray();
                        if (lstClubsId.Any())
                        {
                            _clubService.Delete(lstClubsId);
                        }
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


        // GET: Event/Create
        public ActionResult Create()
        {
            var model = new EventViewModel();
            return View(model);
        }

        // POST: Event/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(EventViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Event>(collection);
                    _eventService.InsertWithLog(model, AdminHelper.AdminId);
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
                    if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                }
            }
            return View(collection);
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var model = _mapper.Map<EventViewModel>(_eventService.GetById(id));
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Event/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, EventViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Event>(collection);
                        _eventService.UpdateWithLog(model, AdminHelper.AdminId);
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


        [HttpGet]
        [AllowAnonymous]
        public JsonResult DropDown(string key, int? p)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length < 3) return null;
            var result = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetEvents(key, (p.GetValueOrDefault(1) - 1) * 30, 30, out long totalCount));
            return Json(new DropDownModel { TotalCount = totalCount, Items = result }, JsonRequestBehavior.AllowGet);
        }
    }
}