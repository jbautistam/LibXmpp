using System;

namespace Bau.Libraries.LibXmppClient.Users
{
	/// <summary>
	///		Clase con los datos de un grupo
	/// </summary>
	public class JabberGroup
	{   
		// Enumerados públicos
		/// <summary>
		///		Tipo de grupo
		/// </summary>
		public enum GroupType
		{
			/// <summary>Grupo definido por el usuario</summary>
			User,
			/// <summary>Grupo formado por contactos pendientes de suscripción</summary>
			Subscription
		}

		public JabberGroup(GroupType type, string name)
		{
			Type = type;
			Name = name;
		}

		/// <summary>
		///		Indica si es un grupo de respuesta pendiente
		/// </summary>
		public GroupType Type { get; }

		/// <summary>
		///		Nombre del grupo
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Contactos
		/// </summary>
		public JabberContactsDictionary Contacts { get; } = new JabberContactsDictionary();
	}
}
