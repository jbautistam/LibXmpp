using System;

namespace Bau.Libraries.LibXmppClient.EventArguments
{
	/// <summary>
	///		Argumentos del evento de modificación de la lista de contactos
	/// </summary>
	public class RosterUpdatedRequestArgs : AbstractJabberEventArgs
	{
		public RosterUpdatedRequestArgs(Core.JabberConnection connection) : base(connection) { }
	}
}
