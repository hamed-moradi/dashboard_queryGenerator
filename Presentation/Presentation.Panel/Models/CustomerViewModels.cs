using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class CustomerViewModel : UserViewModel
    {
        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public new UserEditStatus? Status { get; set; }

    }
}