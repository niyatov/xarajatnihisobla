using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Newtonsoft.Json;
using OutlayMudBlazor.Dtoes;
using System.Net.Http.Json;

namespace OutlayMudBlazor.Services;

public class OutlayService : HttpClientBase
{
    public OutlayService(HttpClient httpClient) : base(httpClient) { }

    public async Task<Result<List<Outlays>?>> GetOutlaysAsync(string categoryId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/Outlay/GetOutlays?categoryId={categoryId}");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<List<Outlays>>(contentString);

            return new(true) { Data = entity };
        }
        else
        {
            return new(false) { ErrorMessage = contentString };
        }
    }

    public async Task<Result<Outlay>> GetOutlayAsync(string outlayId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/Outlay/GetOutlay/{outlayId}");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<Outlay>(contentString);

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


    public async Task<Result> CreateOutlayAsync(CreateOutlay outlay)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Outlay");
        httpRequest.Content = JsonContent.Create(outlay);

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



    public async Task<Result> UpdateOutlayAsync(UpdateOutlay outlay, string outlayId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/api/Outlay/UpdateOutlay/{outlayId}");
        httpRequest.Content = JsonContent.Create(outlay);

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
                return new(false) { ErrorMessage = contentString };
            }
        }
    }



    public async Task<Result> DeleteOutlayAsync(string outlayId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/Outlay/DeleteOutlay/{outlayId}");
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

}
