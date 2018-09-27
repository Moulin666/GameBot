using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace GameBotProject
{
	public class VkApi
	{
		private readonly IConfiguration _configuration;

		public VkApi (IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<Boolean> SendMethod (String method, params String[] args)
		{
			if (args.Length == 0)
			{
				// TODO : Log message: "Send method exception. Args not valid."
				return false;
			}

			StringBuilder parameters = new StringBuilder();
			foreach (var arg in args)
			{

			}

			string requestUri = String.Format("{0}{1}?{2}&access_token={3}&v={4}",
				_configuration["VkApi:Url"],
				method,
				parameters,
				_configuration["VkApi:AccessToken"],
				_configuration["VkApi:Version"]);

			var webRequest = WebRequest.Create(requestUri);
			webRequest.Method = "GET";

			await webRequest.GetResponseAsync();

			// TODO : Log message: String.Format("Send response. requestUri - {0} | requestResponse - {1}", requestUri, requestResponse);
			return true;
		}
	}
}
