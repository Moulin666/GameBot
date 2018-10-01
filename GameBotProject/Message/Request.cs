using System;
using System.Collections.Generic;


namespace GameBotProject.Message
{
	public class Request : IMessage
	{
		protected readonly String _messageOperation;

		protected readonly Dictionary<Byte, Object> _parameters;

		protected readonly String _debugMessage;

		public String MessageOperation
		{
			get
			{
				return _messageOperation;
			}
		}

		public Dictionary<Byte, Object> Parameters
		{
			get
			{
				return _parameters;
			}
		}

		public String DebugMessage
		{
			get
			{
				return _debugMessage;
			}
		}

		public Request (String messageOperation, Dictionary<Byte, Object> parameters)
		{
			_messageOperation = messageOperation;
			_parameters = parameters;
		}

		public Request (String messageOperation, Dictionary<Byte, Object> parameters, String debugMessage)
			: this(messageOperation, parameters)
		{
			_debugMessage = debugMessage;
		}
	}
}
