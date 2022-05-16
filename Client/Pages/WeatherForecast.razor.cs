using Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Client.Pages
{
    public partial class WeatherForecast
    {
		private List<Api.WeatherForecast> WeatherForecasts = new();
		[Inject] private HttpClient HttpClient { get; set; }
		[Inject] private IConfiguration Config { get; set; }
		[Inject] private ITokenService TokenService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			var tokenResponse = await TokenService.GetToken("WeatherForecastAPIAll");
			HttpClient.SetBearerToken(tokenResponse.AccessToken);

			var result = await HttpClient.GetAsync(Config["apiUrl"] + "/api/WeatherForecast");

			if (result.IsSuccessStatusCode)
			{
				WeatherForecasts = await result.Content.ReadFromJsonAsync<List<Api.WeatherForecast>>();
			}
		}
	}
}
