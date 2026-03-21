using System.Globalization;

namespace MyRecipeBook.API.Middleware
{
    public class CultureMiddleware(RequestDelegate culture)
    {
        private readonly RequestDelegate _culture = culture;

        public async Task Invoke(HttpContext context)
        {
            var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var cultureQuery = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            var requestedCulture = new CultureInfo("en");
            if (!string.IsNullOrWhiteSpace(cultureQuery)
                && supportedCultures.Any(c => c.Name.Equals(cultureQuery, StringComparison.InvariantCultureIgnoreCase)))
            {
                requestedCulture = new CultureInfo(cultureQuery);
            }

            CultureInfo.CurrentCulture = requestedCulture;
            CultureInfo.CurrentUICulture = requestedCulture;

            await _culture(context);
        }
    }
}
