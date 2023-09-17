using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopping.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public string StripePaymentId { get; set; }
        [NotMapped]
        public string ClientSecret { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        [NotMapped]
        public double CartTotal { get; set; }
    }
}
