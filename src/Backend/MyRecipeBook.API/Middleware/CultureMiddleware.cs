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

            var culture = new CultureInfo("en");
            if (string.IsNullOrWhiteSpace(cultureQuery) == false
                && supportedCultures.Any(c => c.Name.Equals(cultureQuery, StringComparison.InvariantCultureIgnoreCase)))
            {
                culture = new CultureInfo(cultureQuery);
            }

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            await _culture(context);
        }
    }
}
