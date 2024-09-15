using VillaBooking.Domain.Entities;

namespace VillaBooking.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? VillaList { get; set; }
    }
}
