using System;
using System.ComponentModel.DataAnnotations;


namespace GameBotProject.Models.DataBaseModels
{
	public class DbTest
	{
		[Key]
		public Int32 id { get; set; }

		public String ReturnString { get; set; }
	}
}
