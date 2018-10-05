using System;
using System.ComponentModel.DataAnnotations;


namespace GameBotProject.Models.DataBaseModels
{
	public class Character
	{
		[Key]
		public Int32 id { get; set; }

		public Account Account { get; set; }

		public String Name { get; set; }
		public String Gender { get; set; }
		public String Race { get; set; }

		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
	}
}
