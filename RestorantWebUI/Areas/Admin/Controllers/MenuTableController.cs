using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restorant.WebUI.Dtos.MenuTableDtos;

namespace Restorant.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/MenuTable")]
    public class MenuTableController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MenuTableController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/MenuTable/GetAllMenuTable");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<List<ResultMenuTableDto>>(jsonData);
                return View(value);
            }
            return View();
        }
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CreateMenuTableDto createMenuTableDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createMenuTableDto);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/MenuTable/AddMenuTable", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "MenuTable", new { area = "Admin" });
            }
            return View(createMenuTableDto);
        }

        [HttpGet]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/MenuTable/GetMenuTableById/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<UpdateMenuTableDto>(jsonData);
                return View(value);
            }
            return View();
        }
        [HttpPost]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(ResultMenuTableDto resultMenuTableDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(resultMenuTableDto);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/MenuTable/UpdateMenuTable", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "MenuTable", new { area = "Admin" });
            }
            return View(resultMenuTableDto);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/MenuTable/DeleteMenuTable/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "MenuTable", new { area = "Admin" });
            }
            return View();
        }

        [HttpGet]
        [Route("TableListByStatus")]
        public async Task<IActionResult> TableListByStatus()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/MenuTable/GetAllMenuTable");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMenuTableDto>>(jsonData);
                return View(values);
            }
            return View();
        }

    }
}
