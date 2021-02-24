using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreFront.DATA.EF;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;

namespace StoreFront.UI.MVC.Controllers
{
    public class FiltersController : Controller
    {
        private Store_FrontEntities ctx = new Store_FrontEntities();
        // GET: Filters
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ClientSide()
        {
            var products = ctx.Products.Include(p => p.ProductName)
                                       .Include(p => p.UnitPrice)
                                       .Include(p => p.Image)
                                       .Include(p => p.Category);
            return View(products.ToList());
        }
        public ActionResult ProductsQS(string searchFilter)
        {
            var products = ctx.Products;
            if (string.IsNullOrEmpty(searchFilter))
            {
                return View(products.ToList());
            }
            else
            {
                var filteredProducts = products.Where(p => p.Category.ToLower().Contains(searchFilter.ToLower()));

                return View(filteredProducts);
            }
        }
        /////////////////PAGING////////////////////
        public ActionResult ProductsMVCPaging(string searchCategory, int page = 1)
        {
            int pageSize = 5;

            //retrieve all products and order by name
            var products = ctx.Products.OrderBy(p => p.Category).ToList();

            #region Search Logic
            if (!string.IsNullOrEmpty(searchCategory))
            {
                products = products.Where(p => p.Category.ToLower().Contains(searchCategory.ToLower())).ToList(); //method syntax
            }

            ViewBag.SearchCategory = searchCategory;
            #endregion

            //return using pagedlistmvc and the page number and size
            return View(products.ToPagedList(page, pageSize));
        }
    }
}