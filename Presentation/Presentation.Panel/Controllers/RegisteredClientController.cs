using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Domain.Model.Entities;
using PagedList;
using Presentation.Panel.FilterAttributes;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class RegisteredClientController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IRegisteredClientService _registeredClientService;

        public RegisteredClientController(IRegisteredClientService registeredClientService, IMapper mapper, ILog4Net logger)
        {
            _registeredClientService = registeredClientService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: RegisteredClient
        public ActionResult Index()
        {
            try
            {
                var result = _mapper.Map<IList<RegisteredClientViewModel>>(_registeredClientService.Find(new RegisteredClient()));
                ViewBag.Enteries = result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // GET: RegisteredClient/Details/5
        public ActionResult Details(int id)
        {
            RegisteredClientViewModel model;
            try
            {
                model = _mapper.Map<RegisteredClientViewModel>(_registeredClientService.GetById(id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }

            return View(model);
        }

        // POST: RegisteredClient/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, RegisteredClientViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    _registeredClientService.Delete(_mapper.Map<RegisteredClient>(collection));
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

        // GET: RegisteredClient/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<RegisteredClientViewModel>(_registeredClientService.GetById(id));
            return View(model);
        }

        // POST: RegisteredClient/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(RegisteredClientViewModel collection)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<RegisteredClient>(collection);
                    _registeredClientService.Update(model);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);

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
