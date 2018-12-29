using System;

namespace Bau.Libraries.LibXmppClient.EventArguments
{
	/// <summary>
	///		Argumentos del evento de cambio de estado
	/// </summary>
	public class ChangedStatusEventArgs : AbstractContactEventArgs
	{
		public ChangedStatusEventArgs(Core.JabberConnection connection, Users.JabberContact contact, Users.JabberContactStatus newStatus)
								: base(connection, contact)
		{
			NewStatus = newStatus;
		}

		/// <summary>
		///		Nuevo estado del contacto
		/// </summary>
		public Users.JabberContactStatus NewStatus { get; }
	}
}
