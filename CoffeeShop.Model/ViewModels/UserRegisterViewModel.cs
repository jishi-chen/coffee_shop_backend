using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Model.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public List<SelectListItem> GenderOption { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem("男", "0"){ Selected = true },
            new SelectListItem("女", "1"),
            new SelectListItem("其他", "2"),
        };
        public string? Description { get; set; }
    }

    public class Address
    {
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? AddressField { get; set; }
    }
}
