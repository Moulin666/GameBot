using System;
using System.Threading.Tasks;


namespace GameBotProject.Message
{
	public interface IMessageHandler
    {
		String MessageOperation { get; }

		Task<Boolean> HandleMessage (IMessage message);
    }
}
