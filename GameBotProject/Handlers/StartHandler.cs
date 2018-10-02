﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GameBotProject.Common;
using GameBotProject.Message;
using GameBotProject.Models;
using GameBotProject.Models.DataBaseModels;
using GameBotProject.Models.VkApiModels;


namespace GameBotProject.Handlers
{
	public class StartHandler : IMessageHandler
	{
		public String MessageOperation => "start";

		public async Task<Boolean> HandleMessage (IMessage message)
		{
			var vkApi = (VkApi)message.Parameters[(byte)MessageParameterCode.VkApi];
			var messageModel = (MessageNewModel)message.Parameters[(byte)MessageParameterCode.MessageModel];
			var context = (DataBaseContext)message.Parameters[(byte)MessageParameterCode.DataBase];

			Account account = context.Accounts.FirstOrDefault(a => a.Login == messageModel.FromId);
			if (account == null)
			{
				account = new Account();
				account.Login = messageModel.FromId;
				account.created_at = DateTime.Now;
				account.updated_at = DateTime.Now;

				context.Add(account);
				await context.SaveChangesAsync();

				await vkApi.SendMessage("*Приветствующий текст, повествующий что-нибудь интересное*", messageModel.FromId);
				await vkApi.SendMessage("Чтобы создать персонажа используй команду 'герой [Имя] [М / Ж] [Человек / NaN / NaN]'", messageModel.FromId);
			
				return true;
			}

			Character character = context.Characters.FirstOrDefault(c => c.AccountId == account);
			if (character == null)
			{
				await vkApi.SendMessage("Приветствую, ты ещё не закончили процесс создания своего героя", messageModel.FromId);
				await vkApi.SendMessage("Чтобы создать персонажа используй команду 'герой [Имя] [М / Ж] [Человек / NaN / NaN]'", messageModel.FromId);

				return true;
			}

			await vkApi.SendMessage("Используй команду 'помощь' чтобы получить список доступных возможностей",
				messageModel.FromId);
			await vkApi.SendMessage("Да начнутся приключения...", messageModel.FromId);

			return true;
		}
	}
}
