using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
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

        public BotApiController(IConfiguration configuration, DataBaseContext context)
        {
            _configuration = configuration;
	        _context = context;

	        _vkApi = new VkApi(_configuration);
		}

        [HttpGet]
        public String OnHandleGetRequest()
        {
			return "Welcome. Go to www.vk.com/club" + _configuration["VkApi:GroupId"];
        }

        [HttpPost]
        public async Task<String> OnHandleRequest([FromBody] JObject request)
        {
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
						MessageNewModel message = request.SelectToken("object").ToObject<MessageNewModel>();

						// TODO : Make redirect to handler.

						if (!_vkApi.SendMessage("Сделать переадресацию",
							message.PeerId).Result)
						{
							_log.ErrorFormat("Message not sent.");
						}
					}
					break;

				case "group_join":
					break;
			}

            return "ok";
        }
    }
}
