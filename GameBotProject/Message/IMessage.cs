using System;
using System.Collections.Generic;


namespace GameBotProject.Message
{
	public interface IMessage
    {
		String MessageOperation { get; }

		Dictionary<Byte, Object> Parameters { get; }
	}
}
