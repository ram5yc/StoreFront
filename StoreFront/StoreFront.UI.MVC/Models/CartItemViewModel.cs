using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreFront.DATA.EF;
using System.ComponentModel.DataAnnotations;

namespace StoreFront.UI.MVC.Models
{
    public class CartItemViewModel
    {
        [Range(1, byte.MaxValue, ErrorMessage = "* Please Enter a Quantity between 1 and 255.")]
        public int Qty { get; set; }
        public Product Product { get; set; }

        public CartItemViewModel (int qty, Product product)
        {
            Qty = qty;
            Product = product;
        }//end FQCTOR
    }//end class
}