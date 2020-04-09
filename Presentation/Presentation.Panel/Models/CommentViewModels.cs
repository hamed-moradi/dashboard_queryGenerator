using System.ComponentModel.DataAnnotations;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Models
{
    public class CommentViewModel : ViewModel
    {
        [Display(Name = "شناسه")]
        public int? Id { get; set; }

        [Display(Name = "والد")]
        public int? ReplyTo { get; set; }

        [Display(Name = "مدیر")]
        public string AdminCreator { get; set; }

        [Display(Name = "کاربر")]
        public int? UserCreatorId { get; set; }

        [Display(Name = "کاربر")]
        public string UserCreatorName { get; set; }

        [Display(Name = "شناسه هدف")]
        [Search]
        public int? CommentEntityId { get; set; }

        [Display(Name = "نوع")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public GeneralEnums.CommentEntityType? CommentEntityTypeId { get; set; }

        [Display(Name = "دیدگاه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد متن خالی است")]
        [Search]
        public string Body { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public new UserEditStatus? Status { get; set; }

        [Display(Name = "آی پی")]
        public string IP { get; set; }

        [Display(Name = "عنوان شناسه هدف")]
        public string CommentEntityTypeTitle { get; set; }
        

    }
}