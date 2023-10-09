using FUCarRentingSystem_RazorPage.Utils;

public class AuthMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // select the role
        var role = context.Session.GetString("role");

        // if role equal admin  then allow every paththat contain admin
        if (context.Request.Path.Value.ToLower().Contains("admin"))
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
        if (context.Request.Path.Value.ToLower().Contains("user"))
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