using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Restorant.WebUI.Dtos.OrderDetailDtos;
using Restorant.WebUI.Dtos.ProductDtos;

namespace Restorant.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/OrderDetail")]
    public class OrderDetailController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderDetailController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/OrderDetail/GetOrderWithProduct");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<List<ResultOrderDetailWithProductDto>>(jsonData);
                return View(value);
            }
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/Product/GetAllProducts");
            var JsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(JsonData);
            List<SelectListItem> productValues = (from x in values
                                                   select new SelectListItem
                                                   {
                                                       Text = x.ProductName,
                                                       Value = x.ProductId.ToString()
                                                   }).ToList();
            ViewBag.ProductValues = productValues;
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CreateOrderDetailDto createOrderDetailDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createOrderDetailDto);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/OrderDetail/AddOrderDetail", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "OrderDetail", new { area = "Admin" });
            }
            return View(createOrderDetailDto);
        }

        [HttpGet]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var responseMessage1 = await client.GetAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/Product/GetAllProducts");
            var JsonData1 = await responseMessage1.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(JsonData1);
            List<SelectListItem> productValues = (from x in values
                                                  select new SelectListItem
                                                  {
                                                      Text = x.ProductName,
                                                      Value = x.ProductId.ToString()
                                                  }).ToList();
            ViewBag.ProductValues = productValues;

            var responseMessage = await client.GetAsync($"https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/OrderDetail/GetOrderWithProductByOrderId/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<UpdateOrderDetailDto>(jsonData);
                return View(value);
            }
            return View();
        }

        [HttpPost]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(UpdateOrderDetailDto updateOrderDetailDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateOrderDetailDto);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/OrderDetail/UpdateOrderDetail", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "OrderDetail", new { area = "Admin" });
            }
            return View(updateOrderDetailDto);
        }

        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/OrderDetail/DeleteOrderDetail/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "OrderDetail", new { area = "Admin" });
            }
            return View();
        }
    }
}
