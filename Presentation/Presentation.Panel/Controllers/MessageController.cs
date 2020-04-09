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
    public class MessageController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService, IMapper mapper, ILog4Net logger)
        {
            _messageService = messageService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Message
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new MessageViewModel();
                long totalCount;
                var request = new FilteredModel<Message>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<MessageViewModel>>(_messageService.GetPaging(new Message(), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<MessageViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // POST: Message
        [HttpPost]
        public ActionResult Index(MessageViewModel collection)
        {
            long totalCount;
            ViewBag.SearchModel = collection;
            var request = new FilteredModel<Message>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
                var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<MessageViewModel>>(_messageService.GetPaging(_mapper.Map<Message>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<MessageViewModel>>(_messageService.GetPaging(_mapper.Map<Message>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<MessageViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Message/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                var message = _messageService.GetById(id);
                var result = _mapper.Map<MessageViewModel>(message);
                message.Status = (byte)GeneralEnums.MessageStatus.Read;
                message.LastSeenAt = DateTime.Now;
                message.LastSeenUserId = LogedInAdmin.Id;
                _messageService.Update(message);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // GET: Message/Edit/5
        public ActionResult Edit(int id)
        {
            var message = _messageService.GetById(id);
            var result = _mapper.Map<MessageViewModel>(message);
            //message.Status = GeneralEnums.MessageStatus.Read;
            //message.LastSeenAt = DateTime.Now;
            //message.LastSeenUserId = UserHelper.UserId;
            //_messageService.Save(message);
            return View(result);
        }

        // POST: Message/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, MessageViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        _messageService.Update(_mapper.Map<Message>(collection));
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

        // GET: Message/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_mapper.Map<MessageViewModel>(_messageService.GetById(id)));
        }

        // POST: Message/Delete/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Delete(int id, MessageViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    _messageService.Delete(_mapper.Map<Message>(collection));
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
    }
}