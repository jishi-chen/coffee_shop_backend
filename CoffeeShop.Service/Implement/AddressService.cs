using CoffeeShop.Model.Entities;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Service.Implement
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<SelectListItem> GetAddressCityList(string? cityId)
        {
            List<SelectListItem> cityList = new List<SelectListItem>();
            IEnumerable<AddressCity> cityModel = _unitOfWork.AddressRepository.GetAddressCity();
            foreach (var c in cityModel)
            {
                cityList.Add(new SelectListItem()
                {
                    Text = c.CityName,
                    Value = c.Id.ToString(),
                    Selected = cityId == c.Id.ToString(),
                });
            }
            return cityList;
        }

        public List<SelectListItem> GetAddressAreaList(string? cityId, string? areaId)
        {
            List<SelectListItem> areaList = new List<SelectListItem>();
            IEnumerable<AddressArea> areaModel = _unitOfWork.AddressRepository.GetAddressAreaByCityId(int.TryParse(cityId, out int id) ? id : 0);
            foreach (var c in areaModel)
            {
                areaList.Add(new SelectListItem()
                {
                    Text = c.AreaName,
                    Value = c.Id.ToString(),
                    Selected = areaId == c.Id.ToString(),
                });
            }
            return areaList;
        }

        public AddressArea? GetAddressAreaById(int id)
        {
            return _unitOfWork.AddressRepository.GetById(id);
        }
    }
}
