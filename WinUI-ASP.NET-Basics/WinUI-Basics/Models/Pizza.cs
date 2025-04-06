using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WinUI_Basics.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => _price = Math.Round(value, 2);
        }
        public string? ImgUrl { get; set; }
        [JsonPropertyName("toppings")]
        public List<Toppings> PizzaToppings { get; set; } = new();
    }
}
