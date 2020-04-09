using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Application;
using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Panel.Controllers
{
    public class GeoController : BaseController
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IDropDownService _dropDownService;

        public GeoController(IDropDownService dropDownService, IMapper mapper, ILog4Net logger)
        {
            _dropDownService = dropDownService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        [HttpGet]
        [AllowAnonymous]
        public JsonResult DropDown(string key, int? p)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length < 3) return null;
            var result = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetCities(key, (p.GetValueOrDefault(1) - 1) * 30, 30, out long totalCount));
            return Json(new DropDownModel { TotalCount = totalCount, Items = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetDistricts(int cityId)
        {
            var response = new JsonViewModel { status = HttpStatusCode.InternalServerError, message = GeneralMessages.Error };
            try
            {
                response.data = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetDistricts(cityId));
                response.status = HttpStatusCode.OK;
                response.message = GeneralMessages.Ok;
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}