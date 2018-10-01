using System;
using System.Threading.Tasks;
using GameBotProject.Message;
using GameBotProject.Models.VkApiModels;


namespace GameBotProject.Handlers
{
	public class TestHandler : IMessageHandler
	{
		public String MessageOperation
		{
			get
			{
				return "тест";
			}
		}

		public async Task<Boolean> HandleMessage (IMessage message)
		{
			var vkApi = (VkApi)message.Parameters["vkApi"];
			var messageModel = (MessageNewModel)message.Parameters["messageNewModel"];

			return await vkApi.SendMessage("TestHandler", messageModel.PeerId);
		}
	}
}
