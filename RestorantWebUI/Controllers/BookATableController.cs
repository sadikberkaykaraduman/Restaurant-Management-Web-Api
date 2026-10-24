using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restorant.WebUI.Dtos.BookingDtos;

namespace Restorant.WebUI.Controllers
{
    [AllowAnonymous]
    public class BookATableController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BookATableController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/Contact/GetAllContacts");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JArray item = JArray.Parse(responseBody);
            string value = item[0]["contactLocation"].ToString();
            ViewBag.location = value;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateBookingDto createBookingDto)
        {
            HttpClient client2 = new HttpClient();
            HttpResponseMessage response = await client2.GetAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/Contact/GetAllContacts");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JArray item = JArray.Parse(responseBody);
            string value = item[0]["contactLocation"].ToString();
            ViewBag.location = value;

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createBookingDto);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/Booking/AddBooking", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "ShowCase");
            }
            else
            {
                var errorContent = await responseMessage.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorContent);
                return View();
            }
        }
    }
}
