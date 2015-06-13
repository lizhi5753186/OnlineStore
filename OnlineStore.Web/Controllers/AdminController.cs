using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Web.OrderService;
using OnlineStore.Web.ProductService;
using OnlineStore.Web.UserService;
using OnlineStore.Web.ViewModels;
using CategoryDto = OnlineStore.Web.ProductService.CategoryDto;
using ProductDto = OnlineStore.Web.ProductService.ProductDto;

namespace OnlineStore.Web.Controllers
{
    [HandleError]
    public class AdminController : ControllerBase
    {
        #region Common Utility Actions

        // 保存图片到服务器指定目录下
        [NonAction]
        private void SaveFile(HttpPostedFileBase postedFile, string filePath, string saveName)
        {
            string phyPath = Request.MapPath("~" + filePath);
            if (!Directory.Exists(phyPath))
            {
                Directory.CreateDirectory(phyPath);
            }
            try
            {
                postedFile.SaveAs(phyPath + saveName);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);

            }
        }

        // 图片上传功能的实现
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase fileData, string folder)
        {
            var result = string.Empty;
            if (fileData != null)
            {
                string ext = Path.GetExtension(fileData.FileName);
                result = Guid.NewGuid()+ ext;
                SaveFile(fileData, Url.Content("~/Images/Products/"), result);
            }
            return Content(result);
        }
        #endregion

        #region Administration
        [Authorize]
        public ActionResult Administration()
        {
            ViewBag.Message = "Please select the administration task below.";
            return View();
        }
        #endregion 

        #region Categories

        [Authorize]
        public ActionResult Categories()
        {
            using (var proxy = new ProductServiceClient())
            {
                var categories = proxy.GetCategories();
                return View(categories);
            }
        }

        public ActionResult EditCategory(string id)
        {
            using (var proxy = new ProductServiceClient())
            {
                var category = proxy.GetCategoryById(new Guid(id));
                return View(category);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditCategory(CategoryDto category)
        {
            using (var proxy = new ProductServiceClient())
            {
                var categoryList = new List<CategoryDto>() {category};
                proxy.UpdateCategories(categoryList.ToArray());
                return RedirectToSuccess("更新商品分类成功!", "Categories","Admin");
            }
        }

        [Authorize]
        public ActionResult DeleteCategory(string id)
        {
            using (var proxy = new ProductServiceClient())
            {
                proxy.DeleteCategories(new List<string> { id }.ToArray());
                return RedirectToSuccess("删除商品分类成功！", "Categories", "Admin");
            }
        }

        [Authorize]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddCategory(CategoryDto category)
        {
            using (var proxy = new ProductServiceClient())
            {
                proxy.CreateCategories(new List<CategoryDto> { category }.ToArray());
                return RedirectToSuccess("添加商品分类成功！", "Categories", "Admin");
            }
        }

        #endregion

        #region Products

        [Authorize]
        public ActionResult Products()
        {
            using (var proxy = new ProductServiceClient())
            {
                var model = proxy.GetProducts();
                return View(model);
            }
        }

        [Authorize]
        public ActionResult EditProduct(string id)
        {
            using (var proxy = new ProductServiceClient())
            {
                var model = proxy.GetProductById(new Guid(id));
                var categories = proxy.GetCategories();
                categories.ToList().Insert(0, new CategoryDto(){  Id = Guid.Empty.ToString(), Name = "(未分类)", Description = "(未分类)" });
                if (model.Category != null)
                    ViewData["categories"] = new SelectList(categories, "Id", "Name", model.Category.Id);
                else
                    ViewData["categories"] = new SelectList(categories, "Id", "Name", Guid.Empty.ToString());
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditProduct(ProductDto product)
        {
            using (var proxy = new ProductServiceClient())
            {
                proxy.UpdateProducts(new List<ProductDto> { product }.ToArray());
                if (product.Category.Id != Guid.Empty.ToString())
                    proxy.CategorizeProduct(new Guid(product.Id), new Guid(product.Category.Id));
                else
                    proxy.UncategorizeProduct(new Guid(product.Id));
                return RedirectToSuccess("更新商品信息成功！", "Products", "Admin");
            }
        }

        [Authorize]
        public ActionResult AddProduct()
        {
            using (var proxy = new ProductServiceClient())
            {
                var categories = proxy.GetCategories();
                categories.ToList().Insert(0, new CategoryDto() { Id = Guid.Empty.ToString(), Name = "(未分类)", Description = "(未分类)" });
                ViewData["categories"] = new SelectList(categories, "Id", "Name", Guid.Empty.ToString());
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddProduct(ProductDto product)
        {
            using (var proxy = new ProductServiceClient())
            {
                if (string.IsNullOrEmpty(product.ImageUrl))
                {
                    var fileName = Guid.NewGuid() + ".png";
                    System.IO.File.Copy(Server.MapPath("~/Images/Products/ProductImage.png"), Server.MapPath(string.Format("~/Images/Products/{0}", fileName)));
                    product.ImageUrl = fileName;
                }
                var addedProducts = proxy.CreateProducts(new List<ProductDto> { product }.ToArray());
                if (product.Category != null &&
                    product.Category.Id != Guid.Empty.ToString())
                    proxy.CategorizeProduct(new Guid(addedProducts[0].Id), new Guid(product.Category.Id));
                return RedirectToSuccess("添加商品信息成功！", "Products", "Admin");
            }
        }

        [Authorize]
        public ActionResult DeleteProduct(string id)
        {
            using (var proxy = new ProductServiceClient())
            {
                proxy.DeleteProducts(new List<string> { id }.ToArray());
                return RedirectToSuccess("删除商品信息成功！", "Products", "Admin");
            }
        }

        #endregion 

        #region User Accounts
        [Authorize]
        public ActionResult UserAccounts()
        {
            using (var proxy = new UserServiceClient())
            {
                var users = proxy.GetUsers();
                var model = new List<UserAccountModel>();
                users.ToList().ForEach(u => model.Add(UserAccountModel.CreateFromDto(u)));
                return View(model);
            }
        }

        [Authorize]
        public ActionResult AddUserAccount()
        {
            using (var proxy = new UserServiceClient())
            {
                var roles = proxy.GetRoles() ?? new List<RoleDto>().ToArray();

                roles.ToList().Insert(0, new RoleDto(){ Id = Guid.Empty.ToString(), Name = "(未指定)", Description = "(未指定)" });

                ViewData["roles"] = new SelectList(roles, "Id", "Name", Guid.Empty.ToString());
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddUserAccount(UserAccountModel model)
        {
            using (var proxy = new UserServiceClient())
            {
                var user = model.ConvertToDto();
                var createdUsers = proxy.CreateUsers(new List<UserDto> { user }.ToArray());
                if (model.Role.Id != Guid.Empty.ToString())
                    proxy.AssignRole(new Guid(createdUsers[0].Id), new Guid(model.Role.Id));
                return RedirectToSuccess("创建用户账户成功！", "UserAccounts", "Admin");
            }
        }

        [Authorize]
        public ActionResult EditUserAccount(string id)
        {
            using (var proxy = new UserServiceClient())
            {
                var user = proxy.GetUserByKey(new Guid(id));
                var model = UserAccountModel.CreateFromDto(user);
                var roles = proxy.GetRoles();
                if (roles == null)
                    roles = new List<RoleDto>().ToArray();
                roles.ToList().Insert(0, new RoleDto() { Id = Guid.Empty.ToString(), Name = "(未指定)", Description = "(未指定)" });
                if (model.Role != null)
                    ViewData["roles"] = new SelectList(roles, "Id", "Name", model.Role.Id);
                else
                    ViewData["roles"] = new SelectList(roles, "Id", "Name", Guid.Empty.ToString());
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditUserAccount(UserAccountModel model)
        {
            using (var proxy = new UserServiceClient())
            {
                var user = model.ConvertToDto();
                proxy.UpdateUsers(new List<UserDto> { user }.ToArray());
                if (model.Role.Id != Guid.Empty.ToString())
                    proxy.AssignRole(new Guid(model.Id), new Guid(model.Role.Id));
                else
                    proxy.UnassignRole(new Guid(model.Id));
                return RedirectToSuccess("更新用户账户成功！", "UserAccounts", "Admin");
            }
        }

        [Authorize]
        public ActionResult DisableUserAccount(string id)
        {
            using (var proxy = new UserServiceClient())
            {
                proxy.DisableUser(new UserDto() { Id = id });
                return RedirectToAction("UserAccounts");
            }
        }

        [Authorize]
        public ActionResult EnableUserAccount(string id)
        {
            using (var proxy = new UserServiceClient())
            {
                proxy.EnableUser(new UserDto() { Id = id });
                return RedirectToAction("UserAccounts");
            }
        }

        [Authorize]
        public ActionResult DeleteUserAccount(string id)
        {
            using (var proxy = new UserServiceClient())
            {
                proxy.DeleteUsers(new List<UserDto> { new UserDto() { Id = id }}.ToArray());
                return RedirectToSuccess("删除用户账户成功！", "UserAccounts", "Admin");
            }
        }
        #endregion

        #region Roles
        [Authorize]
        public ActionResult Roles()
        {
            using (var proxy = new UserServiceClient())
            {
                var model = proxy.GetRoles();
                return View(model);
            }
        }

        [Authorize]
        public ActionResult EditRole(string id)
        {
            using (var proxy = new UserServiceClient())
            {
                var model = proxy.GetUserByKey(new Guid(id));
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditRole(RoleDto model)
        {
            using (var proxy = new UserServiceClient())
            {
                proxy.UpdateRoles(new List<RoleDto> { model }.ToArray());
                return RedirectToSuccess("更新账户角色成功！", "Roles", "Admin");
            }
        }

        public ActionResult DeleteRole(string id)
        {
            using (var proxy = new UserServiceClient())
            {
                proxy.DeleteRoles(new List<string> { id }.ToArray());
                return RedirectToSuccess("删除账户角色成功！", "Roles", "Admin");
            }
        }

        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRole(RoleDto model)
        {
            using (var proxy = new UserServiceClient())
            {
                proxy.CreateRoles(new List<RoleDto> { model }.ToArray());
                return RedirectToSuccess("添加账户角色成功！", "Roles", "Admin");
            }
        }
        #endregion

        #region Orders
        public ActionResult Orders()
        {
            using(var proxy = new OrderServiceClient())
            {
                var model = proxy.GetAllOrders();
                return View(model);
            }
        }

        public ActionResult Order(string id)
        {
            using (var proxy = new OrderServiceClient())
            {
                var model = proxy.GetOrder(new Guid(id));
                return View(model);
            }
        }

        public ActionResult DispatchOrder(string id)
        {
            using (var proxy = new OrderServiceClient())
            {
                proxy.Dispatch(new Guid(id));
                return RedirectToSuccess(string.Format("订单 {0} 已成功发货！", id.ToUpper()), "Orders", "Admin");
            }
        }
        #endregion
    }
}
