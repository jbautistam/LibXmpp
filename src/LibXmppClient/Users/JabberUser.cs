using System;

namespace Bau.Libraries.LibXmppClient.Users
{
	/// <summary>
	///		Clase con los datos del usuario
	/// </summary>
	public class JabberUser : JabberContact
	{
		public JabberUser(string host, string login, string password) : base(host, login)
		{
			Password = password;
		}

		/// <summary>
		///		Contraseña
		/// </summary>
		public string Password { get; }
	}
}
