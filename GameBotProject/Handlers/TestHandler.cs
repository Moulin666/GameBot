using System;
using System.Threading.Tasks;
using GameBotProject.Common;
using GameBotProject.Message;
using GameBotProject.Models.VkApiModels;


namespace GameBotProject.Handlers
{
	public class TestHandler : IMessageHandler
	{
		public String MessageOperation => "тест";

		public async Task<Boolean> HandleMessage (IMessage message)
		{
			var vkApi = (VkApi)message.Parameters[(byte)MessageParameterCode.VkApi];
			var messageModel = (MessageNewModel)message.Parameters[(byte)MessageParameterCode.MessageModel];

			return await vkApi.SendMessage("TestHandler", messageModel.PeerId);
		}
	}
}
