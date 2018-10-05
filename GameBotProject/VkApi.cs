using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Configuration;


namespace GameBotProject
{
	public class VkApi
	{
		private readonly ILog _log = LogManager.GetLogger(typeof(VkApi));
		private readonly IConfiguration _configuration;

		public VkApi (IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<Object> SendMethod (String method, params String[] args)
		{
			if (args.Length == 0)
			{
				_log.Error("Send method exception. Args not valid.");
				return false;
			}

			try
			{
				StringBuilder parameters = new StringBuilder();
				for (int i = 0; i < args.Length; i += 2)
				{
					if (i == 0)
						parameters.AppendFormat("{0}={1}", args[i], args[i + 1]);
					else
						parameters.AppendFormat("&{0}={1}", args[i], args[i + 1]);
				}

				string requestUri = String.Format("{0}{1}?{2}&access_token={3}&v={4}",
					_configuration["VkApi:Url"],
					method,
					parameters,
					_configuration["VkApi:AccessToken"],
					_configuration["VkApi:Version"]);

				_log.Info(requestUri);

				var webRequest = WebRequest.Create(requestUri);
				webRequest.Method = "GET";

				var responseObject = await webRequest.GetResponseAsync();
				var responseStream = responseObject.GetResponseStream();
				var sr = new StreamReader(responseStream ?? throw new InvalidOperationException());
				string requestResponse = await sr.ReadToEndAsync();

				_log.DebugFormat("Send request. requestUri - {0} | requestResponse - {1}", requestUri, requestResponse);
				return requestResponse;
			}
			catch (Exception e)
			{
				_log.ErrorFormat("Crash SendMethod method - {0}", e);
				return false;
			}
		}

		public async Task<Boolean> SendMessage (String message, String userId)
		{
			try
			{
				string requestUri = String.Format("{0}{1}?message={2}&user_id={3}&access_token={4}&v={5}",
					_configuration["VkApi:Url"],
					"messages.send",
					message,
					userId,
					_configuration["VkApi:AccessToken"],
					_configuration["VkApi:Version"]);

				_log.Info(requestUri);

				var webRequest = WebRequest.Create(requestUri);
				webRequest.Method = "GET";

				var responseObject = await webRequest.GetResponseAsync();
				var responseStream = responseObject.GetResponseStream();
				var sr = new StreamReader(responseStream ?? throw new InvalidOperationException());
				string requestResponse = await sr.ReadToEndAsync();

				_log.DebugFormat("Send message request. msg - {0} | user_id {1}| requestResponse - {2}",
					message, userId, requestResponse);
				
				return true;
			}
			catch (Exception e)
			{
				_log.ErrorFormat("Crash SendMessage method. Exception - {0}", e);
				return false;
			}
		}
	}
}
