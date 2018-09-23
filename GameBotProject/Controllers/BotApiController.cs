using System;
using System.Net;
using GameBotProject.Models.VkApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace GameBotProject.Controllers
{
    [Route("")]
    public class BotApiController : Controller
    {
        private readonly IConfiguration configuration;

        public BotApiController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public String OnHandleGetRequest()
        {
            return "Welcome. Go to www.vk.com/club" + configuration["VkApi:GroupId"];
        }

        [HttpPost]
        public String OnHandleRequest([FromBody] JObject request)
        {
            if (request.GetValue("type").Value<String>() == "confirmation" &&
               request.GetValue("group_id").Value<String>() == configuration["VkApi:GroupId"])
            {
                return configuration["VkApi:ConfirmationCode"];
            }

            // Send msg to logs

            switch(request.GetValue("type").Value<String>())
			{
				case "message_new":
					{
						MessageNewModel message = request.SelectToken("object").ToObject<MessageNewModel>();

						string msge = string.Format("pId = {0} <br> {1}", message.PeerId, request["object"]);

						var webRequestq = WebRequest.Create(
							String.Format("{0}messages.send?message={1}&user_id={2}&access_token={3}&v={4}",
							configuration["VkApi:Url"],
							msge,
							request.SelectToken("object.peer_id"),
							configuration["VkApi:AccessToken"],
							configuration["VkApi:Version"]));

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
								configuration["VkApi:Url"],
								msg,
								message.PeerId,
								configuration["VkApi:AccessToken"],
								configuration["VkApi:Version"]));

							webRequest.Method = "GET";

							webRequest.GetResponse();
						}
						else if (message.MessageText == "DataBase")
						{
							string msg = "Надо прикрутить базу данных и протестировать его этим методом";

							webRequest = WebRequest.Create(
								String.Format("{0}messages.send?message={1}&user_id={2}&access_token={3}&v={4}",
								configuration["VkApi:Url"],
								msg,
								message.PeerId,
								configuration["VkApi:AccessToken"],
								configuration["VkApi:Version"]));

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
