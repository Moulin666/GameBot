using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace GameBotProject.Models.VkApiModels
{
	[Serializable]
	public class MessageNewModel
    {
		[JsonProperty("date")]
		public UInt32 Date { get; set; }

		[JsonProperty("from_id")]
		public String FromId { get; set; }

		[JsonProperty("id")]
		public String ID { get; set; }

		[JsonProperty("out")]
		public String Out { get; set; }

		[JsonProperty("peer_id")]
		public String PeerId { get; set; }

		[JsonProperty("text")]
		public String MessageText { get; set; }

		[JsonProperty("conversation_message_id")]
		public String ConversationMessageId { get; set; }

		[JsonProperty("fwd_messages")]
		public List<ForwardedMessage> ForwardedMessages { get; set; }

		[JsonProperty("important")]
		public Boolean Important { get; set; }

		[JsonProperty("random_id")]
		public String RandomId { get; set; }

		[JsonProperty("is_hidden")]
		public Boolean IsHidden { get; set; }
	}
}
