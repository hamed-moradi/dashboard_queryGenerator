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

namespace Presentation.Panel.Controllers
{
    public class AdminController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IAdminService _adminService;
        private readonly ICandooSmsService _smsService;
        private readonly IDropDownService _dropDownService;

        public AdminController(IMapper mapper, ILog4Net logger, IAdminService adminService, ICandooSmsService smsService, IDropDownService dropDownService)
        {
            _mapper = mapper;
            _logger = logger;
            _adminService = adminService;
            _smsService = smsService;
            _dropDownService = dropDownService;
        }
        #endregion

        // GET: Admin
        [HttpGet]
        public ActionResult Index(AdminViewModel model)
        {
            try
            {
                ViewBag.SearchModel = model;
                var request = new FilteredModel<Admin>();
                var offset = (model.ThisPageIndex - 1) * model.ThisPageSize;

                var result = _mapper.Map<IList<AdminViewModel>>(_adminService.GetPaging(_mapper.Map<Admin>(model), out long totalCount, model.PageOrderBy, model.PageOrder, offset, model.ThisPageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<AdminViewModel>(result, model.ThisPageIndex, model.ThisPageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();;
        }

        // POST: Admin
        [HttpPost]
        public ActionResult List(AdminViewModel collection)
        {
            try
            {
                var request = new FilteredModel<AdminViewModel>
                {
                    PageIndex = collection.ThisPageIndex,
                    Order = collection.PageOrder,
                    OrderBy = collection.PageOrderBy,
                    PageSize = collection.ThisPageSize
                };

                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<AdminViewModel>>(_adminService.GetPaging(_mapper.Map<Admin>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
                {
                    request.PageIndex = (int)(totalCount / request.PageSize);
                    if (totalCount % request.PageSize > 0)
                    {
                        request.PageIndex++;
                    }
                    offset = (request.PageIndex - 1) * request.PageSize;
                    result = _mapper.Map<IList<AdminViewModel>>(_adminService.GetPaging(_mapper.Map<Admin>(collection), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                }
                ViewBag.OnePageOfEntries = new StaticPagedList<AdminViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                ViewBag.SearchModel = collection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AdminGrid", ViewBag.OnePageOfEntries)
                : View();
        }

        // GET: Admin/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var admin = _mapper.Map<AdminViewModel>(_adminService.GetById(id));
            return View(admin);
        }

        // POST: Admin/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, AdminViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    var model = _adminService.GetById(id);
                    if (model != null)
                    {
                        //model.UpdaterId = LogedInAdmin.Id;
                        _adminService.Delete(model);
                        if (!string.IsNullOrEmpty(collection.PreviousUrl))
                        {
                            return Redirect(collection.PreviousUrl);
                        }
                        return RedirectToAction("Index");
                    }
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

        // GET: Admin/ResetPassword/5
        [HttpGet]
        public ActionResult ResetPassword(int id)
        {
            Admin admin = null;
            try
            {
                admin = _adminService.GetById(id);
                if (admin != null)
                {
                    var newPass = new Random().Next(100000, 999999).ToString();
                    var response = string.Empty;                    

                    _smsService.SendSms(new[] { Convert.ToInt64(admin.Phone) }, newPass );
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        admin.Password = CommonHelper.Md5Password(newPass);
                        //admin.UpdatedAt = DateTime.Now;
                        //admin.UpdaterId = LogedInAdmin.Id;
                        _adminService.ChangePassword(admin);
                        ModelState.AddModelError(string.Empty, "گذرواژه جدید ایجاد و با پیامک برای کاربر ارسال شد");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "مشکل در ارسال پیامک");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.NotFound);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View("Details", _mapper.Map<AdminViewModel>(admin));
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            var model = new AdminViewModel();
            ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", null);
            return View(model);
        }

        // POST: Admin/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(AdminViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_adminService.Find(new Admin { Phone = collection.Phone }).Any())
                    {
                        ModelState.AddModelError("", "شماره تلفن وارد شده قبلا ثبت شده است.");
                        ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", collection.RoleId);
                        return View(collection);
                    }

                    if (_adminService.Find(new Admin { Email = collection.Email }).Any())
                    {
                        ModelState.AddModelError("", "آدرس ایمیل وارد شده قبلا ثبت شده است.");
                        ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", collection.RoleId);
                        return View(collection);
                    }

                    var model = _mapper.Map<Admin>(collection);
                    //model.CreatorId = LogedInAdmin.Id;
                    _adminService.InsertWithLog(model, AdminHelper.AdminId);
                    if (!string.IsNullOrEmpty(collection.PreviousUrl))
                    {
                        return Redirect(collection.PreviousUrl);
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", collection.RoleId);
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
                    if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                }
                ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", collection.RoleId);
            }
            return View(collection);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var model = _mapper.Map<AdminViewModel>(_adminService.GetById(id));
                model.ConfirmPassword = model.Password;
                ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", model.RoleId);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, AdminViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        if (_adminService.Find(new Admin { Phone = collection.Phone }).Any(x => x.Id != id))
                        {
                            ModelState.AddModelError("", "شماره تلفن وارد شده قبلا ثبت شده است.");
                            ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", collection.RoleId);
                            return View(collection);
                        }

                        if (_adminService.Find(new Admin { Email = collection.Email }).Any(x => x.Id != id))
                        {
                            ModelState.AddModelError("", "آدرس ایمیل وارد شده قبلا ثبت شده است.");
                            ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", collection.RoleId);
                            return View(collection);
                        }

                        collection.UpdaterId = LogedInAdmin.Id;
                        var model = _mapper.Map<Admin>(collection);
                        _adminService.UpdateWithLog(model, AdminHelper.AdminId);
                        if (!string.IsNullOrEmpty(collection.PreviousUrl))
                        {
                            return Redirect(collection.PreviousUrl);
                        }
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
                }
                ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", collection.RoleId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                ViewBag.RolesList = new SelectList(_dropDownService.GetRoles(), "id", "name", collection.RoleId);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }
    }
}