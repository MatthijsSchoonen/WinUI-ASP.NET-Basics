using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WinUI_Basics.Models
{
    class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        [JsonPropertyName("toppings")]
        public List<Toppings> PizzaToppings { get; set; } = new();
    }
}
