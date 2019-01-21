using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitFlexApparel.Models
{
    public class ProductsModel
    {
        public int Id { get; set; }
        public Product ProductObj { get; set; }
        public List<ProductPrice> Prices { get; set; }
    }
}