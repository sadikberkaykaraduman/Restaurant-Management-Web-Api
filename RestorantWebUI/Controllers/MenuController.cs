using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restorant.WebUI.Dtos.BasketDtos;
using Restorant.WebUI.Dtos.ProductDtos;
using System.Text;

namespace Restorant.WebUI.Controllers
{
    [AllowAnonymous]
    public class MenuController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MenuController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.v = id;
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/Product/GetProductWithCategory");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<List<ResultProductWithCategory>>(jsonData);
                return View(value);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBasket(int id, int menuTableId)
        {
            if (menuTableId == 0)
            {
                return BadRequest("MenuTableId Does Not Contain 0.");
            }

            CreateBasketDto createBasketDto = new CreateBasketDto
            {
                BasketProductId = id,
                BasketMenuTableId = menuTableId
            };

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createBasketDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/Basket/AddBasket", stringContent);

            var client2 = _httpClientFactory.CreateClient();
            await client2.GetAsync($"https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/MenuTable/ChangeMenuTableStatusToTrue/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return Json(createBasketDto);
        }
    }
}
