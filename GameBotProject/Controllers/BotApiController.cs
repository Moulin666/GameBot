using System;
using System.Linq;
using System.Net;
using GameBotProject.Models;
using GameBotProject.Models.DataBaseModels;
using GameBotProject.Models.VkApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;


namespace GameBotProject.Controllers
{
	[Route("")]
    public class BotApiController : Controller
    {
        private readonly IConfiguration _configuration;
		private readonly DataBaseContext _context;

        public BotApiController(IConfiguration configuration, DataBaseContext context)
        {
            _configuration = configuration;
	        _context = context;
        }

        [HttpGet]
        public String OnHandleGetRequest()
        {
	        return "Welcome. Go to www.vk.com/club" + _configuration["VkApi:GroupId"];
        }

        [HttpPost]
        public String OnHandleRequest([FromBody] JObject request)
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

						string msge = "Привет";

						var webRequestq = WebRequest.Create(
							String.Format("{0}messages.send?message={1}&user_id={2}&access_token={3}&v={4}",
							_configuration["VkApi:Url"],
							msge,
							request.SelectToken("object.peer_id"),
							_configuration["VkApi:AccessToken"],
							_configuration["VkApi:Version"]));

						webRequestq.Method = "GET";

						webRequestq.GetResponse();

						// Handle msg

						#region test answer

						WebRequest webRequest;

						if (message.MessageText == "герой" || message.MessageText == "hero")
						{
							string msg = "Твой герой....";

							webRequest = WebRequest.Create(
								String.Format("{0}messages.send?message={1}&user_id={2}&access_token={3}&v={4}",
								_configuration["VkApi:Url"],
								msg,
								message.PeerId,
								_configuration["VkApi:AccessToken"],
								_configuration["VkApi:Version"]));

							webRequest.Method = "GET";

							webRequest.GetResponse();
						}
						else if (message.MessageText == "dbtest")
						{
							DbTest dbTest = _context.DbTest.First();

							string msg = $"Start DataBase test.<br>Return string is {dbTest.ReturnString}";

							webRequest = WebRequest.Create(
								String.Format("{0}messages.send?message={1}&user_id={2}&access_token={3}&v={4}",
								_configuration["VkApi:Url"],
								msg,
								message.PeerId,
								_configuration["VkApi:AccessToken"],
								_configuration["VkApi:Version"]));

							webRequest.Method = "GET";

							webRequest.GetResponse();
						}

						#endregion
					}
					break;

				case "group_join":
					break;
			}

            return "ok";
        }
    }
}
