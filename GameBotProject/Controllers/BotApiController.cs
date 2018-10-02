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
        public String OnHandleGetRequest()
        {
	        return "Welcome. Go to www.vk.com/club" + _configuration["VkApi:GroupId"];
        }

        [HttpPost]
        public async Task<String> OnHandleRequest([FromBody] JObject request)
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

            // Send msg to logs

            switch(request.GetValue("type").Value<String>())
			{
				case "message_new":
					{
						MessageNewModel messageModel = request.SelectToken("object").ToObject<MessageNewModel>();

						var dictionary = new Dictionary<Byte, Object>()
						{
							{ (byte)MessageParameterCode.Configuration, _configuration },
							{ (byte)MessageParameterCode.VkApi, _vkApi },
							{ (byte)MessageParameterCode.DataBase, _context },
							{ (byte)MessageParameterCode.MessageModel, messageModel }
						};

						if (messageModel.MessageText.ToLower() == "начать"
						    || messageModel.MessageText.ToLower() == "start")
						{
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

						// TODO : Check if not subscribe notify and break.

						var messageOperation = messageModel.MessageText.Split(' ').First();
						var message = new Request(messageOperation, dictionary);
						var handlers = _requestHandlerList.Where(h => h.MessageOperation == messageOperation.ToLower());

						await _vkApi.SendMessage(string.Format("DEBUG : Operation - {0}. Found {1} handlers.",
							messageOperation, handlers.Count()), messageModel.FromId);

						if (!handlers.Any())
						{
							await _vkApi.SendMessage("TODO : Check, if account already register return hero, else return notify",
								messageModel.FromId);

							break;
						}

						foreach (var handler in handlers)
						{
							await handler.HandleMessage(message);
						}
					}
					break;

				case "group_join":
					break;
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
