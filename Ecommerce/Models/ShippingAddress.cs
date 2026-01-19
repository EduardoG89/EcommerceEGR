namespace Ecommerce.Models
{
    public class ShippingAddress
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? OrderId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string AdditionalInfo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }

        public User User { get; set; }
    }
}
