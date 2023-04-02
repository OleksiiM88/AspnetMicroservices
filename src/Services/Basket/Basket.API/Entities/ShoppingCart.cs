namespace Basket.API.Entities
{
	public class ShoppingCart
	{
        public ShoppingCart()
        {
            
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
            Items = new List<ShoppingCartItem>();
        }
        public string UserName { get; set; }
		public List<ShoppingCartItem> Items { get; set; }

        public decimal TotalPrice
        {
            get 
            { 
                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Price * item.Quantity;
                }
                return total;
            }

        }

    }
}
