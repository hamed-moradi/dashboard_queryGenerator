using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Application;
using Asset.Infrastructure._App;
using AutoMapper;
using Presentation.Panel.Models;
using System.Collections.Generic;
using Domain.Model.Entities;

namespace Tests.Logic
{
    [TestClass]
    public class Geography
    {
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;
        public Geography()
        {
            _discountService = ServiceLocatorAdapter.Current.GetInstance<IDiscountService>();
            _mapper = ServiceLocatorAdapter.Current.GetInstance<IMapper>();
        }
        [TestMethod]
        public void InsertDbGeography()
        {
            var model = new Discount { Title = "title", CreatorId = 1113 };
            //model.Location = DbGeography.PointFromText("point(35.6891975 51.3889736)", 4326);
            var result = _discountService.Insert(model);
            //var viewModel = _mapper.Map<List<DiscountViewModel>>(model);
        }
        [TestMethod]
        public void GetDbGeography()
        {
            var result = _discountService.GetPaging(new Discount(), out long totalCount);
            var model = _discountService.GetById(1);
            var viewModel = _mapper.Map<List<DiscountViewModel>>(model);
        }
    }
}