using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using Domain.Application;
using MD.PersianDateTime;
using Newtonsoft.Json;
using Presentation.Panel.FilterAttributes;
using Presentation.Panel.Helpers;
using Presentation.Panel.Models;

namespace Presentation.Panel.Controllers
{
    public class AccountController : BaseController
    {
        #region Constructor
        private readonly ILog4Net _logger;
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;
        private readonly ICandooSmsService _candooSmsService;

        public AccountController(IAdminService adminService, IMapper mapper, ILog4Net logger, ICandooSmsService candooSmsService)
        {
            _adminService = adminService;
            _candooSmsService = candooSmsService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        // GET: Account
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/SignIn
        [HttpPost]
        [SecurityFilter]
        [AllowAnonymous]
        public ActionResult SignIn(string username, string password, string returnUrl)
        {
            try
            {
                ViewBag.Username = username;
                ViewBag.Password = password;
                password = CommonHelper.Md5Password(password);
                var admin = _adminService.SignIn(username, password);
                if (admin != null)
                {
                    if (admin.Status == (byte)GeneralEnums.Status.Active)
                    {
                        var serializeModel = new CustomPrincipalSerializeModel
                        {
                            Id = admin.Id.Value,
                            FullName = admin.FullName,
                            Avatar = admin.Avatar,
                            LastLogin = new PersianDateTime().ToString(),
                            IP = AdminHelper.CurrentIp
                        };

                        var userData = JsonConvert.SerializeObject(serializeModel);
                        var authTicket = new FormsAuthenticationTicket(Convert.ToInt32(admin.Id), admin.FullName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);
                        var encTicket = FormsAuthentication.Encrypt(authTicket);
                        var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        Response.Cookies.Add(faCookie);
                        _adminService.SetLoginDate(admin);
                        return Redirect(returnUrl ?? "/Home");
                    }
                    ModelState.AddModelError(string.Empty, "این کاربر غیرفعال می باشد لطفا با مدیر سیستم تماس بگیرید");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "نام کاربری یا گذرواژه اشتباه می باشد");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        // Post: Account/SignOut
        [HttpPost]
        [AuthenticationFilter]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn", "Account");
        }

        // GET: Account/ForgetPassword/09120000000
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgetPassword(string cellPhone)
        {
            try
            {
                var user = _adminService.GetByCellPhone(cellPhone);
                if (user == null)
                {
                    return Content("error|این شماره موبایل در سیستم وجود ندارد.", "text/html");
                }

                if (user.Status != (byte)GeneralEnums.Status.Active)
                {
                    return Content("error|این کاربر غیرفعال می باشد. لطفا با مدیر سیستم تماس بگیرید.", "text/html");
                }
                var newPass = new Random().Next(100000, 999999).ToString();
                var response = string.Empty;
                _candooSmsService.SendSms(new[] { Convert.ToInt64(user.Phone) }, newPass );
                if (string.IsNullOrWhiteSpace(response))
                {
                    user.Password = CommonHelper.Md5Password(newPass);
                    _adminService.ChangePassword(user);
                    return Content("success|پیامکی حاوی کلمه عبور جدید برای شما ارسال شد.", "text/html");
                }
                else
                {
                    return Content("error|اشکال در ارسال پیامک.", "text/html");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return Content("error|خطایی رخ داده است. لطفا با مدیر سیستم تماس بگیرید.", "text/html");
        }

        // GET: Account/ChangePassword/5
        [AuthenticationFilter]
        public ActionResult ChangePassword()
        {
            try
            {
                var model = _mapper.Map<ChangePasswordViewModel>(_adminService.GetById(LogedInAdmin.Id));
                model.CurrentPassword = "";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Account/ChangePassword/5
        [HttpPost]
        [SecurityFilter]
        [AuthenticationFilter]
        public ActionResult ChangePassword(ChangePasswordViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentPassword = CommonHelper.Md5Password(collection.CurrentPassword);
                    var baseModel = _adminService.GetById(LogedInAdmin.Id);
                    if (baseModel.Password != currentPassword)
                    {
                        ModelState.AddModelError(string.Empty, "گذرواژه جاری اشتباه است");
                        return View(collection);
                    }
                    var saveModel = _mapper.Map<Admin>(collection);
                    //saveModel.UpdaterId = LogedInAdmin.Id;
                    _adminService.ChangePassword(saveModel);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View(collection);
        }
    }
}