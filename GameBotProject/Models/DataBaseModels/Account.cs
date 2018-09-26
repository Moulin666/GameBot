using System;
using System.ComponentModel.DataAnnotations;


namespace GameBotProject.Models.DataBaseModels
{
	public class Account
	{
		[Key]
		public Int32 id { get; set; }

		public String Login { get; set; }

		public Int32 AdminLvl { get; set; }

		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
	}
}
