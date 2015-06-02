using System;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineStore.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected Guid UserId
        {
            get
            {
                if (Session["UserId"] != null)
                    return (Guid)Session["UserId"];
                else
                {
                    var id = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                    Session["UserId"] = id;
                    return id;
                }
            }
        }

        protected ActionResult RedirectToSuccess(string pageTitle, string action = "Index", string controller = "Home", int waitSeconds = 3)
        {
            return this.RedirectToAction("SuccessPage", "Home", new { pageTitle = pageTitle, retAction = action, retController = controller, waitSeconds = waitSeconds });
        }
    }
}
