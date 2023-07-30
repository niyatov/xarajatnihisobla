using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Newtonsoft.Json;
using OutlayMudBlazor.Dtoes;
using System.Net.Http.Json;

namespace OutlayMudBlazor.Services;

public class CategoryService : HttpClientBase
{
    public CategoryService(HttpClient httpClient) : base(httpClient) { }

    public async Task<Result<List<Categories>?>> GetCategoriesAsync()
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, "/api/Category/GetCategories");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<List<Categories>>(contentString);

            return new(true) { Data = entity };
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
            return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
        }
    }


    public async Task<Result<Category>> GetCategoryAsync(string categoryId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/Category/GetCategory/{categoryId}");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<Category>(contentString);

            return new(true) { Data = entity };
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new(false) { IsNotFound = true };
            }
            else
            {
                var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
                return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
            }
        }
    }

    public async Task<Result<int>> CreateCategoryAsync(CreateCategory category)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/Category/CreateCategory");
        httpRequest.Content = JsonContent.Create(category);

        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<int>(contentString);

            return new(true) { Data = entity };
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
            return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
        }
    }


    public async Task<Result> UpdateCategoryAsync(UpdateCategory category, string categoryId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/api/Category/UpdateCategory/{categoryId}");
        httpRequest.Content = JsonContent.Create(category);

        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new(true);
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new(false) { IsNotFound = true };
            }
            else
            {
                var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
                return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
            }
        }
    }


    public async Task<Result> DeleteCategoryAsync(string categoryId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/Category/DeleteCategory/{categoryId}");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var deleteOrganziationJson = await response.Content.ReadAsStringAsync();
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new(true);
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
            return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
        }
    }


    public async Task<Result<int>> JoinCategoryAsync(JoinCategory category)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Category/JoinCategory");
        httpRequest.Content = JsonContent.Create(category);

        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<int>(contentString);

            return new(true) { Data = entity };
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
            return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
        }
    }

    public async Task<Result> CleanCategoryAsync(string categoryId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/Category/CleanCategory/{categoryId}");

        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var deleteOrganziationJson = await response.Content.ReadAsStringAsync();
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new(true);
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
            return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
        }
    }


    public async Task<Result<Statistics>> ShowStatistics(string categoryId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/Category/ShowStatistics/{categoryId}");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<Statistics>(contentString);

            return new(true) { Data = entity };
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new(false) { IsNotFound = true };
            }
            else
            {
                var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
                return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
            }
        }
    }

    public async Task<Result<Users>> ShowUsers(string categoryId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/Category/ShowUsers/{categoryId}");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<Users>(contentString);

            return new(true) { Data = entity };
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
            return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
        }
    }

    public async Task<Result<string>> DeleteUserAsync(string categoryId, int userId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/Category/DeleteUser/{categoryId}/{userId}");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var deleteOrganziationJson = await response.Content.ReadAsStringAsync();
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new(true) { Data = contentString };
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<string?[]>(contentString);
            return new(false) { ErrorMessage = errors?[0], ErrorDetails = errors?[1] };
        }
    }

}
