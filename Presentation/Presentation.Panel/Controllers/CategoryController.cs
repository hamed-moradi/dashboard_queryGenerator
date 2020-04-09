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
    public class CategoryController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, IMapper mapper, ILog4Net logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        // GET: Category
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new CategoryViewModel();
                var request = new FilteredModel<Category>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<CategoryViewModel>>(_categoryService.GetPaging(new Category(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<CategoryViewModel>(result, request.PageIndex, request.PageSize, (int) totalCount);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Category
        [HttpPost]
        public ActionResult Index(CategoryViewModel collection)
        {
            var request = new FilteredModel<Category>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<CategoryViewModel>>(_categoryService.GetPaging(_mapper.Map<Category>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int) (totalCount/request.PageSize);
                if (totalCount%request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<CategoryViewModel>>(_categoryService.GetPaging(_mapper.Map<Category>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<CategoryViewModel>(result, request.PageIndex, request.PageSize, (int) totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Category/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<CategoryViewModel>(_categoryService.GetById(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                return View();
            }
        }

        // POST: Category/Details/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Details(int id, CategoryViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _categoryService.Delete(_mapper.Map<Category>(collection));
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

        // GET: Category/Create
        public ActionResult Create()
        {
            var viewModel = new CategoryViewModel { CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category())) };
            return View(viewModel);
        }

        // POST: Category/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(CategoryViewModel collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if (collection.ParentId.HasValue && _categoryService.HasProvider(collection.ParentId.Value))
                    //{
                    //    ModelState.AddModelError(string.Empty, "والد انتخابی برای این دسته بندی شامل ویژگی می باشد. شما نمیتوانید از والد انتخابی استفاده نمایید.");
                    //}
                    //else
                    //{
                    //}
                    var model = _mapper.Map<Category>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    _categoryService.Insert(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
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
                    if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
                }
            }
            collection.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category()));
            return View(collection);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var viewModel = _mapper.Map<CategoryViewModel>(_categoryService.GetById(id));
            viewModel.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category()));
            return View(viewModel);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, CategoryViewModel collection)
        {
            try
            {
                if (id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        if (collection.Id == collection.ParentId)
                        {
                            ModelState.AddModelError(string.Empty, "شما نمیتوانید دسته بندی مورد نظر را بعنوان والد خود انتخاب کنید.");
                        }
                        //else if (collection.ParentId.HasValue && _categoryService.HasProvider(collection.ParentId.Value))
                        //{
                        //    ModelState.AddModelError(string.Empty, "والد انتخابی برای این دسته بندی شامل ویژگی می باشد. شما نمیتوانید از والد انتخابی استفاده نمایید.");
                        //}
                        else
                        {
                            var model = _mapper.Map<Category>(collection);
                            model.UpdaterId = LogedInAdmin.Id;
                            _categoryService.Update(model);
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, GeneralMessages.DefectiveEntry);
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
            collection.CategoryTree = _mapper.Map<List<TreeModel>>(_categoryService.Find(new Category()));
            return View(collection);
        }
    }
}