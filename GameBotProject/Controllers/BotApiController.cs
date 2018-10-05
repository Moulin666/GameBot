using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using GameBotProject.Common;
using GameBotProject.Message;
using GameBotProject.Models;
using GameBotProject.Models.DataBaseModels;
using GameBotProject.Models.VkApiModels;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;


namespace GameBotProject.Controllers
{
	[Route("")]
    public class BotApiController : Controller
	{
		private readonly ILog _log = LogManager.GetLogger(typeof(BotApiController));

        private readonly IConfiguration _configuration;

		private readonly VkApi _vkApi;
		private readonly DataBaseContext _context;

		public List<IMessageHandler> _requestHandlerList { get; protected set; }

        public BotApiController(IConfiguration configuration, DataBaseContext context)
        {
            _configuration = configuration;
	        _context = context;

	        _vkApi = new VkApi(_configuration);

	        GatherMessageHandlers();
        }

		[HttpGet]
		public String OnHandleGetRequest ()
		{
			return "Welcome. Go to www.vk.com/club" + _configuration["VkApi:GroupId"];
		}

		[HttpPost]
		public async Task<String> OnHandleRequest ([FromBody] JObject request)
		{
			try
			{
				if (request.GetValue("secret").Value<String>() != _configuration["VkApi:SecretKey"])
				{
					return "Error";
				}

				if (request.GetValue("type").Value<String>() == "confirmation" &&
				    request.GetValue("group_id").Value<String>() == _configuration["VkApi:GroupId"])
				{
					return _configuration["VkApi:ConfirmationCode"];
				}

				switch (request.GetValue("type").Value<String>())
				{
					case "message_new":
					{
						MessageNewModel messageModel = request.SelectToken("object").ToObject<MessageNewModel>();

						var dictionary = new Dictionary<Byte, Object>()
						{
							{
								(byte)MessageParameterCode.Configuration, _configuration
							},
							{
								(byte)MessageParameterCode.VkApi, _vkApi
							},
							{
								(byte)MessageParameterCode.DataBase, _context
							},
							{
								(byte)MessageParameterCode.MessageModel, messageModel
							}
						};

						if (messageModel.MessageText.ToLower() == "начать"
						    || messageModel.MessageText.ToLower() == "start")
						{
							if (_requestHandlerList != null)
								await _requestHandlerList.FirstOrDefault(h => h.MessageOperation == "start")
									.HandleMessage(new Request("start", dictionary));

							break;
						}

						Account account = _context.Accounts.FirstOrDefault(a => a.Login == messageModel.FromId);
						if (account == null)
						{
							await _vkApi.SendMessage(
								"Приветствую тебя странник. <br> Используй команду 'начать' или 'start' чтобы я понял что ты готов к приключениям.",
								messageModel.FromId);
							break;
						}

						object isMemberResult = _vkApi.SendMethod("groups.isMember", "group_id", _configuration["VkApi:GroupId"],
							"user_id", messageModel.FromId, "extended", "0").Result;
						try
						{
							if (!JObject.Parse(isMemberResult.ToString()).GetValue("response").Value<Boolean>())
							{
								await _vkApi.SendMessage(
									"Чтобы продолжить тебе необходимо вступить в нашу группу.",
									messageModel.FromId);
								break;
							}
						}
						catch (Exception ex)
						{
							_log.FatalFormat("Is Member Fatal Error - {0}", ex);
						}

						var messageOperation = messageModel.MessageText.Split(' ').First();
						var message = new Request(messageOperation, dictionary);
						var handlers = _requestHandlerList.Where(h => h.MessageOperation == messageOperation.ToLower());

						if (!handlers.Any())
						{
							await _vkApi.SendMessage("Используй команду 'помощь' чтобы получить список доступных возможностей",
								messageModel.FromId);
							break;
						}

						foreach (var handler in handlers)
							await handler.HandleMessage(message);

						break;
					}

					case "group_join":
						break;
				}
			}
			catch (Exception ex)
			{
				_log.FatalFormat("Error text: {0} | Error help link: {1}", ex, ex.HelpLink);
			}

			return "ok";
		}

		public void GatherMessageHandlers()
		{
			var handlers = from t in Assembly.GetAssembly(GetType()).GetTypes().Where(t => t.GetInterfaces()
				.Contains(typeof(IMessageHandler)))
				select Activator.CreateInstance(t) as IMessageHandler;

			_log.InfoFormat("Load handlers. Found {0} handlers.", handlers.Count());

			_requestHandlerList = handlers.ToList();
		}
	}
}
