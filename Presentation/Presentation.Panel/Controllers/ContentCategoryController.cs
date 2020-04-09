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
    public class ContentCategoryController : BaseController
    {
        #region Constructor

        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IContentCategoryService _contentCategoryService;

        public ContentCategoryController(IContentCategoryService contentCategoryService, IMapper mapper, ILog4Net logger)
        {
            _contentCategoryService = contentCategoryService;
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
                ViewBag.SearchModel = new ContentCategoryViewModels();
                var request = new FilteredModel<ContentCategory>();
                var offset = (request.PageIndex - 1) * request.PageSize;
                var result = _mapper.Map<IList<ContentCategoryViewModels>>(_contentCategoryService.GetPaging(new ContentCategory(), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<ContentCategoryViewModels>(result, request.PageIndex, request.PageSize, (int) totalCount);
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
        public ActionResult Index(ContentCategoryViewModels collection)
        {
            var request = new FilteredModel<ContentCategory>
            {
                PageIndex = collection.ThisPageIndex,
                Order = collection.PageOrder,
                OrderBy = collection.PageOrderBy
            };
            var offset = (request.PageIndex - 1) * request.PageSize;
            var result = _mapper.Map<IList<ContentCategoryViewModels>>(_contentCategoryService.GetPaging(_mapper.Map<ContentCategory>(collection), out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int) (totalCount/request.PageSize);
                if (totalCount%request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<ContentCategoryViewModels>>(_contentCategoryService.GetPaging(_mapper.Map<ContentCategory>(collection), out totalCount, request.OrderBy, request.Order, offset, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<ContentCategoryViewModels>(result, request.PageIndex, request.PageSize, (int) totalCount);
            ViewBag.SearchModel = collection;
            return View();
        }

        // GET: Category/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_mapper.Map<ContentCategoryViewModels>(_contentCategoryService.GetById(id)));
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
        public ActionResult Details(int id, ContentCategoryViewModels collection)
        {
            try
            {
                if (id > 0)
                {
                    collection.UpdaterId = LogedInAdmin.Id;
                    _contentCategoryService.Delete(_mapper.Map<ContentCategory>(collection));
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
            var viewModel = new ContentCategoryViewModels { CategoryTree = _mapper.Map<List<TreeModel>>(_contentCategoryService.Find(new ContentCategory())) };
            return View(viewModel);
        }

        // POST: Category/Create
        [HttpPost]
        [SecurityFilter]
        public ActionResult Create(ContentCategoryViewModels collection)
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
                    var model = _mapper.Map<ContentCategory>(collection);
                    model.CreatorId = LogedInAdmin.Id;
                    _contentCategoryService.Insert(model);
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
            collection.CategoryTree = _mapper.Map<List<TreeModel>>(_contentCategoryService.Find(new ContentCategory()));
            return View(collection);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var viewModel = _mapper.Map<ContentCategoryViewModels>(_contentCategoryService.GetById(id));
            viewModel.CategoryTree = _mapper.Map<List<TreeModel>>(_contentCategoryService.Find(new ContentCategory()));
            return View(viewModel);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [SecurityFilter]
        public ActionResult Edit(int id, ContentCategoryViewModels collection)
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
                            var model = _mapper.Map<ContentCategory>(collection);
                            model.UpdaterId = LogedInAdmin.Id;
                            _contentCategoryService.Update(model);
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
            collection.CategoryTree = _mapper.Map<List<TreeModel>>(_contentCategoryService.Find(new ContentCategory()));
            return View(collection);
        }
    }
}