using Domain.Model.Entities;
using Domain.Application;
using Microsoft.SqlServer.Types;
using System.Data.OleDb;
using System.Web.Mvc;
using Presentation.Panel.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System;
using Asset.Infrastructure.Common;
using Asset.Infrastructure._App;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Dapper;
using System.Data;
using Domain.Application.Repository;
using Domain.Application._App;
using PagedList;

namespace Presentation.Panel.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILog4Net _logger;
        private readonly IUserService _userService;

        public HomeController(IMapper mapper, ILog4Net logger, IUserService userService)
        {
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\Filtered.xls;Extended Properties='Excel 8.0;HDR=Yes;'";
            //using (OleDbConnection connection = new OleDbConnection(con))
            //{
            //    connection.Open();
            //    OleDbCommand command = new OleDbCommand("select * from [Sheet3$]", connection);
            //    using (OleDbDataReader dr = command.ExecuteReader())
            //    {
            //        while (dr.Read())
            //        {
            //            var lat = dr["Lat"];
            //            var lng = dr["Lng"];
            //            double decLat = 0;
            //            double decLng = 0;
            //            if (lat != null && lng != null)
            //            {
            //                double.TryParse(lat.ToString(), out decLat);
            //                double.TryParse(lng.ToString(), out decLng);
            //            }
            //            var cat = dr["CategoryId"];
            //            int catId = 1;
            //            if (cat != null)
            //            {
            //                int.TryParse(cat.ToString(), out catId);
            //            }
            //            try
            //            {
            //                _providerService.Insert(new Provider
            //                {
            //                    Title = dr["Title"].ToString(),
            //                    GeoTitle = dr["GeoTitle"].ToString(),
            //                    OwnerPersonId = 1,
            //                    PhoneNo = dr["Tel"].ToString(),
            //                    Address = dr["Address"].ToString(),
            //                    CategoryId = catId,
            //                    Location = SqlGeography.Point(decLat, decLng, 4326),
            //                    CreatorId = 1113,
            //                    Status = 1
            //                });
            //            }
            //            catch { }
            //        }
            //    }
            //}

            //var business = new Business();
            //business.Id = 4;
            //var parameters = new DynamicParameters();
            //parameters.Add(nameof(business.Id), business.Id, DbType.String, ParameterDirection.Input, 50);
            //var result = _repository.ExecuteScalar($"SELECT [dbo].[GetBusinessReservationStatus](@{nameof(business.Id)})AS Result", parameters);

            //try
            //{
            //    var result = _repository.Query($"SELECT * FROM [dbo].[Business] WHERE ((SELECT [dbo].[GetBusinessReservationStatus](Id)) = 1)");
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex);
            //}

            return View();
        }        

        //[HttpPost]
        //public ActionResult ChartData()
        //{
        //    try
        //    {
        //        var jData = new JObject();
                
        //        var business = _mapper.Map<List<Business>>(_businessService.Find(new Business()));
        //        var businessAtachment = _mapper.Map<List<BusinessAttachment>>(_businessAttachmentService.Find(new BusinessAttachment()));
                
        //        jData.Add("BusinessCount", business.Count());
        //        jData.Add("BusinessAtachmentCount", businessAtachment.Count());

        //        //Evidence Approved , Availability notAvailable
        //        jData.Add("BuisnessChartData1", business.Where(w => w.EvidenceStatusId == 1 && w.AvailabilityStatusId == 2).Count());
        //        //Evidence Unknown or Rejected , Availability Available
        //        jData.Add("BuisnessChartData2", business.Where(w => w.EvidenceStatusId == 0 || w.EvidenceStatusId == 2 && w.AvailabilityStatusId == 1).Count());
        //        //Status Deactive , Availability Available
        //        jData.Add("BuisnessChartData3", business.Where(w => w.Status == 0 && w.AvailabilityStatusId == 1).Count());

        //        jData.Add("LatestBusiness", 10);
        //        jData.Add("LatestEdivence", business.Where(w => w.EvidenceStatusId == 0).Count());
        //        jData.Add("AvailableBusiness", business.Where(w => w.AvailabilityStatusId == 1).Count());

        //        return Content(JsonConvert.SerializeObject(new { status = HttpStatusCode.OK, message = "", data = JsonConvert.SerializeObject(jData) }));

        //        //return Json(new JsonViewModel
        //        //{
        //        //    status = HttpStatusCode.OK,
        //        //    message = "",
        //        //    data = JsonConvert.SerializeObject(jData)
        //        //});

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex);
        //        if (ex.InnerException != null && ex.InnerException.Source.Equals(GeneralMessages.ExceptionSource)) ModelState.AddModelError(string.Empty, ex.Message); else ModelState.AddModelError(string.Empty, GeneralMessages.UnexpectedError);
        //        return Json(new JsonViewModel { status = HttpStatusCode.BadRequest, message = ex.Message });
        //    }
            
        //}

        //public ActionResult TopTenRequest()
        //{
        //    var request = new FilteredModel<ServiceRequest>();
        //    var offset = (request.PageIndex - 1) * request.PageSize;
        //    var result = _mapper.Map<IList<FacilityRequestViewModel>>(_facilityRequestService.GetPaging(new ServiceRequest { CustomerId = null }, out long totalCount, request.OrderBy, request.Order, offset, request.PageSize));
        //    return PartialView("_TopTenRequest", result);
        //}

        //public ActionResult TopTenChangeRequest()
        //{
        //    var request = new FilteredModel<Business> { };
        //    var offset = (request.PageIndex - 1) * request.PageSize;
        //    var result = _mapper.Map<IList<BusinessViewModel>>(_businessService.GetPaging(new Business { ProviderId = null,ChangeRequest=1 }, out long totalCount, request.OrderBy, request.Order, offset));

        //    return PartialView("_TopTenChangeRequest", result);
        //}

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Error404()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Error504()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageFiles()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}