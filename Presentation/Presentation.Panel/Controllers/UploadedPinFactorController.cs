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
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class UploadedPinFactorController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IUploadedPinFactorService _uploadedPinFactorService;
        private readonly IPreUploadedPinFactorItemService _preUploadedPinFactorItemService;
        public UploadedPinFactorController(IUploadedPinFactorService uploadedPinFactorService, IMapper mapper, IPreUploadedPinFactorItemService preUploadedPinFactorItemService, ILog4Net logger)
        {
            _uploadedPinFactorService = uploadedPinFactorService;
            _mapper = mapper;
            _preUploadedPinFactorItemService = preUploadedPinFactorItemService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.SearchModel = new UploadedPinFactorViewModel();
                var request = new FilteredModel<UploadedPinFactor>();
                var result = _mapper.Map<IList<UploadedPinFactorViewModel>>(_uploadedPinFactorService.GetPaging(new UploadedPinFactor(), out long totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
                ViewBag.OnePageOfEntries = new StaticPagedList<UploadedPinFactorViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(UploadedPinFactorViewModel model)
        {
            long totalCount;
            model.Id = null;
            ViewBag.SearchModel = model;
            var request = new FilteredModel<UploadedPinFactorViewModel>
            {
                PageIndex = model.ThisPageIndex,
                Order = model.PageOrder,
                OrderBy = model.PageOrderBy
            };
            var searchModel = _mapper.Map<UploadedPinFactor>(model);
            var result = _mapper.Map<IList<UploadedPinFactorViewModel>>(_uploadedPinFactorService.GetPaging(searchModel, out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            if (!result.Any() && totalCount > 0 && request.PageIndex > 1)
            {
                request.PageIndex = (int)(totalCount / request.PageSize);
                if (totalCount % request.PageSize > 0)
                {
                    request.PageIndex++;
                }
                result = _mapper.Map<IList<UploadedPinFactorViewModel>>(_uploadedPinFactorService.GetPaging(_mapper.Map<UploadedPinFactor>(model), out totalCount, request.OrderBy, request.Order, (request.PageIndex - 1) * request.PageSize, request.PageSize));
            }
            ViewBag.OnePageOfEntries = new StaticPagedList<UploadedPinFactorViewModel>(result, request.PageIndex, request.PageSize, (int)totalCount);
            return View();
        }

        [HttpGet]
        public ActionResult UploadFactor()
        {
            try
            {
                return View(new UploadedPinFactorViewModel()
                {
                    InputTypeId = GeneralEnums.PinInputType.File,
                    Status = GeneralEnums.PinFactorStatus.New,
                    Description = string.Empty
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        [HttpPost]
        [SecurityFilter]
        public ActionResult UploadFactor(UploadedPinFactorViewModel viewModel)
        {
            try
            {
                var result = UploadFactorByFile(viewModel);
                if (result.Length > 0)
                {
                    ModelState.AddModelError(string.Empty, result.ToString());
                }
                return View(viewModel);
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
            return View(viewModel);
        }
        private string UploadFactorByFile(UploadedPinFactorViewModel viewModel)
        {
            var pinFile = viewModel.File;
            if (pinFile == null)
            {
                return "فایلی انتخاب نشده است";
            }
            if (pinFile.ContentLength <= 0)
            {
                return "فایلی انتخابی فاقد محتوا است";
            }
            var isExistUploadedPinFactor = _uploadedPinFactorService.GetPaging(new UploadedPinFactor { Filename = pinFile.FileName, Status = (byte)GeneralEnums.PinFactorStatus.Cancelled }, out long total).Any();
            if (isExistUploadedPinFactor)
            {
                return "فایلی با این نام قبلا ثبت شده";
            }
            var pinTypeValue = 0;
            var fetchedResult = new Dictionary<string, string>();
            using (var streamReader = new StreamReader(pinFile.InputStream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var isNumber = int.TryParse(line.Substring(0, 2), out int junk);
                    if (line.Contains("PackageId:"))
                    {
                        var factorNo = line.Substring(10);
                        viewModel.Title = factorNo;
                        viewModel.FactorNo = factorNo;
                    }
                    else if (line.Contains("Quantity:"))
                    {
                        viewModel.Quantity = int.Parse(line.Substring(9));
                    }
                    else if (line.Contains("FaceValue:"))
                    {
                        pinTypeValue = int.Parse(line.Substring(10));
                        switch (pinTypeValue)
                        {
                            case 10000:
                                viewModel.PinTypeId = GeneralEnums.PinType._10_000;
                                break;
                            case 20000:
                                viewModel.PinTypeId = GeneralEnums.PinType._20_000;
                                break;
                            case 50000:
                                viewModel.PinTypeId = GeneralEnums.PinType._50_000;
                                break;
                            case 100000:
                                viewModel.PinTypeId = GeneralEnums.PinType._100_000;
                                break;
                            case 200000:
                                viewModel.PinTypeId = GeneralEnums.PinType._200_000;
                                break;
                        }
                    }
                    else if (line.Contains("CardPrefix:"))
                    {
                        viewModel.OpTypeId = GeneralEnums.OperatorType.MCI;
                    }
                    else if (isNumber)
                    {
                        var codes = line.Split(' ');
                        fetchedResult.Add(codes[0], codes[1]);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(viewModel.Title) ||
                !viewModel.Quantity.HasValue ||
                !viewModel.PinTypeId.HasValue ||
                !viewModel.OpTypeId.HasValue ||
                !fetchedResult.Any())
            {
                return  "فایل انتخابی صحیح نمی باشد چون فاقد اطلاعات لازم است.";
            }

            var duplicates = _preUploadedPinFactorItemService.CheckDuplicatedPins(fetchedResult.Keys.ToArray()).Select(s => s.SerialNo);
            if (duplicates.Any())
            {
                fetchedResult = fetchedResult.Where(w => !duplicates.Contains(w.Key)).ToDictionary(d => d.Key, d => d.Value);
            }
            if (!fetchedResult.Any())
            {
                return "تمام پین ها تکراری هستند";
            }

            viewModel.Price = viewModel.Quantity * pinTypeValue;
            viewModel.Filename = pinFile.FileName;
            viewModel.Status = GeneralEnums.PinFactorStatus.New;
            viewModel.Description = string.Empty;
            viewModel.CreatorId = LogedInAdmin.Id;
            var model = _mapper.Map<UploadedPinFactor>(viewModel);
            var preUploadedPinFactorItems = new List<PreUploadedPinFactorItem>();
            var now = DateTime.Now;
            foreach (var item in fetchedResult)
            {
                preUploadedPinFactorItems.Add(new PreUploadedPinFactorItem
                {
                    SerialNo = item.Key,
                    PinCode = Encryption.Encrypt(item.Value),
                    //FactorId = res,
                    OpTypeId = (byte)viewModel.OpTypeId,
                    PinTypeId = (byte)viewModel.PinTypeId,
                    CreatedAt = now
                });
            }

            return _uploadedPinFactorService.CreateFactor(model, preUploadedPinFactorItems);

            //var res = _uploadedPinFactorService.Insert(model);
            //if (res <= 0)
            //{
            //    builder.Append("فاکتور ثبت نشد");
            //    return builder;
            //}
            //model.ReferenceNumber = model.Id = res;

            

            //var changeset = _preUploadedPinFactorItemService.BulkInsert(preUploadedPinFactorItems);
            //if (changeset > 0)
            //{
            //    model.Status = (byte)GeneralEnums.PinFactorStatus.New;
            //    _uploadedPinFactorService.Update(model);
            //    builder.Append($"تعداد {fetchedResult.Count} از {fetchedResult.Count} شارژ با موفقیت ثبت شدند.");
            //    return builder;
            //}
            //model.Status = (byte)GeneralEnums.PinFactorStatus.Cancelled;
            //_uploadedPinFactorService.Update(model);
            //foreach (var item in preUploadedPinFactorItems)
            //{
            //    _logger.Info(
            //        $"SerialNo : {item.SerialNo} ,PinCode : {item.PinCode}, FactorID : {item.FactorId}, OpTypeID : {item.OpTypeId} , PinTypeID : {item.PinTypeId}",
            //        "Unsaved_PreUploadedPinFactorItems");
            //}

            //var msg = "شارژها با موفقیت دریافت شد ولی دیتابیس ثبت نشد و نتایج لاگ شد.";
            //_logger.Error(msg);
            //builder.Append(msg);
            //return builder;
        }

        [HttpPost]
        [SecurityFilter]
        public ActionResult Approve(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Json(new { Status = 0, Message = GeneralMessages.EmptyId });
                }
                if (_uploadedPinFactorService.ApproveFactor(id))
                {
                    return Json(new { Status = 1, Message = $"فاکتور انتخابی تائید شد." });
                }
                return Json(new { Status = 0, Message = GeneralMessages.ErrorInSave });

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new { Status = 0, Message = GeneralMessages.UnexpectedError });
            }
        }
        [HttpPost]
        [SecurityFilter]
        public ActionResult Invalidate(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Json(new { Status = 0, Message = GeneralMessages.EmptyId });
                }
                if (_uploadedPinFactorService.InvalidateFactor(id))
                {
                    return Json(new { Status = 1, Message = $"فاکتور انتخابی نامعتبر شد." });
                }
                return Json(new { Status = 0, Message = GeneralMessages.ErrorInSave });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new { Status = 0, Message = GeneralMessages.UnexpectedError });
            }
        }
    }
}