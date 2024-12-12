using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Frontend.Filters 
{
    public class SupplierAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var supplierJson = context.HttpContext.Session.GetString("supplier");

            if (string.IsNullOrEmpty(supplierJson))
            {
                context.Result = new RedirectToActionResult("Login", "Supplier", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
