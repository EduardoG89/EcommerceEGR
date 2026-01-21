namespace Ecommerce.DTOs.Orders
{
    public class PaymentDto
    {
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}
