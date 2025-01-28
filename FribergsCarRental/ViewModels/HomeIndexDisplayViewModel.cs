using FribergsCarRental.Models;

namespace FribergsCarRental.ViewModels
{
    public class HomeIndexDisplayViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public List<string> Pictures { get; set; }
    }
}
