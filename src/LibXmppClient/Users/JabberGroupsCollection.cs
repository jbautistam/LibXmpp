using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibXmppClient.Users
{
	/// <summary>
	///		Colección de <see cref="JabberGroup"/>
	/// </summary>
	public class JabberGroupsCollection : List<JabberGroup>
	{
		/// <summary>
		///		Añade un grupo
		/// </summary>
		public JabberGroup Add(JabberGroup.GroupType type, string name)
		{
			JabberGroup group = Search(type, name);

				// Si no existe el grupo lo añade
				if (group == null)
				{
					group = new JabberGroup(type, name);
					Add(group);
				}
				// Devuelve el grupo	
				return group;
		}

		/// <summary>
		///		Comprueba si existe un grupo
		/// </summary>
		public bool Exists(JabberGroup.GroupType type, string name)
		{
			return Search(type, name) != null;
		}

		/// <summary>
		///		Busca un grupo
		/// </summary>
		public JabberGroup Search(JabberGroup.GroupType type, string name)
		{
			return this.FirstOrDefault(group => group.Type == type && group.Name.EqualsIgnoreCase(name));
		}
	}
}
