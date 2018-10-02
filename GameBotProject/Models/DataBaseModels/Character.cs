using System;
using System.ComponentModel.DataAnnotations;


namespace GameBotProject.Models.DataBaseModels
{
	public class Character
	{
		[Key]
		public Int32 id { get; set; }

		public Account AccountId { get; set; }

		public String Name { get; set; }

		public Int32 Gender { get; set; }
		public Int32 Race { get; set; }

		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
	}
}
