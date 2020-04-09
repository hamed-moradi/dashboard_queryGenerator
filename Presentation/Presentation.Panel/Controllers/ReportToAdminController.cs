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
    public class ReportToAdminController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IReportToAdminService _reportToAdminService;

        public ReportToAdminController(IReportToAdminService reportToAdminService, IMapper mapper, ILog4Net logger)
        {
            _reportToAdminService = reportToAdminService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: ReportToAdmin
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new ReportToAdminViewModel();
                long totalCount;
                var request = new FilteredModel<ReportToAdmin>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ReportToAdminViewModel>>(_reportToAdminService.GetPaging(new ReportToAdmin(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ReportToAdminViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: ReportToAdmin
        [HttpPost]
        public ActionResult Index(ReportToAdminViewModel collection)
        {
            long totalCount;
            var request = new FilteredModel<ReportToAdmin>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result =
                _mapper.Map<IList<ReportToAdminViewModel>>(_reportToAdminService.GetPaging(_mapper.Map<ReportToAdmin>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result =
                    _mapper.Map<IList<ReportToAdminViewModel>>(_reportToAdminService.GetPaging(_mapper.Map<ReportToAdmin>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ReportToAdminViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: ReportToAdmin/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                var model = _reportToAdminService.GetById(id);
                model.ReadAt = model.ReadAt ?? DateTime.Now;
                model.UpdaterId = LogedInAdmin.Id;
                _reportToAdminService.Update(model);
                return View(_mapper.Map<ReportToAdminViewModel>(model));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // GET: ReportToAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<ReportToAdminViewModel>(_reportToAdminService.GetById(id));
            return View(model);
        }

        // POST: ReportToAdmin/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, ReportToAdminViewModel collection, string selectedTags)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<ReportToAdmin>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _reportToAdminService.Update(model);
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

        // GET: ReportToAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_mapper.Map<ReportToAdminViewModel>(_reportToAdminService.GetById(id)));
        }

        // POST: ReportToAdmin/Delete/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Delete(int id, ReportToAdminViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _reportToAdminService.Delete(_mapper.Map<ReportToAdmin>(collection));
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