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
    public class Match2ClubController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IMatch2ClubService _match2ClubService;
        private readonly IPredictionService _predictionService;
        private readonly IDropDownService _dropDownService;


        public Match2ClubController(IMapper mapper, ILog4Net logger, IMatch2ClubService match2ClubService, IDropDownService dropDownService, IPredictionService predictionService)
        {
            _mapper = mapper;
            _logger = logger;
            _match2ClubService = match2ClubService;
            _dropDownService = dropDownService;
            _predictionService = predictionService;
        }
        #endregion

        // GET: Match2Club
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new Match2ClubViewModel();
                var request = new FilteredModel<Match2Club>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<Match2ClubViewModel>>(_match2ClubService.GetPaging(new Match2Club(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<Match2ClubViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
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

        // POST: Match2Club
        [HttpPost]
        public ActionResult Index(Match2ClubViewModel collection)
        {
            var request = new FilteredModel<Match2Club>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<Match2ClubViewModel>>(_match2ClubService.GetPaging(_mapper.Map<Match2Club>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<Match2ClubViewModel>>(_match2ClubService.GetPaging(_mapper.Map<Match2Club>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<Match2ClubViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Match2Club/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var Match2Club = _mapper.Map<Match2ClubViewModel>(_match2ClubService.GetById(id));
            return View(Match2Club);
        }

        // POST: Match2Club/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, Match2ClubViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var model = _match2ClubService.GetById(id);
                    if (model != null)
                    {
                        //model.UpdaterId = LogedInMatch2Club.Id;
                        _match2ClubService.Delete(model);
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


        // GET: Match2Club/Create
        public ActionResult Create()
        {
            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", null);
            return View();
        }

        // POST: Match2Club/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(Match2ClubViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    collection.AwayClubScore = null;
                    collection.HomeClubScore = null;

                    if (collection.AwayClubId.Value == collection.HomeClubId.Value)
                    {
                        ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection?.EventId ?? null);
                        ModelState.AddModelError(string.Empty, "میزبان و مهمان بصورت یکسان انتخاب شده اند.");
                        return View(collection);
                    }

                    var lstTournamentMatch = _match2ClubService.Find(new Match2Club { MatchId = collection.MatchId });
                    if (lstTournamentMatch.Any())
                    {
                        ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection?.EventId ?? null);
                        ModelState.AddModelError(string.Empty, "این مسابقه قبلا به کلابها تخصیص شده است.");
                        return View(collection);
                    }

                    var model = _mapper.Map<Match2Club>(collection);
                    _match2ClubService.InsertWithLog(model, AdminHelper.AdminId);
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

        // GET: Match2Club/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var model = _mapper.Map<Match2ClubViewModel>(_match2ClubService.GetById(id));
                if (model.OccurrenceDate <= DateTime.Now)
                    ModelState.AddModelError(string.Empty, "امکان ویرایش این آیتم بدلیل اتمام مسابقه امکان پذیر نمی باشد.");

                if (model.HomeClubScore.HasValue || model.AwayClubScore.HasValue)
                    ModelState.AddModelError(string.Empty, "امکان ویرایش این آیتم بدلیل مشخص شدن نتیجه مسابقه امکان پذیر نمی باشد.");

                var predicationSearchModel = _mapper.Map<Prediction>(new PredictionViewModel() { AwayClubId = model.AwayClubId, HomeClubId = model.HomeClubId, MatchId = model.MatchId });
                if (_predictionService.Find(predicationSearchModel).Any())
                    ModelState.AddModelError(string.Empty, "امکان ویرایش این آیتم بدلیل ثبت پیش بینی برای آن امکان پذیر نمی باشد.");

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

        // POST: Match2Club/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, Match2ClubViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var oldModel = _mapper.Map<Match2ClubViewModel>(_match2ClubService.GetById(id));

                        if (oldModel.OccurrenceDate <= DateTime.Now)
                        {
                            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", oldModel.EventId);
                            ModelState.AddModelError(string.Empty, "امکان ویرایش این آیتم بدلیل اتمام مسابقه امکان پذیر نمی باشد.");
                            return View(oldModel);
                        }

                        if (oldModel.HomeClubScore.HasValue || oldModel.AwayClubScore.HasValue)
                        {
                            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", oldModel.EventId);
                            ModelState.AddModelError(string.Empty, "امکان ویرایش این آیتم بدلیل مشخص شدن نتیجه مسابقه امکان پذیر نمی باشد.");
                            return View(oldModel);
                        }

                        var predicationSearchModel = _mapper.Map<Prediction>(new PredictionViewModel() { AwayClubId = oldModel.AwayClubId, HomeClubId = oldModel.HomeClubId, MatchId = oldModel.MatchId });
                        if (_predictionService.Find(predicationSearchModel).Any())
                        {
                            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", oldModel.EventId);
                            ModelState.AddModelError(string.Empty, "امکان ویرایش این آیتم بدلیل ثبت پیش بینی برای آن امکان پذیر نمی باشد.");
                            return View(oldModel);
                        }

                        var model = _mapper.Map<Match2Club>(collection);
                        if (model.AwayClubId.Value == model.HomeClubId.Value)
                        {
                            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", model.EventId);
                            ModelState.AddModelError(string.Empty, "میزبان و مهمان بصورت یکسان انتخاب شده اند.");
                            return View(collection);
                        }

                        var lstTournamentMatch = _match2ClubService.Find(new Match2Club { MatchId = collection.MatchId });
                        if (lstTournamentMatch.Any(x => x.Id != model.Id))
                        {
                            ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", collection?.EventId ?? null);
                            ModelState.AddModelError(string.Empty, "این مسابقه قبلا به کلابها تخصیص شده است.");
                            return View(collection);
                        }

                        model.AwayClubScore = null;
                        model.HomeClubScore = null;
                        var resultCode = _match2ClubService.UpdateWithLog(model, AdminHelper.AdminId);
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


        // GET: Match2Club/SetScore/5
        /// <summary>
        /// ثبت امتیاز
        /// </summary>
        /// <param name="matchId">شناسه مسابقه</param>
        /// <returns></returns>
        public ActionResult SetScore(int matchId)
        {
            try
            {
                var model = _mapper.Map<Match2ClubViewModel>(_match2ClubService.Find(new Match2Club { MatchId = matchId }).FirstOrDefault());
                if (model.HomeClubScore.HasValue || model.AwayClubScore.HasValue)
                    ModelState.AddModelError(string.Empty, "امکان ویرایش این آیتم بدلیل مشخص شدن نتیجه مسابقه امکان پذیر نمی باشد.");

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

        // POST: Match2Club/SetScore/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult SetScore(int id, Match2ClubViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var model = _match2ClubService.GetById(id);
                    if (model.HomeClubScore.HasValue || model.AwayClubScore.HasValue)
                    {
                        ModelState.AddModelError(string.Empty, "امکان ویرایش این آیتم بدلیل مشخص شدن نتیجه مسابقه امکان پذیر نمی باشد.");
                        return View(collection);
                    }

                    model.HomeClubScore = collection.HomeClubScore;
                    model.AwayClubScore = collection.AwayClubScore;
                    if (!model.HomeClubScore.HasValue || !model.AwayClubScore.HasValue)
                    {
                        ModelState.AddModelError(string.Empty, "جهت تعیین امتیازات لطفا هر دو امتیاز کلاب میزبان و میهمان را وارد نمایید.");
                        return View(collection);
                    }

                    var resultCode = _match2ClubService.SetScore(model, AdminHelper.AdminId);
                    switch (resultCode)
                    {
                        case -1:
                            ModelState.AddModelError(string.Empty, "مسابقه یافت نشد");
                            return View(collection);
                        case -2:
                            ModelState.AddModelError(string.Empty, "مسابقه فعال نمی باشد");
                            return View(collection);
                        case -3:
                            ModelState.AddModelError(string.Empty, "رویداد یافت نشد");
                            return View(collection);
                        case -4:
                            ModelState.AddModelError(string.Empty, "رویداد فعال نمی باشد");
                            return View(collection);
                        case -5:
                            ModelState.AddModelError(string.Empty, "کلاب میزبان یافت نشد");
                            return View(collection);
                        case -6:
                            ModelState.AddModelError(string.Empty, "کلاب میزبان فعال نمی باشد");
                            return View(collection);
                        case -7:
                            ModelState.AddModelError(string.Empty, "کلاب مهمان یافت نشد");
                            return View(collection);
                        case -8:
                            ModelState.AddModelError(string.Empty, "کلاب مهمان فعال نمی باشد");
                            return View(collection);
                        case -9:
                            ModelState.AddModelError(string.Empty, "افراد کلاب یافت نشدند");
                            return View(collection);
                        case -10:
                            ModelState.AddModelError(string.Empty, "افراد کلاب فعال نمی باشد");
                            return View(collection);
                        case -11:
                            ModelState.AddModelError(string.Empty, "ارتباطی بین کلابها و مسابقه وجود ندارد");
                            return View(collection);
                        case -12:
                            ModelState.AddModelError(string.Empty, "نتیجه این مسابقه قبلا ثبت شده است");
                            return View(collection);
                    }

                    return RedirectToAction("Index", "Match");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
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