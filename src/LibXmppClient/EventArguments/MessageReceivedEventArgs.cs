using System;

namespace Bau.Libraries.LibXmppClient.EventArguments
{
	/// <summary>
	///		Argumentos del evento de recepción de un mensaje
	/// </summary>
	public class MessageReceivedEventArgs : AbstractContactEventArgs
	{
		public MessageReceivedEventArgs(Core.JabberConnection connection, Users.JabberContact contact,
										string subject, string body) : base(connection, contact)
		{
			Subject = subject;
			Body = body;
		}

		/// <summary>
		///		Asunto del mensaje
		/// </summary>
		public string Subject { get; }

		/// <summary>
		///		Cuerpo del mensaje
		/// </summary>
		public string Body { get; }
	}
}
