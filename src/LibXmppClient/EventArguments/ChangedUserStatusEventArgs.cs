using System;

namespace Bau.Libraries.LibXmppClient.EventArguments
{
	/// <summary>
	///		Agumentos del evento de cambio de estado de un usuario
	/// </summary>
	public class ChangedUserStatusEventArgs : AbstractJabberEventArgs
	{
		public ChangedUserStatusEventArgs(Core.JabberConnection connection, Users.JabberUser user) : base(connection)
		{
			User = user;
		}

		/// <summary>
		///		Usuario
		/// </summary>
		public Users.JabberUser User { get; }
	}
}
