using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibXmppClient.Users
{
	/// <summary>
	///		Diccionario de contactos
	/// </summary>
	public class JabberContactsDictionary : Dictionary<string, JabberContact>
	{
		/// <summary>
		///		Añade un contacto
		/// </summary>
		public void Add(JabberContact contact)
		{
			Add(contact.Jid, contact);
		}

		/// <summary>
		///		Obtiene un contacto
		/// </summary>
		public JabberContact GetContact(string jid)
		{
			if (TryGetValue(jid, out JabberContact contact))
				return contact;
			else
				return null;
		}

		/// <summary>
		///		Busca un contacto por dominio y nombre
		/// </summary>
		internal JabberContact Search(string host, string login)
		{
			return GetContact($"{login}@{host}");
		}
	}
}
