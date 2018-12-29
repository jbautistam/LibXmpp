using System;

namespace Bau.Libraries.LibXmppClient
{
	/// <summary>
	///		Clase de conexión a XMPP
	/// </summary>
	public class JabberManager : IDisposable
	{ 
		// Eventos públicos
		public event EventHandler<EventArguments.MessageReceivedEventArgs> MessageReceived;
		public event EventHandler<EventArguments.ChangedStatusEventArgs> ChangedStatus;
		public event EventHandler<EventArguments.SubscriptionRequestEventArgs> SubscriptionRequested;
		public event EventHandler<EventArguments.RosterUpdatedRequestArgs> RosterUpdated;
		public event EventHandler<EventArguments.ChangedUserStatusEventArgs> ChangedUserStatus;
		public event EventHandler<EventArguments.FormRequestedEventArgs> FormRequested;

		public JabberManager()
		{
			Connections = new Core.JabberConnectionsCollection(this);
		}

		/// <summary>
		///		Registra un usuario
		/// </summary>
		public bool RegisterBegin(string host, out string error)
		{
			Core.Register.JabberRegisterConnection connection = new Core.Register.JabberRegisterConnection(this, new Servers.JabberServer(host));

				// Comienza el proceso de registro
				return connection.RegisterBegin(out error);
		}

		/// <summary>
		///		Comprueba si existe una conexión
		/// </summary>
		public bool Exists(string address, string login)
		{
			return Connections.Exists(address, login);
		}

		/// <summary>
		///		Añade una conexión
		/// </summary>
		public Core.JabberConnection AddConnection(string host, string user, string password)
		{
			return AddConnection(new Servers.JabberServer(host), new Users.JabberUser(host, user, password));
		}

		/// <summary>
		///		Añade una conexión
		/// </summary>
		public Core.JabberConnection AddConnection(Servers.JabberServer server, Users.JabberUser user)
		{
			return Connections.Add(server, user);
		}

		/// <summary>
		///		Lanza el evento de solicitud de formulario
		/// </summary>
		internal void RaiseEventRequestForm(EventArguments.FormRequestedEventArgs arguments)
		{
			FormRequested?.Invoke(this, arguments);
		}

		/// <summary>
		///		Lanza el evento de mensaje recibido
		/// </summary>
		internal void RaiseEventMessageReceived(EventArguments.MessageReceivedEventArgs arguments)
		{
			MessageReceived?.Invoke(this, arguments);
		}

		/// <summary>
		///		Lanza el evento de cambio de estado
		/// </summary>
		internal void RaiseEventChangedStatus(EventArguments.ChangedStatusEventArgs arguments)
		{
			ChangedStatus?.Invoke(this, arguments);
		}

		/// <summary>
		///		Lanza el evento de lista de contactos modificada
		/// </summary>
		internal void RaiseEventRosterUpdated(EventArguments.RosterUpdatedRequestArgs arguments)
		{
			RosterUpdated?.Invoke(this, arguments);
		}

		/// <summary>
		///		Lanza el evento de solicitud de suscripción
		/// </summary>
		internal void RaiseEventSubscriptionRequest(EventArguments.SubscriptionRequestEventArgs arguments)
		{
			SubscriptionRequested?.Invoke(this, arguments);
		}

		/// <summary>
		///		Lanza el evento de cambio de estado del usuario
		/// </summary>
		internal void RaiseEventUserStatusChanged(EventArguments.ChangedUserStatusEventArgs arguments)
		{
			ChangedUserStatus?.Invoke(this, arguments);
		}

		/// <summary>
		///		Desconecta las conexiones
		/// </summary>
		public void Disconnect()
		{
			if (Connections != null)
			{
				Connections.Disconnect();
				Connections.Clear();
			}
		}

		/// <summary>
		///		Libera la memoria
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{ // Desconecta
				if (disposing)
					Disconnect();
				// Indica que se ha liberado
				IsDisposed = true;
			}
		}

		/// <summary>
		///		Libera el objeto
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		///		Conexiones
		/// </summary>
		public Core.JabberConnectionsCollection Connections { get; }

		/// <summary>
		///		Indica si se ha liberado la conexión
		/// </summary>
		public bool IsDisposed { get; private set; }
	}
}
