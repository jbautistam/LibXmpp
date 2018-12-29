using System;

namespace Bau.Libraries.LibXmppClient.EventArguments
{
	/// <summary>
	///		Argumentos para los eventos de Jabber
	/// </summary>
	public abstract class AbstractJabberEventArgs : EventArgs
	{
		public AbstractJabberEventArgs(Core.JabberConnection connection)
		{
			Connection = connection;
		}

		/// <summary>
		///		Conexión que lanza el evento
		/// </summary>
		public Core.JabberConnection Connection { get; }
	}
}
