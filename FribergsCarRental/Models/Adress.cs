namespace FribergsCarRental.Models
{
    public class Adress
    {
        public int AdressId { get; set; }
        public string Street { get; set; } = "";
        public string StreetNumber { get; set; } = "";
        public string ZipCode { get; set; } = "";
        public string City { get; set; } = "";
    }
}
