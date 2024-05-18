namespace Microsoft.AspNetCore.Http
{
    using legend.Entities;

    public static class HttpContextExtension
    {
        public static User GetUserIdFromContext(this HttpContext httpContext)
        {
            if (httpContext.Items["User"] is User user)
            {
                return user;
            }
            throw new Exception("User claim not found.");
        }
    }
}
