using System;
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
    public class VehicleModelController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IVehicleModelService _vehicleModelService;
        private readonly IDropDownService _dropDownService;

        public VehicleModelController(IVehicleModelService vehicleModelService, IDropDownService dropDownService, IMapper mapper, ILog4Net logger)
        {
            _vehicleModelService = vehicleModelService;
            _dropDownService = dropDownService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        // GET: VehicleModel
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new VehicleModelViewModel();
                var request = new FilteredModel<VehicleModel>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<VehicleModelViewModel>>(_vehicleModelService.GetPaging(new VehicleModel(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<VehicleModelViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: VehicleModel
        [HttpPost]
        public ActionResult Index(VehicleModelViewModel collection)
        {
            var request = new FilteredModel<VehicleModel>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<VehicleModelViewModel>>(_vehicleModelService.GetPaging(_mapper.Map<VehicleModel>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<VehicleModelViewModel>>(_vehicleModelService.GetPaging(_mapper.Map<VehicleModel>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<VehicleModelViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: VehicleModel/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<VehicleModelViewModel>(_vehicleModelService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: VehicleModel/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, VehicleModelViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _vehicleModelService.Delete(_mapper.Map<VehicleModel>(collection));
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

        // GET: VehicleModel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleModel/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(VehicleModelViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<VehicleModel>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    _vehicleModelService.Insert(model);
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

        // GET: VehicleModel/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_mapper.Map<VehicleModelViewModel>(_vehicleModelService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: VehicleModel/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, VehicleModelViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<VehicleModel>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _vehicleModelService.Update(model);
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
            if (string.IsNullOrWhiteSpace(key)) return null;
            var result = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetVehicleModels(key, (p.GetValueOrDefault(1) - 1) * 30, 30, out long totalCount));
            return Json(new DropDownModel { TotalCount = totalCount, Items = result }, JsonRequestBehavior.AllowGet);
        }
    }
}