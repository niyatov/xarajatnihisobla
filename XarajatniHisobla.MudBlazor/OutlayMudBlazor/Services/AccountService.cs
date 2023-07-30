using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Newtonsoft.Json;
using OutlayMudBlazor.Dtoes;
using System.Net.Http.Json;

namespace OutlayMudBlazor.Services;

public class AccountService : HttpClientBase
{
    public AccountService(HttpClient httpClient) : base(httpClient) { }

    public async Task<Result<UserAvatar>> GetAccountAsync()
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, "/api/Account/Get");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<UserAvatar>(contentString);

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
                return new(false) { ErrorMessage = contentString };
            }
        }
    }


    public async Task<Result> CreateOrUpdateAvatar(Avatar account)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Account/CreateOrUpdateAvatar");
        httpRequest.Content = JsonContent.Create(account);

        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new(true);
        }
        else
        {
            return new(false) { ErrorMessage = contentString };
        }
    }



    public async Task<Result> UpdateAccountAsync(UpdateAccount account)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Put, "/api/Account/UpdateAccount");
        httpRequest.Content = JsonContent.Create(account);

        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new(true);
        }
        else
        {
            return new(false) { ErrorMessage = contentString };
        }
    }



    public async Task<Result> DeleteAvatarAsync()
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/Account/DeleteAvatar");
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
            return new(false) { ErrorMessage = contentString };
        }
    }


    public async Task<Result> LogOutAsync()
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/Account/LogOut");
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
            return new(false) { ErrorMessage = contentString };
        }
    }

    public async Task<Result> RegisterAsync(Register register)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Account/Register");
        httpRequest.Content = JsonContent.Create(register);

        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new(true);
        }
        else
        {
            return new(false) { ErrorMessage = contentString };
        }
    }

    public async Task<Result> SignInAsync(SignIn signIn)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Account/SignIn");
        httpRequest.Content = JsonContent.Create(signIn);

        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new(true);
        }
        else
        {
            return new(false) { ErrorMessage = contentString };
        }
    }

}