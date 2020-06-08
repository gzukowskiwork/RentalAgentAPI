using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;


namespace SimpleFakeRent.Extensions
{
    public static class AppExtensions
    {
        public static void SwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
