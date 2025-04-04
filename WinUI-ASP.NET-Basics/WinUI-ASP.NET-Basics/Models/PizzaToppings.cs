namespace WinUI_ASP.NET_Basics.Models
{
    public class PizzaToppings
    {
        public int Id { get; set; }
        public int PizzaId { get; set; }
        public Pizza? Pizza { get; set; }
        public int ToppingId { get; set; }
        public Topping? Topping { get; set; }
    }
}
