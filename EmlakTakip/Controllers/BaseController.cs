


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmlakTakip.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
    {
        var path = context.HttpContext.Request.Path.Value?.ToLower();

        // Eğer giriş veya kayıt sayfasındaysa kontrol yapma
        if (path != null && (path.Contains("/account/login") || path.Contains("/account/login")))
        {
            base.OnActionExecuting(context);
            return;
        }

        var adminId = context.HttpContext.Session.GetInt32("AdminKullaniciAdi");

        if (adminId == null)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }

        base.OnActionExecuting(context); 
    }
    }
}
