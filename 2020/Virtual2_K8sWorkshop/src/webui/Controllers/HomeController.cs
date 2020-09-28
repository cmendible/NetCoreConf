namespace webui.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using webui.Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var client = _httpClientFactory.CreateClient("kubernetes-client");

            // Call *privacywebapi*, and display its response in the page
            var request = new System.Net.Http.HttpRequestMessage();
            request.RequestUri = new Uri("http://privacywebapi/privacy");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            ViewData["Message"] = await response.Content.ReadAsStringAsync();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
