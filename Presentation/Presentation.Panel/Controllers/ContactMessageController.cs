using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using PagedList;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class ContactMessageController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IContactMessageService _contactMessageService;

        public ContactMessageController(IContactMessageService contactMessageService, IMapper mapper, ILog4Net logger)
        {
            _contactMessageService = contactMessageService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Comment
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new ContactMessageViewModels();
                var request = new FilteredModel<ContactMessage>();
                request.OrderBy = "Id";
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ContactMessageViewModels>>(_contactMessageService.GetPaging(new ContactMessage(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ContactMessageViewModels>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactMessageViewModels collection)
        {
            var request = new FilteredModel<ContactMessage>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<ContactMessageViewModels>>(_contactMessageService.GetPaging(_mapper.Map<ContactMessage>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<ContactMessageViewModels>>(_contactMessageService.GetPaging(_mapper.Map<ContactMessage>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ContactMessageViewModels>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Message/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                var message = _contactMessageService.GetById(id);
                var result = _mapper.Map<ContactMessageViewModels>(message);
                message.Status = (byte)GeneralEnums.MessageStatus.Read;
                message.LastSeenAt = DateTime.Now;
                message.LastSeenUserId = LogedInAdmin.Id;
                _contactMessageService.Update(message);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

    }
}