using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restorant.WebUI.Dtos.CategoryDtos;

namespace Restorant.WebUI.ViewComponents.UILayoutMenuViewComponents
{
    public class _UILayoutMenuCategoriesComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _UILayoutMenuCategoriesComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://signalrapiparanova-cjg4ayb8fpdyfcau.germanywestcentral-01.azurewebsites.net/api/Category/GetAllCategories");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
                return View(value);
            }
            return View();
        }
    }
}
