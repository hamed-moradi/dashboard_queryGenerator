using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.Model.Entities;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Models
{
    public class ContentViewModel : ViewModel
    {
        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "خلاصه")]
        [Search]
        public string Summary { get; set; }

        [Display(Name = "متن اصلی")]
        [Search]
        public string Original { get; set; }

        [Display(Name = "متن")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [Search]
        public string Body { get; set; }

        [Display(Name = "قیمت")]
        [Search]
        public int? Price { get; set; }

        [Display(Name = "اولویت")]
        [Search]
        public byte? Priority { get; set; }

        [Display(Name = "شروع نمایش")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string StartShowDate { get; set; }

        [Display(Name = "پایان نمایش")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string EndShowDate { get; set; }

        [Display(Name = "تعداد نظرات")]
        [Search(GeneralEnums.SearchFieldType.Number)]
        public int? CommentCount { get; set; }

        [Display(Name = "تعداد لایک")]
        [Search(GeneralEnums.SearchFieldType.Number)]
        public int? LikeCount { get; set; }

        [Display(Name = "نمی پسندم")]
        [Search(GeneralEnums.SearchFieldType.Number)]
        public int? DisLikeCount { get; set; }

        [Display(Name = "تعداد بازدید")]
        [Search(GeneralEnums.SearchFieldType.Number)]
        public int? ViewCount { get; set; }

        [Display(Name = "دسته بندی")]
        public string[] CategoryId { get; set; }

        [Search]
        [Display(Name = "دسته بندی")]
        public string CategoryTitle { get; set; }

        [Display(Name = "نوع ")]
        public int? ContentTypeId { get; set; }

        //[Display(Name = "برچسب ها")]
        //public List<Content2Tag> Tags { get; set; }

        //[Display(Name = "مکان ها")]
        //public List<Content2Position> Positions { get; set; }

        [Display(Name = "نمایه")]
        public string Thumbnail { get; set; }

        [Display(Name = "عکس")]
        public string Photo { get; set; }
    }
}