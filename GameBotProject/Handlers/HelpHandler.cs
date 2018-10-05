using System;
using System.Text;
using System.Threading.Tasks;
using GameBotProject.Common;
using GameBotProject.Message;
using GameBotProject.Models;
using GameBotProject.Models.VkApiModels;


namespace GameBotProject.Handlers
{
	public class HelpHandler : IMessageHandler
	{
		public String MessageOperation => "помощь";

		public async Task<Boolean> HandleMessage(IMessage message)
		{
			var vkApi = (VkApi)message.Parameters[(byte)MessageParameterCode.VkApi];
			var messageModel = (MessageNewModel)message.Parameters[(byte)MessageParameterCode.MessageModel];

			StringBuilder msg = new StringBuilder();
			msg.Append("🌸 Доступные команды 🌸");
			msg.Append("<br>Герой - получить информацию о твоём герое.");

			await vkApi.SendMessage(msg.ToString(), messageModel.FromId);

			return true;
		}
	}
}
