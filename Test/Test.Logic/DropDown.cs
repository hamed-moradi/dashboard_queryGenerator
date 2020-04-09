using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Application;
using Asset.Infrastructure._App;
using AutoMapper;
using Presentation.Panel.Models;
using System.Collections.Generic;

namespace Tests.Logic
{
    [TestClass]
    public class DropDown
    {
        private readonly IDropDownService _dropDownService;
        private readonly IMapper _mapper;
        public DropDown()
        {
            _dropDownService = ServiceLocatorAdapter.Current.GetInstance<IDropDownService>();
            _mapper = ServiceLocatorAdapter.Current.GetInstance<IMapper>();
        }
        [TestMethod]
        public void PersonDropDown()
        {
            var result = _mapper.Map<List<DropDownViewModel>>(_dropDownService.GetPersons("هاد", 0, 30, out long totalCount));
        }
    }
}
