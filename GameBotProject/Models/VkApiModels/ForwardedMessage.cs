using System;
using Newtonsoft.Json;


namespace GameBotProject.Models.VkApiModels
{
	[Serializable]
	public class ForwardedMessage
    {
		[JsonProperty("date")]
		public UInt32 Date { get; set; }

		[JsonProperty("from_id")]
		public String FromId { get; set; }

		[JsonProperty("text")]
		public String MessageText { get; set; }

		[JsonProperty("update_time")]
		public UInt32 UpdateTime { get; set; }
	}
}
