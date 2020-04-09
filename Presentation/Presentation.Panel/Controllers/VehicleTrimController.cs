﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asset.Infrastructure._App;
using AutoMapper;
using Domain.Model.Entities;
using Domain.Application;
using Newtonsoft.Json;
using PagedList;
using Presentation.Panel.Models;
using Presentation.Panel.FilterAttributes;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Controllers
{
    public class VehicleTrimController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IVehicleTrimService _vehicleTrimService;
        private readonly IDropDownService _dropDownService;

        public VehicleTrimController(IVehicleTrimService vehicleTrimService, IDropDownService dropDownService, IMapper mapper, ILog4Net logger)
        {
            _vehicleTrimService = vehicleTrimService;
            _dropDownService = dropDownService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        // GET: VehicleTrim
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new VehicleTrimViewModel();
                var request = new FilteredModel<VehicleTrim>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<VehicleTrimViewModel>>(_vehicleTrimService.GetPaging(new VehicleTrim(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<VehicleTrimViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: VehicleTrim
        [HttpPost]
        public ActionResult Index(VehicleTrimViewModel collection)
        {
            var request = new FilteredModel<VehicleTrim>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<VehicleTrimViewModel>>(_vehicleTrimService.GetPaging(_mapper.Map<VehicleTrim>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<VehicleTrimViewModel>>(_vehicleTrimService.GetPaging(_mapper.Map<VehicleTrim>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<VehicleTrimViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: VehicleTrim/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<VehicleTrimViewModel>(_vehicleTrimService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: VehicleTrim/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, VehicleTrimViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _vehicleTrimService.Delete(_mapper.Map<VehicleTrim>(collection));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }

        // GET: VehicleTrim/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleTrim/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(VehicleTrimViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<VehicleTrim>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    _vehicleTrimService.Insert(model);
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
                    if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                }
            }
            return View(collection);
        }

        // GET: VehicleTrim/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_mapper.Map<VehicleTrimViewModel>(_vehicleTrimService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: VehicleTrim/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, VehicleTrimViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<VehicleTrim>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _vehicleTrimService.Update(model);
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
            var result = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetVehicleTrims(key, (p.GetValueOrDefault(1) - 1) * 30, 30, out long totalCount));
            return Json(new DropDownModel { TotalCount = totalCount, Items = result }, JsonRequestBehavior.AllowGet);
        }
    }
}