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

namespace StoreFront.UI.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private Store_FrontEntities db = new Store_FrontEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Blend).Include(p => p.Category).Include(p => p.ProductStatus);
            return View(products.ToList());
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
        public ActionResult Create([Bind(Include = "ProductID,ProductName,UnitPrice,UnitsInStock,UnitsOnOrder,BlendID,Description,ProductStatusID,Image,CategoryID")] Product product, HttpPostedFileBase productPicture)
        {
            if (ModelState.IsValid)
            {
                #region File Upload
                string imgName = "noImage.png";
                
                if (productPicture != null)
                {
                    imgName = productPicture.FileName;
                    
                    string ext = imgName.Substring(imgName.LastIndexOf(".")); //ext.

                    string[] goodExts = { ".jpg", ".jpeg", ".gif", ".png" };
                    
                    if (goodExts.Contains(ext.ToLower()) && productPicture.ContentLength <= 4194304)
                    
                    {
                        imgName = Guid.NewGuid() + ext.ToLower();

                        string savePath = Server.MapPath("~/Content/imgPictures/Products/");
                        Image convertedImage = Image.FromStream(productPicture.InputStream);
                        int maxImageSize = 500;
                        int maxThumbSize = 100;

                        //call to imageService.ResizeImage
                        Images.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);
                    }
                    else
                    {
                        imgName = "NoImage.png";
                    }
                }
                //NO MATTER WHAT - add the imageName property of the book object to send to the DB.

                product.Image = imgName;
                #endregion
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlendID = new SelectList(db.Blends, "BlendID", "BlendName", product.BlendID);
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
