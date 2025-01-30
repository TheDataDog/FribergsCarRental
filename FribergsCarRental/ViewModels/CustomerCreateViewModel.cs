using FribergsCarRental.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace FribergsCarRental.ViewModels
{
    public class CustomerCreateViewModel
    {
        public Customer Customer { get; set; }
        public string ReturnUrl { get; set; }
    }
}
