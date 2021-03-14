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

        #region Ajax Operations

        #region Ajax Delete
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AjaxDelete(int id)
        {
            //get prod from db
            Product prod = db.Products.Find(id);
            //remove prod from ef
            db.Products.Remove(prod);

            //save changes
            db.SaveChanges();
            //create message to send to user
            var message = $"Deleted the following product from the database: {prod.ProductName}";

            //return jsonresult
            return Json(new
            {
                id = id,
                message = message
            });

        }
        #endregion
        #endregion

        #region Create
        //add the publisher to the db via ajax and return the result
        public JsonResult AjaxCreate(Product product)
        {
            //even though this is a json result the VIEW is a partial view
            //so that we can render it in the Index (our div that we created)

            //hard code that each publisher will b active( no checkbox in the form)

            db.Products.Add(product);
            db.SaveChanges();
            return Json(product);
        }
        //
        #endregion


        #region Ajax Edit [GET]
        public PartialViewResult ProductEdit(int id)
        {
            Product product = db.Products.Find(id);

            return PartialView(product);
        }

        #endregion

        #region Ajax Edit [POST]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(Product product)
        {
            db.Entry(product).State = EntityState.Modified; //how it updates with our changes
            db.SaveChanges();
            return Json(product);
        }
        #endregion


        #region AddToCart
        [HttpPost]
        public ActionResult AddToCart(int qty, int productID)
        {
            //create empty version of LOCAL shopping cart
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //check the cart in session(global)
            //if cart has stuff in it, assign its value to local dictionary
            if (Session["cart"] != null)
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

        public ActionResult Tiles()
        {
            return View(db.Products.ToList());
        }

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


        public ActionResult Create([Bind(Include = "ProductID,ProductName,UnitPrice,UnitsInStock,UnitsOnOrder,BlendID,Description,ProductStatusID,Image,CategoryID")] Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                #region File Upload
                string imgName = "no image.png";

                if (image != null)
                {
                    imgName = image.FileName;

                    string ext = imgName.Substring(imgName.LastIndexOf(".")); //ext.

                    string[] goodExts = { ".jpg", ".jpeg", ".gif", ".png" };

                    if (goodExts.Contains(ext.ToLower()) && (image.ContentLength <= 4194304))

                    {
                        imgName = Guid.NewGuid() + ext.ToLower();

                        string savePath = Server.MapPath("~/Content/img/productImage/");

                        Image convertedImage = Image.FromStream(image.InputStream); //from Image.cs in Utilities folder

                        int maxImageSize = 500;
                        int maxThumbSize = 100;

                        //call to imageService.ResizeImage
                        Images.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);
                    }
                    else
                    {
                        imgName = "no image.png";
                    }
                }
                //NO MATTER WHAT - add the imageName property of the book object to send to the DB.

                product.Image = imgName;
                #endregion
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Blends, "ProductID", "ProductName", product.ProductID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "StatusName", product.ProductStatusID);
            return View(product);
        }

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
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,UnitPrice,UnitsInStock,UnitsOnOrder,BlendID,Description,ProductStatusID,Image,CategoryID")] Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                #region File Upload
                if (image != null)
                {
                    string imgName = image.FileName;

                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    string[] goodExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    if (goodExts.Contains(ext.ToLower()) && (image.ContentLength <= 4194304)) //4mb max by ASP.NET
                    {
                        imgName = Guid.NewGuid() + ext;
                        string savePath = Server.MapPath("~/Content/img/productImage/");

                        Image convertedImage = Image.FromStream(image.InputStream);
                        int maxImageSize = 500;
                        int maxThumbSize = 100;

                        Images.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);

                        if (product.Image != null && product.Image != "no image.png")
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/img/productImage/" + Session["currentImage"].ToString()));
                        }
                        product.Image = imgName;
                    }
                }
                #endregion
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
            if (product.Image != null && product.Image != "no image.png")
            {
                System.IO.File.Delete(Server.MapPath("~/Content/img/productImage/" + Session["currentImage"].ToString()));
            }
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
