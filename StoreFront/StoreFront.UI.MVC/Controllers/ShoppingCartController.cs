using StoreFront.UI.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreFront.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            //create local version of shopping cart from session (global)
            //if value is null or count is 0 create an empty instance and provide no cart items verbiage

            var shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];


            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
                ViewBag.Message = "There are no products in your cart";
            }
            //if cart isn't null and count > 0 null the messaging
            else
            {
                ViewBag.Message = null;
            }
            return View(shoppingCart);//make sure to Shopping Cart 
        }
        public ActionResult UpdateCart(int productID, int qty)
        {
            //if they zero out the qty from the update, remove
            //from the cart
            if (qty == 0)
            {
                RemoveFromCart(productID);
                return RedirectToAction("Index");
            }
            //retrieve the cart from session and assign it to the local dictionary
            Dictionary<int, CartItemViewModel> shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            //update the qty in the LOCAL storage
            shoppingCart[productID].Qty = qty;

            //return the LOCAL cart to session (GLobal)
            Session["cart"] = shoppingCart;

            //logic to display a message if they update to NO items in their cart
            if (shoppingCart.Count == 0)
            {
                ViewBag.Message = "There are no products in your cart";
            }
            //redirect to the Index -  We need the logic in the index ACTION to run so just returning the view will not work
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFromCart(int id)
        {
            //cart out of sessino and into the local
            Dictionary<int, CartItemViewModel> shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            //call the remove() from the dictionary class
            shoppingCart.Remove(id);

            //null the session to hide the cart link when session is empty
            if (shoppingCart.Count == 0)
            {
                Session["cart"] = null;
            }

            //redirect back to the index action (running all of the code and repopulating the table or displaying the count message)
            return RedirectToAction("Index");

        }
    }
}