using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Newtonsoft.Json;
using OutlayMudBlazor.Dtoes;
using System.Net.Http.Json;

namespace OutlayMudBlazor.Services;

public class DebtService : HttpClientBase
{
    public DebtService(HttpClient httpClient) : base(httpClient) { }

    public async Task<Result<List<Debts>?>> GetDebtsAsync()
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, "/api/Debt/GetDebts");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<List<Debts>>(contentString);

            return new(true) { Data = entity };
        }
        else
        {
            return new(false) { ErrorMessage = contentString };
        }
    }


    public async Task<Result> CreateDebtAsync(CreateDebt debt)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Debt/CreateDebt");
        httpRequest.Content = JsonContent.Create(debt);

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


    public async Task<Result> UpdateDebtAsync(UpdateDebt debt, string debtId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/api/Debt/UpdateDebt/{debtId}");
        httpRequest.Content = JsonContent.Create(debt);

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



    public async Task<Result> UpdateDebtForDelete(int debtId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/api/Debt/UpdateDebtForDelete/{debtId}");
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


    public async Task<Result> UpdateDebtByReveiver(UpdateDebtByReveiver debt, int debtId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/api/Debt/UpdateDebtByReveiver/{debtId}");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        httpRequest.Content = JsonContent.Create(debt);

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

    public async Task<Result> DeleteDebtAsync(int debtId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/Debt/DeleteDebt/{debtId}");
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

    public async Task<Result<List<DebtStatistics>?>> ShowResults()
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/Debt/ShowResults");
        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await httpClient.SendAsync(httpRequest);
        var contentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var entity = JsonConvert.DeserializeObject<List<DebtStatistics>>(contentString);

            return new(true) { Data = entity };
        }
        else
        {
            return new(false) { ErrorMessage = contentString };
        }
    }
}