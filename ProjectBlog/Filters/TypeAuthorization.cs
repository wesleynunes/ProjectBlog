using ProjectBlog.Models;
using System.Linq;
using System.Web.Mvc;

namespace ProjectBlog.Filters
{
    public class TypeAuthorization : AuthorizeAttribute
    {
        private UserType[] AuthorizedTypes;

        public TypeAuthorization(UserType[] AuthorizedUserTypes)
        {
            AuthorizedTypes = AuthorizedUserTypes;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool authorized = AuthorizedTypes.Any(t => filterContext.HttpContext.User.IsInRole(t.ToString()));

            if (!authorized)
            {
                filterContext.Controller.TempData["ErrorAuthorization"] = "Você não tem permissão para acessar essa página";

                filterContext.Result = new RedirectResult("~/Posts/index");
            }
        }
    }
}
