using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreFront.DATA.EF;
using StoreFront.UI.MVC.Utilities;
using PagedList;
using PagedList.Mvc;
using StoreFront.UI.MVC.Models; //for the add to cart method
using System.Collections.Generic;

namespace StoreFront.UI.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private Store_FrontEntities db = new Store_FrontEntities();

        #region AddToCart
        [HttpPost]
        public ActionResult AddToCart(int qty, int productID)
        {
            //create empty version of LOCAL shopping cart
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //check the cart in session(global)
            //if cart has stuff in it, assign its value to local dictionary
            if(Session["cart"] != null)
            {
                //put global into local version
                shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];
            }
            //if global version is empty
            else
            {
                //create instance of local dictionary
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            //get the product object being adding - firstOfDeault() alloes for null return
            Product product = db.Products.Where(p => p.ProductID == productID).FirstOrDefault();

            //if product is null - redirec to the index view
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            //product is valid
            else
            {
                //create shoppingCartViewModel object
                CartItemViewModel item = new CartItemViewModel(qty, product);
                //if that productID already exists in the shopping cart, add to qty
                if (shoppingCart.ContainsKey(product.ProductID))
                {
                    shoppingCart[product.ProductID].Qty += qty;
                }
                //if it is not in the cart - add it
                else
                {
                    shoppingCart.Add(product.ProductID, item);
                }
                //update the global cart(session)
                Session["cart"] = shoppingCart;
            }
            //as long as the product was added redirect user to the shoppingCart Index
            return RedirectToAction("Index", "ShoppingCart");
        }
        #endregion

        //GET: Products
        public ActionResult Index(string searchCategory, int page = 1)
        {
            int pageSize = 5;

            //retrieve all products and order by name
            var products = db.Products.OrderBy(p => p.Category.CategoryName).ToList();

            #region Search Logic
            if (!string.IsNullOrEmpty(searchCategory))
            {
                //products = products.Where(p => p.Category == (searchCategory.ToLower()).ToList()); //method syntax
                products = products.Where(p => p.Category.CategoryName.ToLower().Contains(searchCategory.ToLower())).ToList();
            }

            ViewBag.SearchCategory = searchCategory;
            #endregion

            //return using pagedlistmvc and the page number and size
            return View(products.ToPagedList(page, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.BlendID = new SelectList(db.Blends, "BlendID", "BlendName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "StatusName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


        //public ActionResult Create([Bind(Include = "ProductID,ProductName,UnitPrice,UnitsInStock,UnitsOnOrder,BlendID,Description,ProductStatusID,Image,CategoryID")] Product product, HttpPostedFileBase productPicture)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        #region File Upload
        //        string imgName = "noImage.png";
                
        //        if (productPicture != null)
        //        {
        //            imgName = productPicture.FileName;
                    
        //            string ext = imgName.Substring(imgName.LastIndexOf(".")); //ext.

        //            string[] goodExts = { ".jpg", ".jpeg", ".gif", ".png" };
                    
        //            if (goodExts.Contains(ext.ToLower()) && productPicture.ContentLength <= 4194304)
                    
        //            {
        //                imgName = Guid.NewGuid() + ext.ToLower();

        //                string savePath = Server.MapPath("~/Content/imgPictures/Products/");
        //                Image convertedImage = Image.FromStream(productPicture.InputStream);
        //                int maxImageSize = 500;
        //                int maxThumbSize = 100;

        //                //call to imageService.ResizeImage
        //                Images.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);
        //            }
        //            else
        //            {
        //                imgName = "NoImage.png";
        //            }
        //        }
        //        //NO MATTER WHAT - add the imageName property of the book object to send to the DB.

        //        product.Image = imgName;
        //        #endregion
        //        db.Products.Add(product);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.BlendID = new SelectList(db.Blends, "BlendID", "BlendName", product.BlendID);
        //    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
        //    ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "StatusName", product.ProductStatusID);
        //    return View(product);
        //}

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlendID = new SelectList(db.Blends, "BlendID", "BlendName", product.BlendID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "StatusName", product.ProductStatusID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,UnitPrice,UnitsInStock,UnitsOnOrder,BlendID,Description,ProductStatusID,Image,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlendID = new SelectList(db.Blends, "BlendID", "BlendName", product.BlendID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "StatusName", product.ProductStatusID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
