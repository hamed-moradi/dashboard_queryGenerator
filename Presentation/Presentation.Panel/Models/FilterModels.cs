using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Panel.Models
{
    public class BaseFilter
    {
        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;

        public string OrderBy { get; set; } = "Id";

        public string Order { get; set; } = "Desc";
    }

    public class FilteredModel<T> : BaseFilter where T : new()
    {
        public T FilterdModel { get; set; } = new T();
    }
}