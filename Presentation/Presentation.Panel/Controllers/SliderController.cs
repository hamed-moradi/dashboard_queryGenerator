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
    public class SliderController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly ISliderService _sliderService;
        private readonly IDropDownService _dropDownService;

        public SliderController(ISliderService sliderService, IMapper mapper, ILog4Net logger, IDropDownService dropDownService)
        {
            _sliderService = sliderService;
            _mapper = mapper;
            _logger = logger;
            _dropDownService = dropDownService;
        }

        #endregion

        // GET: Slider
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new SliderViewModel();
                long totalCount;
                var request = new FilteredModel<Slider>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<SliderViewModel>>(_sliderService.GetPaging(new Slider(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<SliderViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Slider
        [HttpPost]
        public ActionResult Index(SliderViewModel collection)
        {
            long totalCount;
            ViewBag.SearchModel = collection;
            var request = new FilteredModel<Slider>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
                var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<SliderViewModel>>(_sliderService.GetPaging(_mapper.Map<Slider>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<SliderViewModel>>(_sliderService.GetPaging(_mapper.Map<Slider>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<SliderViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Slider/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                var model = _sliderService.GetById(id);
                var result = _mapper.Map<SliderViewModel>(model);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // GET: Slider/Create
        public ActionResult Create()
        {
            ViewBag.PositionsList = new SelectList(_dropDownService.GetPositions(), "id", "name", null);
            return View();
        }

        // POST: Slider/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(SliderViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Slider>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    _sliderService.Insert(model);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                ViewBag.PositionsList = new SelectList(_dropDownService.GetPositions(), "id", "name", collection.PositionId);
                return View(collection);
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
                ViewBag.PositionsList = new SelectList(_dropDownService.GetPositions(), "id", "name", collection.PositionId);
                return View(collection);
            }
        }

        // GET: Slider/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _sliderService.GetById(id);
            var result = _mapper.Map<SliderViewModel>(model);
            ViewBag.PositionsList = new SelectList(_dropDownService.GetPositions(), "id", "name", model.PositionId);
            return View(result);
        }

        // POST: Slider/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, SliderViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Slider>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _sliderService.Update(model);
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
            ViewBag.PositoinsList = new SelectList(_dropDownService.GetPositions(), "id", "name", collection.PositionId);
            return View(collection);
        }

        // GET: Slider/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_mapper.Map<SliderViewModel>(_sliderService.GetById(id)));
        }

        // POST: Slider/Delete/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Delete(int id, SliderViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var model = _mapper.Map<Slider>(collection);
                    model.UpdaterId = LogedInAdmin.Id;
                    _sliderService.Delete(model);
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