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
using System.IO;
using Presentation.Panel.FilterAttributes;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Controllers
{
    public class AutomatedMessageController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IAutomatedMessageService _automatedMessageService;
        private readonly IAutomatedMessageParameterService _automatedMessageParameterService;
        public AutomatedMessageController(IMapper mapper, ILog4Net logger, IAutomatedMessageService automatedMessageService, IAutomatedMessageParameterService automatedMessageParameterService)
        {
            _mapper = mapper;
            _logger = logger;
            _automatedMessageService = automatedMessageService;
            _automatedMessageParameterService = automatedMessageParameterService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new AutomatedMessageViewModel { PageOrderBy = nameof(AutomatedMessageViewModel.Id) };
                var request = new FilteredModel<AutomatedMessage> { OrderBy = nameof(AutomatedMessageViewModel.Id) };
                var result = _mapper.Map<IList<AutomatedMessageViewModel>>(_automatedMessageService.GetPaging(new AutomatedMessage(), out long totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<AutomatedMessageViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(AutomatedMessageViewModel model)
        {
            var request = new FilteredModel<AutomatedMessage>
            {
                PageIndex = model.ThisPageIndex,
                Order = model.PageOrder,
                OrderBy = model.PageOrderBy
            };
            var result = _mapper.Map<IList<AutomatedMessageViewModel>>(_automatedMessageService.GetPaging(_mapper.Map<AutomatedMessage>(model), out long totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<AutomatedMessageViewModel>>(_automatedMessageService.GetPaging(_mapper.Map<AutomatedMessage>(model), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<AutomatedMessageViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = model;
            return View();
        }
        public ActionResult Details(int id)
        {
            try
            {
                var model = _mapper.Map<AutomatedMessageViewModel>(_automatedMessageService.Single(new AutomatedMessage { Id=id }));
                ViewBag.Parameters = _mapper.Map<List<AutomatedMessageParameterViewModel>>(_automatedMessageParameterService.GetPaging(new AutomatedMessageParameter { AutomatedMessageId = id }, out long totalCount, take: int.MaxValue));
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            try
            {
                var model = _mapper.Map<AutomatedMessageViewModel>(_automatedMessageService.Single(new AutomatedMessage { Id = id }));
                ViewBag.Parameters = _mapper.Map<List<AutomatedMessageParameterViewModel>>(_automatedMessageParameterService.GetPaging(new AutomatedMessageParameter { AutomatedMessageId = id }, out long totalCount, take: int.MaxValue));
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }
        [HttpPost]
        public ActionResult Edit(int id,AutomatedMessageViewModel collection)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(collection.UniqueKey) || string.IsNullOrWhiteSpace(collection.ChannelKey))
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        if (collection.TypeId == GeneralEnums.AutomatedMessageType.SMS)
                        {
                            collection.Subject = null;
                        }
                        var model = _mapper.Map<AutomatedMessage>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        model.UpdatedAt = DateTime.Now;
                        _automatedMessageService.Update(model);
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            ViewBag.Parameters = _mapper.Map<List<AutomatedMessageParameterViewModel>>(_automatedMessageParameterService.GetPaging(new AutomatedMessageParameter { AutomatedMessageId = id }, out long totalCount, take: int.MaxValue));
            return View("Edit", collection);
        }
    }
}