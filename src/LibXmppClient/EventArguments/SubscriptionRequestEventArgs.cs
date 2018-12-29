using System;

namespace Bau.Libraries.LibXmppClient.EventArguments
{
	/// <summary>
	///		Argumentos del evento de solicitud de suscripción por parte de otro usuario
	/// </summary>
	public class SubscriptionRequestEventArgs : AbstractJabberEventArgs
	{ 
		// Enumerados públicos
		public enum SubscriptionStatus
		{
			/// <summary>Se acepta la suscripción </summary>
			Accepted,
			/// <summary>Se rechaza la suscripción</summary>
			Refused,
			/// <summary>Se espera a más tarde</summary>
			Wait
		}

		public SubscriptionRequestEventArgs(Core.JabberConnection connection, string jid) : base(connection)
		{
			Jid = jid;
			Status = SubscriptionStatus.Wait;
		}

		/// <summary>
		///		Jid que solicita la suscripción
		/// </summary>
		public string Jid { get; }

		/// <summary>
		///		Indica si se acepta la conexión
		/// </summary>
		public SubscriptionStatus Status { get; set; }
	}
}
