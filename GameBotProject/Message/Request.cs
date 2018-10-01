using System;
using System.Collections.Generic;


namespace GameBotProject.Message
{
	public class Request : IMessage
	{
		protected readonly String _messageOperation;

		protected readonly Dictionary<String, Object> _parameters;

		protected readonly String _debugMessage;

		public String MessageOperation
		{
			get
			{
				return _messageOperation;
			}
		}

		public Dictionary<String, Object> Parameters
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

		public Request (String messageOperation, Dictionary<String, Object> parameters)
		{
			_messageOperation = messageOperation;
			_parameters = parameters;
		}

		public Request (String messageOperation, Dictionary<String, Object> parameters, String debugMessage)
			: this(messageOperation, parameters)
		{
			_debugMessage = debugMessage;
		}
	}
}
