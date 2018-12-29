using System;

namespace Bau.Libraries.LibXmppClient.Users
{
	/// <summary>
	///		Estado
	/// </summary>
	public class JabberContactStatus
	{
		/// <summary>
		///		Estado del usuario (disponibilidad)
		/// </summary>
		public enum Availability
		{
			/// <summary>Offline - no disponible</summary>
			Offline,
			/// <summary>Online - disponible</summary>
			Online,
			/// <summary>Temporalmente no disponible</summary>
			Away,
			/// <summary>Preparado para chatear</summary>
			Chat,
			/// <summary>Ocupado</summary>
			Dnd,
			/// <summary>Fuera de conexión por un periodo largo de tiempo</summary>
			Xa
		}

		/// <summary>
		///		Estado actual
		/// </summary>
		public Availability Status { get; set; } = Availability.Offline;

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		///		Prioridad del estado en el recurso
		/// </summary>
		public int Priority { get; set; }
	}
}
