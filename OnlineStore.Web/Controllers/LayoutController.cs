using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlineStore.Web.OrderService;
using OnlineStore.Web.ProductService;
using ProductDto = OnlineStore.Web.ProductService.ProductDto;
// ReSharper disable PossibleNullReferenceException

namespace OnlineStore.Web.Controllers
{
    public class LayoutController : Controller
    {
        protected Guid UserId
        {
            get
            {
                if (Session["UserId"] != null)
                {
                    return (Guid)Session["UserId"];
                }
                else
                {
                    var id = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                    Session["UserId"] = id;
                    return id;
                }
            }
        }

        #region Shared Layout Partial view Actions

        public ActionResult _LoginPartial()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (var proxy = new OrderServiceClient())
                {
                    ViewBag.ShoppingCartItems = proxy.GetShoppingCartItemCount(UserId);
                }
            }
            return PartialView();
        }

        public ActionResult CategoriesPartial()
        {
            using (var proxy = new ProductServiceClient())
            {
                var categories = proxy.GetCategories();
                return PartialView(categories);
            }
        }

        public ActionResult NewProductsPartial()
        {
            using (var proxy = new ProductServiceClient())
            {
                var newProducts = proxy.GetNewProducts(4);
                return PartialView(newProducts);
            }
        }

        public ActionResult ProductsPartial(string categoryId = null)
        {
            using (var proxy = new ProductServiceClient())
            {
                IEnumerable<ProductDto> products = null;
                products = string.IsNullOrEmpty((categoryId)) ? proxy.GetProducts() : proxy.GetProductsForCategory(new Guid(categoryId));
                if (string.IsNullOrEmpty(categoryId))
                    ViewBag.CategoryName = "所有商品";
                else
                {
                    var category = proxy.GetCategoryById(new Guid(categoryId));
                    ViewBag.CategoryName = category.Name;
                }

                ViewBag.CategoryID = categoryId;
                return PartialView(products);
            }
        }

        #endregion 
    }
}
