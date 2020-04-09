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
    public class CustomerController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly ICustomerService _customerService;
        private readonly ICommentService _commentService;
        private readonly ICustomerVehicleService _customerVehicleService;
        private readonly ISessionService _sessionService;
        private readonly IUserService _userService;

        public CustomerController(ICustomerService customerService, ICommentService commentService, ICustomerVehicleService customerVehicleService, ISessionService sessionService, IMapper mapper, ILog4Net logger, IUserService userService)
        {
            _customerService = customerService;
            _commentService = commentService;
            _customerVehicleService = customerVehicleService;
            _sessionService = sessionService;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        #endregion

        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new CustomerViewModel();
                var request = new FilteredModel<User>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<CustomerViewModel>>(_customerService.GetPaging(new User(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<CustomerViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: User
        [HttpPost]
        public ActionResult Index(CustomerViewModel collection)
        {
            var request = new FilteredModel<User>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var user = _mapper.Map<User>(collection);
            user.Password = null;
            var result = _mapper.Map<IList<CustomerViewModel>>(_customerService.GetPaging(user, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<CustomerViewModel>>(_customerService.GetPaging(user, out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<CustomerViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: User/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<CustomerViewModel>(_customerService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: User/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, CustomerViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _customerService.Delete(_mapper.Map<User>(collection));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, GeneralMessages.EmptyId);
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

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(CustomerViewModel collection)
        {
            try
            {
                collection.IdentityProviderId = GeneralEnums.IdentityProvider.Dashboard;
                collection.UserTypeId = GeneralEnums.UserType.Customer;

                ModelState["UserTypeId"].Errors.Clear();
                UpdateModel(collection);

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrWhiteSpace(collection.CellPhone) || !string.IsNullOrWhiteSpace(collection.Email))
                    {
                        if (collection.CellPhone != null && collection.CellPhone.StartsWith("0"))
                        {
                            collection.CellPhone = collection.CellPhone.Remove(0, 1);
                        }
                        var model = _mapper.Map<User>(collection);
                        model.CreatorId = LogedInAdmin.Id;
                        model.IdentityProviderId = (byte)GeneralEnums.IdentityProvider.Dashboard;
                        if (!string.IsNullOrWhiteSpace(model.CellPhone))
                        {
                            model.CellPhoneVerifiedAt = DateTime.Now;
                        }
                        if (!string.IsNullOrWhiteSpace(model.Email))
                        {
                            model.EmailVerifiedAt = DateTime.Now;
                        }
                        var result = _customerService.Insert(model);
                        if (result > 0)
                        {
                            return RedirectToAction("Index");
                        }
                        if (Enum.IsDefined(typeof(GeneralEnums.UserInsertError), result) == true)
                        {
                            ModelState.AddModelError(string.Empty, EnumHelper<GeneralEnums.UserInsertError>.GetDisplayValue((GeneralEnums.UserInsertError)result));
                            return View(collection);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "شماره تلفن یا پست الکترونیکی را وارد نمایید");
                    }
                }
                else
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            ModelState.AddModelError(string.Empty, modelError.ErrorMessage);
                        }
                    }

                }
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
            }
            return View(collection);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<CustomerViewModel>(_customerService.GetById(id));
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, CustomerViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        var model = _mapper.Map<User>(collection);
                        model.UpdaterId = LogedInAdmin.Id;
                        _customerService.Update(model);
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
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View(collection);
            }
        }

        // GET: User/Comments
        [HttpGet]
        public ActionResult Comments(int userId)
        {
            ViewBag.UserId = userId;
            try
            {
                ViewBag.SearchModel = new CommentViewModel();
                var request = new FilteredModel<Comment>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(new Comment { UserCreatorId = userId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<CommentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);

                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: User/Comments
        [HttpPost]
        public ActionResult Comments(CommentViewModel collection)
        {
            var request = new FilteredModel<Comment>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(_mapper.Map<Comment>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<CommentViewModel>>(_commentService.GetPaging(_mapper.Map<Comment>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<CommentViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: User/Comments
        [HttpGet]
        public ActionResult Vehicles(int userId)
        {
            ViewBag.CustomerId = userId;
            try
            {
                ViewBag.SearchModel = new CustomerVehicleViewModel();
                var request = new FilteredModel<CustomerVehicle>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<CustomerVehicleViewModel>>(_customerVehicleService.GetPaging(new CustomerVehicle { CustomerId = userId }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<CustomerVehicleViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);

                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: User/Vehicles
        [HttpPost]
        public ActionResult Vehicles(CustomerVehicleViewModel collection)
        {
            var request = new FilteredModel<CustomerVehicle>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<CustomerVehicleViewModel>>(_customerVehicleService.GetPaging(_mapper.Map<CustomerVehicle>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<CustomerVehicleViewModel>>(_customerVehicleService.GetPaging(_mapper.Map<CustomerVehicle>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<CustomerVehicleViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // POST: User/SendNotification
        [HttpPost]
        [SecurityFilter]
        public ActionResult SendNotification(UserViewModel collection, IosNotificationModel notification)
        {
            string response;
            try
            {
                collection.UserTypeId = GeneralEnums.UserType.Customer;
                var user = _mapper.Map<User>(collection);
                user.Password = null;
                var result = _sessionService.SendNotification(user, notification, LogedInAdmin.Id);
                response = result.ToLower().Contains("results") ? "اعلان ها ارسال شدند" : result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                response = GeneralMessages.UnexpectedError;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult Verify(int id)
        //{
        //    var user = _customerService.GetById(id);
        //    var result = _userService.CheckEmailCellphoneForVerify(user);
        //    if (result != 1)
        //    {
        //        if (Enum.IsDefined(typeof(GeneralEnums.UserInsertError), result) == true)
        //        {
        //            ModelState.AddModelError(string.Empty, EnumHelper<GeneralEnums.UserInsertError>.GetDisplayValue((GeneralEnums.UserInsertError)result));
        //            return View("Details", _mapper.Map<CustomerViewModel>(user));
        //        }
        //        ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
        //        return View("Details", _mapper.Map<CustomerViewModel>(user));
        //    }
        //    var date = DateTime.Now;
        //    if (!string.IsNullOrWhiteSpace(user.CellPhone))
        //    {
        //        user.CellPhoneVerifiedAt = date;
        //    }
        //    if (!string.IsNullOrWhiteSpace(user.Email))
        //    {
        //        user.EmailVerifiedAt = date;
        //    }
        //    user.Status = (byte)GeneralEnums.Status.Active;
        //    user.UpdaterId = LogedInAdmin.Id;
        //    _userService.Update(user);
        //    ModelState.AddModelError(string.Empty, "کاربر با موفقیت تایید شد");
        //    return View("Details", _mapper.Map<CustomerViewModel>(user));
        //}
        [HttpPost]
        public ActionResult ChangeActivationStatus(int id, UserEditStatus status)
        {
            var user = _userService.GetById(id);
            if (status == UserEditStatus.Active)
            {
                user.Status = (byte)UserEditStatus.Inactive;
            }
            else
            {
                user.Status = (byte)UserEditStatus.Active;
            }
            user.UpdaterId = LogedInAdmin.Id;
            _userService.Update(user);
            ModelState.AddModelError(string.Empty, $"کاربر با موفقیت {(status == UserEditStatus.Active ? "غیر فعال" : "فعال")} شد");
            return View("Details", _mapper.Map<CustomerViewModel>(user));
        }
    }
}