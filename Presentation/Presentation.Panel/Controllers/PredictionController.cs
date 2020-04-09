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
    public class PredictionController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IPredictionService _predictionService;
        private readonly IPredictionStatisticsService _predictionStatisticsService;
        private readonly IDropDownService _dropDownService;


        public PredictionController(IMapper mapper, ILog4Net logger, IPredictionService predictionService, IDropDownService dropDownService, IPredictionStatisticsService predictionStatisticsService)
        {
            _mapper = mapper;
            _logger = logger;
            _predictionService = predictionService;
            _dropDownService = dropDownService;
            _predictionStatisticsService = predictionStatisticsService;
        }
        #endregion

        // GET: Prediction
        [HttpGet]
        public ActionResult Index(PredictionViewModel model)
        {

            try
            {
                ViewBag.Statistics = _mapper.Map<PredictionStatisticsViewModel>(_predictionStatisticsService.GetStatistics(_mapper.Map<Prediction>(model)));
                ViewBag.SearchModel = model;
                var request = new FilteredModel<Prediction>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<PredictionViewModel>>(_predictionService.GetPaging(_mapper.Map<Prediction>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<PredictionViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
                ViewBag.EventsList = new SelectList(_dropDownService.GetEvents(), "id", "name", null);
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

        // POST: Prediction
        [HttpPost]
        public ActionResult List(PredictionViewModel collection)
        {
            try
            {
                if (collection.HomeClubId.HasValue && collection.HomeClubId.Value == 0)
                {
                    collection.HomeClubId = null;
                }
                if (collection.MatchId.HasValue && collection.MatchId.Value == 0)
                {
                    collection.MatchId = null;
                }

                var request = new FilteredModel<PredictionViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<PredictionViewModel>>(_predictionService.GetPaging(_mapper.Map<Prediction>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<IList<PredictionViewModel>>(_predictionService.GetPaging(_mapper.Map<Prediction>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<PredictionViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
                ViewBag.Statistics = _mapper.Map<PredictionStatisticsViewModel>(_predictionStatisticsService.GetStatistics(_mapper.Map<Prediction>(collection)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_PredictionGrid", ViewBag.OnePageOfEntries)
                : View();
        }
    }
}