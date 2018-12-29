using System;

namespace Bau.Libraries.LibXmppClient.Users
{
	/// <summary>
	///		Estado de la suscripción
	/// </summary>
	public class JabberSubscriptionStatus
	{
		/// <summary>
		///		Modo de la suscripción
		/// </summary>
		public enum SubscriptionMode
		{
			/// <summary>El usuario no tiene una suscripción en el contacto y el contacto no tiene una suscripción pendiente</summary>
			None,
			/// <summary>El usuario está pendiente de la respuesta por parte del contacto a la petición de suscripción</summary>
			UserPendingResponseFromContact,
			/// <summary>El contacto está pendiente de la respuesta por parte del usuario</summary>
			ContactPendingResponseFromUser,
			/// <summary>El usuario y el contacto está conectados</summary>
			Both
		}

		/// <summary>
		///		Modo de la suscripción
		/// </summary>
		public SubscriptionMode Mode { get; set; } = SubscriptionMode.None;

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; set; }
	}
}
