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
    public class PositionController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IPositionService _positionService;
        private readonly IDropDownService _dropDownService;

        public PositionController(IPositionService positionService, IMapper mapper, ILog4Net logger, IDropDownService dropDownService)
        {
            _dropDownService = dropDownService;
            _positionService = positionService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Position
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new PositionViewModel();
                long totalCount;
                var request = new FilteredModel<Position>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<PositionViewModel>>(_positionService.GetPaging(new Position(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<PositionViewModel>(result, request.PageIndex, request.PageSize, (int) totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Position
        [HttpPost]
        public ActionResult Index(PositionViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<Position>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
                var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<PositionViewModel>>(_positionService.GetPaging(_mapper.Map<Position>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int) (totalCount/request.PageSize);
                if (totalCount%request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<PositionViewModel>>(_positionService.GetPaging(_mapper.Map<Position>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<PositionViewModel>(result, request.PageIndex, request.PageSize, (int) totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Position/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<PositionViewModel>(_positionService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // GET: Position/Create
        public ActionResult Create()
        {
            ViewBag.PositionsList = new SelectList(_dropDownService.GetPositions(), "id", "name", null);
            return View();
        }

        // POST: Position/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(PositionViewModel collection, string selectedPositions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Position>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    _positionService.Insert(model);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
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
                return View(collection);
            }
        }

        // GET: Position/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<PositionViewModel>(_positionService.GetById(id));
            return View(model);
        }

        // POST: Position/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, PositionViewModel collection, string selectedPositions)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<Position>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _positionService.Update(model);
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

        // GET: Position/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_mapper.Map<PositionViewModel>(_positionService.GetById(id)));
        }

        // POST: Position/Delete/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Delete(int id, PositionViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _positionService.Delete(_mapper.Map<Position>(collection));
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