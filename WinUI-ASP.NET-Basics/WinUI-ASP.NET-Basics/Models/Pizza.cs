namespace WinUI_ASP.NET_Basics.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public List<PizzaToppings> PizzaToppings { get; set; } = new();
    }
}
