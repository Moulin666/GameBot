using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBotProject.Common;
using GameBotProject.Message;
using GameBotProject.Models;
using GameBotProject.Models.DataBaseModels;
using GameBotProject.Models.VkApiModels;


namespace GameBotProject.Handlers
{
	public class HeroHandler : IMessageHandler
	{
		public String MessageOperation => "герой";

		public async Task<Boolean> HandleMessage(IMessage message)
		{
			var vkApi = (VkApi)message.Parameters[(byte)MessageParameterCode.VkApi];
			var messageModel = (MessageNewModel)message.Parameters[(byte)MessageParameterCode.MessageModel];
			var context = (DataBaseContext)message.Parameters[(byte)MessageParameterCode.DataBase];

			Account account = context.Accounts.FirstOrDefault(a => a.Login == messageModel.FromId);
			if (account == null)
				return true;

			Character character = context.Characters.FirstOrDefault(c => c.Account == account);
			if (character == null)
			{
				if (messageModel.MessageText.Split(' ').Count() != 4)
				{
					await vkApi.SendMessage(
						"Чтобы создать персонажа используй команду 'герой [Имя] [М / Ж] [Галл / Римлянин / Британец]'",
						messageModel.FromId);
					return true;
				}

				string name = messageModel.MessageText.Split(' ')[1];
				if (name.Length < 6 || name.Length > 16)
				{
					await vkApi.SendMessage("Твоё имя не может быть меньше 6 или больше 16 символов",
						messageModel.FromId);
					return true;
				}

				string gender = messageModel.MessageText.Split(' ')[2].ToLower();
				if (gender == "м")
				{
					gender = "Мужской";
				}
				else if (gender == "ж")
				{
					gender = "Женский";
				}
				else
				{
					await vkApi.SendMessage("Пол может быть только М или Ж.", messageModel.FromId);
					return true;
				}

				string race = messageModel.MessageText.Split(' ')[3].ToLower();
				if (race == "галл")
				{
					race = "Галл";
				}
				else if (race == "римлянин")
				{
					race = "Римлянин";
				}
				else if (race == "британец")
				{
					race = "Британец";
				}
				else
				{
					await vkApi.SendMessage("Расса может быть только Галл, Римлянин или Британец.",
						messageModel.FromId);
					return true;
				}

				character = new Character()
				{
					Account = account,
					Name = name,
					Gender = gender,
					Race = race,

					created_at = DateTime.Now,
					updated_at = DateTime.Now
				};

				context.Add(character);
				context.SaveChanges();
			}

			StringBuilder heroInfo = new StringBuilder();
			heroInfo.Append($"🔱 Новобранец {character.Name}");

			//if (character.Guild != "Нет")
			//heroInfo.Append($"({character.Guild.Name})");

			heroInfo.Append($"<br><br>💰 Серебро: 0 | Золото: 0");

			heroInfo.Append($"<br> <br> 🀄 Раса: {character.Race}");
			heroInfo.Append($"<br> 🔮 Пол: {character.Gender}");
			heroInfo.Append($"<br> 🔋 Энергия: 0/5");

			heroInfo.Append($"<br><br> 🗿 Статистика 🗿");
			heroInfo.Append($"<br> Уровень: 1 | Опыт: 0/100");
			heroInfo.Append($"<br> Здоровье: 100 | Интеллект: 100");
			heroInfo.Append($"<br> Физ. защита: 0 | Физ. атака: 0");
			heroInfo.Append($"<br> Маг. защита: 0 | Маг. атака: 0");

			heroInfo.Append($"<br><br> 🎽 Одежда: +0 к физ. защите +0 к маг. защите");
			heroInfo.Append($"<br> ⚔️ Оружие: +0 к физ. атаке +0 к маг. атаке");

			heroInfo.Append($"<br><br> ☀ Занятость: Ничем не занят");

			heroInfo.Append($"<br><br> 😈 Топ в рейтинге: Moulin666");
			heroInfo.Append($"<br> Место в рейтинге: 1");

			await vkApi.SendMessage(heroInfo.ToString(), messageModel.FromId);
			return true;
		}
	}
}
