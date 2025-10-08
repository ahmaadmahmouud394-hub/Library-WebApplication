using Library_WebApplication.Busniness_Object;
using Library_WebApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Library_WebApplication.Middleware
{
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthenticationBO _authentication;
        public SessionValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, AuthenticationBO authBO)
        {
            var path = context.Request.Path.Value?.ToLower();

            // --- 1. Allow access to public/unrestricted paths, including authentication pages and static files ---
            if (path != null &&
                (
                 path.StartsWith("/css") ||
                 path.StartsWith("/js") ||
                 path.StartsWith("/images")))
            {
                await _next(context);
                return;
            }

            string referer = "?";
            if (context.Request.Method.ToLower() != "get")
            {
                Uri.TryCreate(context.Request.Headers.Referer.ToString(), new UriCreationOptions(), out var res);
                referer = res?.AbsolutePath ?? "/";
            }

            if (path == "/" && (referer) != "/")
            {
                path = referer.ToLower();
            }

            if(path.StartsWith("/user"))
            {
                _next(context);
                return;
            }
            var userIdString = context.Session.GetString("UserId");
            var role = context.Session.GetString("Role");
            var token = context.Session.GetString("token");
            switch (role)
            {
                case "Admin":
                    if (path.StartsWith("/admin"))
                    {
                        _next(context);
                        return;
                    }
                    break;
                case "Employee":
                    if (path.StartsWith("/employee"))
                    {
                        _next(context);
                        return;
                    }
                    break;
                case "Client":
                    if (path.StartsWith("/client"))
                    {
                        _next(context);
                        return;
                    }
                    break;

                default:
                    context.Response.Redirect("/User/AccessDenied");
                    break;
            }
            if (path.StartsWith("/admin"))
            {
                _next(context);
                return;
            }
            if (path.StartsWith("/employee"))
            {
                _next(context);
                return;
            }
            if (path.StartsWith("/client"))
            {
                _next(context);
                return;
            }

            // Paths like /User/SomeOtherAction should be allowed *only* if authenticated.
            // The previous logic allowed all /User paths, which is typically not correct 
            // for authenticated areas like /User/Profile unless intended as public access.
            // I've removed the redundant /User check here to rely on the authentication check below.




            // --- 2. Authentication Check ---

            if (string.IsNullOrEmpty(userIdString))
            {
                // Not logged in → redirect to SignIn
                context.Response.Redirect("/User/Index");
                return;
            }

            // Optional: Re-validate token if it's missing but UserId is present
            if (string.IsNullOrEmpty(token))
            {
                // Re-authenticate logic using the ID we have from session
                if (Int32.TryParse(userIdString, out int ID))
                {
                    var user = new User() { Id = ID };
                    // This is likely calling a service to get the user/token and set session again
                    var userAuth = authBO.GetAuthenticated(user);
                    // After re-authentication, you should typically check if it succeeded 
                    // and maybe re-set session variables if they changed.
                }
                else
                {
                    // If UserId is present but invalid/unparseable, redirect to sign in
                    context.Response.Redirect("/User/Index");
                    return;
                }
            }

            // --- 3. Authorization Check (Role-Based) ---

            // Ensure path is not null before checking for roles
            if (path != null)
            {
                // If attempting to access an /admin path without the 'Admin' role
                if (path.StartsWith("/admin") && role != "Admin")
                {
                    context.Response.Redirect("/User/AccessDenied");
                    return;
                }
            }

            // The rest of the requests (authenticated and authorized) proceed
            await _next(context);
        }
        //public async Task InvokeAsync(HttpContext context, AuthenticationBO authBO)
        //{
        //    var path = context.Request.Path.Value?.ToLower();
        //    Console.WriteLine("-------------------------------");
        //    Console.WriteLine(path);
        //    Console.WriteLine("-------------------------------");


        //    // Allow access to SignIn, SignUp, or static files
        //    if (path != null &&
        //        (path.Contains("/User/Index") ||
        //         path.Contains("/User/Signup") ||
        //         path.Contains("/css") ||
        //         path.Contains("/js") ||
        //         path.Contains("/images")))
        //    {
        //        await _next(context);
        //        return;
        //    }

        //    var UserId = context.Session.GetString("UserId");
        //    int ID;
        //    Int32.TryParse(UserId, out ID);
        //    var token = context.Session.GetString("Token");
        //    var role = context.Session.GetString("Role");
        //    var user = new User()
        //    {
        //        Id = ID
        //    };

    //    if (path.StartsWith("/User"))
    //    {
    //        await _next(context);
    //        return;
    //    }
    //    if (string.IsNullOrEmpty(token))
    //    {
    //        var userAuth = authBO.GetAuthenticated(user);
    //    }
    //    // Check session for logged-in user
    //    if (string.IsNullOrEmpty(UserId))
    //    {
    //        // Not logged in → redirect to SignIn
    //        context.Response.Redirect("/Authentications/SignIn");
    //        return;
    //    }



    //    if (user == null)
    //    {
    //        //context.Session.Clear();
    //        context.Response.Redirect("/Authentications/SignIn");

    //    }

    //    //role = user.Role.Name;
    //    if (path.StartsWith("/admin") && role != "Admin")
    //    {

    //        context.Response.Redirect("/Authentications/AccessDenied");

    //        return;
    //    }


    //    await _next(context);
    //}
}
}
