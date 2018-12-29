using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibXmppClient.Core
{
	/// <summary>
	///		Colección de conexiónes
	/// </summary>
	public class JabberConnectionsCollection : List<JabberConnection>
	{
		internal JabberConnectionsCollection(JabberManager manager)
		{
			Manager = manager;
		}

		/// <summary>
		///		Añade una conexión
		/// </summary>
		internal JabberConnection Add(Servers.JabberServer host, Users.JabberUser user)
		{
			JabberConnection connection = Search(host, user);

				// Si no existía la conexión, se añade
				if (connection == null)
				{ 
					// Crea el objeto
					connection = new JabberConnection(Manager, host, user);
					// Añade la conexión
					Add(connection);
				}
				// Devuelve la conexión
				return connection;
		}

		/// <summary>
		///		Busca una conexión en la colección
		/// </summary>
		internal JabberConnection Search(Servers.JabberServer host, Users.JabberUser user)
		{ 
			// Recorre las conexiones
			foreach (JabberConnection connection in this)
				if (connection.Host.EqualsTo(host) && connection.User.EqualsTo(user))
					return connection;
			// Si ha llegado hasta aquí es porque no existe
			return null;
		}

		/// <summary>
		///		Comprueba si existe una conexión
		/// </summary>
		internal bool Exists(string address, string login)
		{ 
			// Recorre las conexiones
			foreach (JabberConnection connection in this)
				if (connection.Host.Address.EqualsIgnoreCase(address) &&
						connection.User.Login.EqualsIgnoreCase(login))
					return true;
			// Si ha llegado hasta aquí es porque no existe la conexión
			return false;
		}

		/// <summary>
		///		Desconecta
		/// </summary>
		internal void Disconnect()
		{
			foreach (JabberConnection connection in this)
				connection.Disconnect();
		}

		/// <summary>
		///		Manager de conexiones
		/// </summary>
		private JabberManager Manager { get; }
	}
}
