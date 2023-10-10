using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Http.Extensions;

public class AuthMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // select the role
        var role = context.Session.GetString("role");
        string url = context.Request.GetDisplayUrl();
        // if role equal admin  then allow every paththat contain admin
        if (url.ToLower().Contains("admin"))
        {
            //if role == null return 401
            if (role == null)
            {
                context.Response.Redirect("/Login", true);
                return;
            }
            //if role != admin return 403
            if (role != Constants.Role.Admin)
            {
                context.Response.StatusCode = 403;
                return;
            }
            //if role == admin await next
            await next(context);
            return;
        }
        if (url.ToLower().Contains("user"))
        {
            //if role == null return 401
            if (role == null)
            {
                context.Response.Redirect("/Login", true);
                return;
            }
            //if role != CustomerRole return 403
            if (role != Constants.Role.Customer)
            {
                context.Response.StatusCode = 403;
                return;
            }
            //if role == CustomerRole await next
            await next(context);
            return;
        }

        await next(context);
    } 
}