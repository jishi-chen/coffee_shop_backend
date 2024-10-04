using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using CoffeeShop.Utility.Helpers;

namespace CoffeeShop.Model.ViewModels
{
    public class UserRegisterViewModel : IValidatableObject
    {
        public int? UserId { get; set; }
        public int TenantId { get; set; }
        [Required(ErrorMessage = "請輸入姓名")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入電子信箱")]
        public string Email { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();


        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "確認密碼與密碼必須相同")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public List<SelectListItem> GenderOption { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem("男", "0"){ Selected = true },
            new SelectListItem("女", "1"),
            new SelectListItem("其他", "2"),
        };
        public bool IsEnabled { get; set; }
        public string? Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var utility = new DataCheckUtility();
            if (!string.IsNullOrEmpty(Password) && !utility.IsAlphaNumeric(Password))
            {
                yield return new ValidationResult("密碼格式錯誤", new[] { "Password" });
            }

            if (!utility.IsEmail(Email))
            {
                yield return new ValidationResult("信箱格式錯誤", new[] { "Email" });
            }
        }
    }

    public class Address
    {
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? AddressField { get; set; }
    }
}
