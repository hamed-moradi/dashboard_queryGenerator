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
    public class MatchGroupController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IMatchGroupService _MatchGroupService;
        private readonly IDropDownService _dropDownService;
        private readonly IPredictionService _predictionService;

        public MatchGroupController(IMapper mapper, IPredictionService predictionService, ILog4Net logger, IMatchGroupService MatchGroupService, IDropDownService dropDownService)
        {
            _mapper = mapper;
            _logger = logger;
            _MatchGroupService = MatchGroupService;
            _dropDownService = dropDownService;
            _predictionService = predictionService;
        }
        #endregion

        // GET: MatchGroup
        [HttpGet]
        public ActionResult Index(MatchGroupViewModel model)
        {
            try
            {
                ViewBag.SearchModel = model;
                var request = new FilteredModel<MatchGroup>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<MatchGroupViewModel>>(_MatchGroupService.GetPaging(_mapper.Map<MatchGroup>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<MatchGroupViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
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

        // POST: MatchGroup
        [HttpPost]
        public ActionResult List(MatchGroupViewModel collection)
        {
            try
            {
                var request = new FilteredModel<MatchGroupViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<MatchGroupViewModel>>(_MatchGroupService.GetPaging(_mapper.Map<MatchGroup>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<IList<MatchGroupViewModel>>(_MatchGroupService.GetPaging(_mapper.Map<MatchGroup>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<MatchGroupViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_MatchGroupGrid", ViewBag.OnePageOfEntries)
                : View();
        }

        // GET: MatchGroup/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var MatchGroup = _mapper.Map<MatchGroupViewModel>(_MatchGroupService.GetById(id));
            return View(MatchGroup);
        }

        // POST: MatchGroup/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, MatchGroupViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var model = _MatchGroupService.GetById(id);
                    if (model != null)
                    {
                        //model.UpdaterId = LogedInMatchGroup.Id;
                        _MatchGroupService.Delete(model);
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


        // GET: MatchGroup/Create
        public ActionResult Create()
        {
            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", null);
            return View(new MatchGroupViewModel());
        }

        // POST: MatchGroup/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(MatchGroupViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<MatchGroup>(collection);
                    _MatchGroupService.InsertWithLog(model, AdminHelper.AdminId);

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

        // GET: MatchGroup/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var model = _mapper.Map<MatchGroupViewModel>(_MatchGroupService.GetById(id));
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

        // POST: MatchGroup/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, MatchGroupViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<MatchGroup>(collection);
                        if (!ModelState.IsValid)
                        {
                            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", model.EventId);
                            return View(collection);
                        }

                        _MatchGroupService.UpdateWithLog(model, AdminHelper.AdminId);
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
        public JsonResult GetMatchGroupeshByEventId(int eventId, string key = "")
        {
            var response = new JsonViewModel { status = HttpStatusCode.InternalServerError, message = GeneralMessages.Error };
            try
            {
                response.data = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetMatchGroupes(eventId, key));
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