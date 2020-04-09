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
    public class MatchController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IMatchService _matchService;
        private readonly IDropDownService _dropDownService;
        private readonly IMatch2ClubService _match2ClubService;
        private readonly IPredictionService _predictionService;

        public MatchController(IMapper mapper, IPredictionService predictionService, IMatch2ClubService match2ClubService, ILog4Net logger, IMatchService matchService, IDropDownService dropDownService)
        {
            _mapper = mapper;
            _logger = logger;
            _matchService = matchService;
            _dropDownService = dropDownService;
            _predictionService = predictionService;
            _match2ClubService = match2ClubService;
        }
        #endregion

        // GET: Match
        [HttpGet]
        public ActionResult Index(MatchViewModel model)
        {
            try
            {
                ViewBag.SearchModel = model;
                var request = new FilteredModel<Match>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<MatchViewModel>>(_matchService.GetPaging(_mapper.Map<Match>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<MatchViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
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

        // POST: Match
        [HttpPost]
        public ActionResult List(MatchViewModel collection)
        {
            try
            {
                var request = new FilteredModel<MatchViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<MatchViewModel>>(_matchService.GetPaging(_mapper.Map<Match>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<IList<MatchViewModel>>(_matchService.GetPaging(_mapper.Map<Match>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<MatchViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_MatchGrid", ViewBag.OnePageOfEntries)
                : View();
        }

        // GET: Match/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var Match = _mapper.Map<MatchViewModel>(_matchService.GetById(id));
            return View(Match);
        }

        // POST: Match/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, MatchViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var model = _matchService.GetById(id);
                    if (model != null)
                    {
                        //model.UpdaterId = LogedInMatch.Id;
                        _matchService.Delete(model);
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


        // GET: Match/Create
        public ActionResult Create()
        {
            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", null);
            return View(new MatchViewModel());
        }

        // POST: Match/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(MatchViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (collection.AwayClubId.Value == collection.HomeClubId.Value)
                    {
                        ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection?.EventId ?? null);
                        ModelState.AddModelError(string.Empty, "میزبان و مهمان بصورت یکسان انتخاب شده اند.");
                        return View(collection);
                    }

                    if (_matchService.CheckExist(_mapper.Map<Match>(collection) , null))
                    {
                        ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection?.EventId ?? null);
                        ModelState.AddModelError(string.Empty, "این مسابقه قبلا ثبت شده است.");
                        return View(collection);
                    }

                    var model = _mapper.Map<Match>(collection);
                    model.Priority = !model.Priority.HasValue ? 0 : model.Priority;
                    model.PredictionWeight = (!model.PredictionWeight.HasValue || model.PredictionWeight.Value == 0) ? 1 : model.PredictionWeight;

                    _matchService.InsertWithLog(model, AdminHelper.AdminId);
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

        // GET: Match/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var model = _mapper.Map<MatchViewModel>(_matchService.GetById(id));
                var m2Club = _match2ClubService.Find(new Match2Club { MatchId = id }).FirstOrDefault();
                if (m2Club != null)
                {           
                    model.HomeClubId = m2Club.HomeClubId;
                    model.AwayClubId = m2Club.AwayClubId;
                    model.HomeClubScore = m2Club.HomeClubScore;
                    model.AwayClubScore = m2Club.AwayClubScore;
                    model.AwayClubName = m2Club.AwayClubName;
                    model.HomeClubName = m2Club.HomeClubName;
                }

                if (m2Club.OccurrenceDate <= DateTime.Now)
                    ModelState.AddModelError(string.Empty, "امکان ویرایش کلاب ها بدلیل اتمام مسابقه امکان پذیر نمی باشد.");

                if (m2Club.HomeClubScore.HasValue || m2Club.AwayClubScore.HasValue)
                    ModelState.AddModelError(string.Empty, "امکان ویرایش کلاب ها بدلیل مشخص شدن نتیجه مسابقه امکان پذیر نمی باشد.");

                var predicationSearchModel = _mapper.Map<Prediction>(new PredictionViewModel() { AwayClubId = m2Club.AwayClubId, HomeClubId = m2Club.HomeClubId, MatchId = m2Club.MatchId });
                if (_predictionService.Find(predicationSearchModel).Any())
                    ModelState.AddModelError(string.Empty, "امکان ویرایش کلاب ها بدلیل ثبت پیش بینی برای آن امکان پذیر نمی باشد.");

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

        // POST: Match/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, MatchViewModel collection , int OldPredictionWeight)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {

                        var oldModel = _mapper.Map<Match2ClubViewModel>(_match2ClubService.Find(new Match2Club { MatchId = id }).FirstOrDefault());
                        if (oldModel.OccurrenceDate <= DateTime.Now && (oldModel.HomeClubId != collection.HomeClubId || oldModel.AwayClubId != collection.AwayClubId))
                        {
                            collection.HomeClubId = oldModel.HomeClubId;
                            collection.AwayClubId = oldModel.AwayClubId;
                            ModelState.AddModelError(string.Empty, "امکان ویرایش کلاب ها بدلیل اتمام مسابقه امکان پذیر نمی باشد.");
                        }

                        else if ((oldModel.HomeClubScore.HasValue || oldModel.AwayClubScore.HasValue) && (oldModel.HomeClubId != collection.HomeClubId || oldModel.AwayClubId != collection.AwayClubId))
                        {
                            collection.HomeClubId = oldModel.HomeClubId;
                            collection.AwayClubId = oldModel.AwayClubId;
                            ModelState.AddModelError(string.Empty, "امکان ویرایش کلاب ها و برخی آیتم های دیگر بدلیل مشخص شدن نتیجه مسابقه امکان پذیر نمی باشد.");
                        }

                        else if ((oldModel.HomeClubScore.HasValue || oldModel.AwayClubScore.HasValue) && (collection.PredictionWeight != OldPredictionWeight))
                        {
                            collection.HomeClubId = oldModel.HomeClubId;
                            collection.AwayClubId = oldModel.AwayClubId;
                            ModelState.AddModelError(string.Empty, "امکان ویرایش امتیاز و برخی آیتم های دیگر بدلیل مشخص شدن نتیجه مسابقه امکان پذیر نمی باشد.");
                        }

                        else if (_predictionService.Find(_mapper.Map<Prediction>(new PredictionViewModel() { AwayClubId = oldModel.AwayClubId, HomeClubId = oldModel.HomeClubId, MatchId = oldModel.MatchId })).Any() && (oldModel.HomeClubId != collection.HomeClubId || oldModel.AwayClubId != collection.AwayClubId))
                        {
                            collection.HomeClubId = oldModel.HomeClubId;
                            collection.AwayClubId = oldModel.AwayClubId;
                            ModelState.AddModelError(string.Empty, "امکان ویرایش کلاب ها بدلیل ثبت پیش بینی برای آن امکان پذیر نمی باشد.");
                        }

                        var model = _mapper.Map<Match>(collection);
                        if (model.AwayClubId.Value == model.HomeClubId.Value)
                        {
                            ModelState.AddModelError(string.Empty, "میزبان و مهمان بصورت یکسان انتخاب شده اند.");
                        }

                        if (_matchService.CheckExist(_mapper.Map<Match>(collection), id))
                        {
                            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection?.EventId ?? null);
                            ModelState.AddModelError(string.Empty, "این مسابقه قبلا ثبت شده است.");
                            return View(collection);
                        }

                        if (!ModelState.IsValid)
                        {
                            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", model.EventId);
                            return View(collection);
                        }

                        model.Priority = !model.Priority.HasValue ? 0 : model.Priority;
                        _matchService.UpdateWithLog(model, AdminHelper.AdminId);
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
        public JsonResult GetMatcheshByEventId(int eventId, string key = "")
        {
            var response = new JsonViewModel { status = HttpStatusCode.InternalServerError, message = GeneralMessages.Error };
            try
            {
                response.data = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetMatches(eventId, key));
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