using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using System.Drawing; // Added to get a little more fun

namespace ASP_NET_practiceAndHomework_5.Middleware
{
    public class RoutesMiddleware
    {
        private RequestDelegate next;

        List<string> sData;

        // Dependency Injection
        public RoutesMiddleware(RequestDelegate next, List<string> list)
        {
            this.next = next;
            sData = list;
        }

        public async Task InvokeAsync(HttpContext context, IOptions<FontColor>options)
        {
            string path = context.Request.Path;

            // Retrieve the mcolor query parameter
            string color = context.Request.Query["color"].ToString();

            // If mcolor is not provided, you can use a default color
            string colorToUse = string.IsNullOrEmpty(color) ? "green" : color;
            
            /////////////////////////
            ///
            // Some extra addings to make game a little more interesting: let's determine if our color is dark or light
            Color rgbColor = Color.FromName(colorToUse);
            double brightness = 0.2126*rgbColor.R + 0.7152*rgbColor.G + 0.0722*rgbColor.B;
            string textColor = "black";
            if (brightness < 150) // If color is "dark" our font will be white
            {
                textColor = "white";
            }

            if (!string.IsNullOrEmpty(path) && path.ToString() == "/home")
            {
                context.Response.ContentType = "text/html;charset=utf-8";
                await context.Response.WriteAsync($"<h1 style=\"background-color:{colorToUse};padding:10px;border-radius:5px;color:{textColor};\">Student information</h1><div>Brightness: {brightness}" +
                    $"<h2>Name: {sData[0]}</h2>" +
                    $"<h2>Surname: {sData[1]}</h2>" +
                    $"<h2>Age: {sData[2]}</h2>");
            }
            else if (!string.IsNullOrEmpty(path) && path.ToString() == "/academy")
            {
                context.Response.ContentType = "text/html;charset=utf-8";
                await context.Response.WriteAsync($"<h1 style=\"background-color:{colorToUse};padding:10px;border-radius:5px;color:{textColor};\">Subjects list</h1>" +
                    $"<h2>{sData[3]}</h2>");
            }
            else
            {
                await next(context);
            }
        }
    }
}
