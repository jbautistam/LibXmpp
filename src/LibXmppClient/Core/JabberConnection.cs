using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibXmppClient.Users;
using Sharp.Xmpp;
using Sharp.Xmpp.Im;

namespace Bau.Libraries.LibXmppClient.Core
{
	/// <summary>
	///		Cliente de Jabber
	/// </summary>
	public class JabberConnection : IDisposable
	{
		internal JabberConnection(JabberManager manager, Servers.JabberServer host, JabberUser user)
		{
			Manager = manager;
			Host = host;
			User = user;
		}

		/// <summary>
		///		Conecta al servidor
		/// </summary>
		public void Connect()
		{ 
			// Desconecta
			Disconnect();
			// Conecta al servidor
			XmppClient = new Sharp.Xmpp.Client.XmppClient(Host.Address, User.Login, User.Password);
			XmppClient.Connect();
			// XmppClient.DebugStanzas = true;
			// Inicializa los eventos
			InitEventHandlers();
			// Carga el roster
			LoadRoster();
			// Cambia el estado a online
			SetStatus(JabberContactStatus.Availability.Online, "En línea");
		}

		/// <summary>
		///		Carga el roster
		/// </summary>
		private void LoadRoster()
		{
			JabberGroup groupUndefined, groupSubscription;

				// Limpia los grupos y los contactos
				Groups.Clear();
				Contacts.Clear();
				// Crea el grupo de "indefinidos"
				groupUndefined = Groups.Add(JabberGroup.GroupType.User, "<No agrupados>");
				groupSubscription = Groups.Add(JabberGroup.GroupType.Subscription, "<Pendientes suscripción>");
				// Añade los contactos al diccionario y los grupos
				foreach (RosterItem rosterItem in XmppClient.GetRoster())
				{
					JabberContact contact = new JabberContact(rosterItem.Jid.Domain, rosterItem.Jid.Node,
															  rosterItem.Jid.Resource, rosterItem.Name);
					bool added = false;

						// Asigna el estado
						contact.Subscription.Mode = GetSubscriptionMode(rosterItem.SubscriptionState, rosterItem.Pending);
						// Añade el contacto 
						Contacts.Add(contact);
						// Añade el contacto a los grupos
						if (contact.Subscription.Mode != JabberSubscriptionStatus.SubscriptionMode.Both)
						{
							groupSubscription.Contacts.Add(contact);
							added = true;
						}
						else
							foreach (string groupName in rosterItem.Groups)
							{
								JabberGroup group = Groups.Add(JabberGroup.GroupType.User, groupName);

									// Añade el contacto
									group.Contacts.Add(contact);
									// Indica que se ha añadido al menos a un grupo
									added = true;
							}
						// Si no se ha añadido a ningún grupo, se añade al de "Indefinidos"
						if (!added)
							groupUndefined.Contacts.Add(contact);
				}
				// Lanza el evento de modificación del roster
				Manager.RaiseEventRosterUpdated(new EventArguments.RosterUpdatedRequestArgs(this));
		}

		/// <summary>
		///		Obtiene el modo de suscripción de un contacto
		/// </summary>
		private JabberSubscriptionStatus.SubscriptionMode GetSubscriptionMode(SubscriptionState state, bool isPending)
		{
			switch (state)
			{
				case SubscriptionState.Both:
					return JabberSubscriptionStatus.SubscriptionMode.Both;
				case SubscriptionState.To:
					return JabberSubscriptionStatus.SubscriptionMode.UserPendingResponseFromContact;
				case SubscriptionState.From:
					return JabberSubscriptionStatus.SubscriptionMode.ContactPendingResponseFromUser;
				default:
					return JabberSubscriptionStatus.SubscriptionMode.None;
			}
		}

		/// <summary>
		///		Inicializa los manejadores de eventos
		/// </summary>
		private void InitEventHandlers()
		{ 
			// Asigna la función callback de suscripción
			XmppClient.SubscriptionRequest = OnSubscriptionRequestCallbak;
			// Inicializa los manejadores de eventos
			XmppClient.Message += (sender, evntArgs) => ReceiveMessage(evntArgs.Jid, evntArgs.Message);
			XmppClient.RosterUpdated += (sender, evntArgs) => LoadRoster();
			XmppClient.SubscriptionApproved += (sender, evntArgs) => ApproveSubscription(evntArgs.Jid);
			XmppClient.SubscriptionRefused += (sender, evntArgs) => DeleteContact(GetContact(evntArgs.Jid));
			XmppClient.StatusChanged += (sender, evntArgs) => ReceiveStatus(evntArgs.Jid, evntArgs.Status);
			XmppClient.Error += (sender, evntArgs) => Console.WriteLine(evntArgs.Reason);
			XmppClient.Unsubscribed += (sender, evntArgs) => Console.WriteLine("Unsubscribed " + evntArgs.Jid);
			XmppClient.ChatStateChanged += (sender, evntArgs) => Console.WriteLine("ChatStateChanged");
		}

		/// <summary>
		///		Callback para el tratamiento de solicitudes de recepción
		/// </summary>
		private bool OnSubscriptionRequestCallbak(Jid jid)
		{
			EventArguments.SubscriptionRequestEventArgs arguments = new EventArguments.SubscriptionRequestEventArgs(this, $"{jid.Node}@{jid.Domain}");

				// Lanza el evento
				Manager.RaiseEventSubscriptionRequest(arguments);
				// Si se ha aceptado, envía una solicitud
				switch (arguments.Status)
				{
					case EventArguments.SubscriptionRequestEventArgs.SubscriptionStatus.Accepted:
							ApproveSubscription(jid);
						break;
					case EventArguments.SubscriptionRequestEventArgs.SubscriptionStatus.Refused:
							XmppClient.Im.RefuseSubscriptionRequest(jid);
						break;
				}
				// Devuelve el valor que indica si se ha aceptado
				return arguments.Status == EventArguments.SubscriptionRequestEventArgs.SubscriptionStatus.Accepted;
		}

		/// <summary>
		///		Aprueba una solicitud de suscripción
		/// </summary>
		private void ApproveSubscription(Jid jid)
		{ 
			// Indica que se ha aprobado la solicitud
			XmppClient.Im.ApproveSubscriptionRequest(jid);
			// Añade el contacto
			XmppClient.AddContact(jid);
		}

		/// <summary>
		///		Trata la recepción de un mensaje
		/// </summary>
		private void ReceiveMessage(Jid jid, Message message)
		{
			JabberContact contact = GetContact(jid);

				if (contact != null)
					Manager.RaiseEventMessageReceived(new EventArguments.MessageReceivedEventArgs(this, contact, message.Subject, message.Body));
				else
					Console.WriteLine($"Se ha recibido un mensaje de un desconocido: {jid.ToString()} {message.Body}");
		}

		/// <summary>
		///		Recibe un cambio de estado
		/// </summary>
		private void ReceiveStatus(Jid jid, Status status)
		{
			JabberContact contact = GetContact(jid);

				if (contact != null)
				{ 
					// Cambia el estado
					contact.Status.Status = GetContactStatus(status.Availability);
					contact.Status.Message = status.Message;
					contact.Status.Priority = status.Priority;
					// Lanza el evento de cambio de estado
					Manager.RaiseEventChangedStatus(new EventArguments.ChangedStatusEventArgs(this, contact, contact.Status));
				}
				else if (jid.Domain.EqualsIgnoreCase(User.Host) && jid.Node.EqualsIgnoreCase(User.Login))
				{
					User.Status.Status = GetContactStatus(status.Availability);
					Manager.RaiseEventUserStatusChanged(new EventArguments.ChangedUserStatusEventArgs(this, User));
				}
		}

		/// <summary>
		///		Obtiene la disponiblidad de un contacto
		/// </summary>
		private JabberContactStatus.Availability GetContactStatus(Availability availability)
		{
			switch (availability)
			{
				case Availability.Away:
					return JabberContactStatus.Availability.Away;
				case Availability.Chat:
					return JabberContactStatus.Availability.Chat;
				case Availability.Dnd:
					return JabberContactStatus.Availability.Dnd;
				case Availability.Online:
					return JabberContactStatus.Availability.Online;
				case Availability.Xa:
					return JabberContactStatus.Availability.Xa;
				default:
					return JabberContactStatus.Availability.Offline;
			}
		}

		/// <summary>
		///		Modifica el estado del usuario
		/// </summary>
		public void SetStatus(JabberContactStatus.Availability status, string message)
		{ 
			// Cambia el estado del usuario
			User.Status.Status = status;
			User.Status.Message = message;
			// Evita los errores con el estado Offline
			if (status == JabberContactStatus.Availability.Offline)
				Disconnect();
			else
				XmppClient.Im.SetStatus(GetContactAvailability(status), message);
		}

		/// <summary>
		///		Obtiene la disponibilidad de un contacto a partir de su estado
		/// </summary>
		private Availability GetContactAvailability(JabberContactStatus.Availability status)
		{
			switch (status)
			{
				case JabberContactStatus.Availability.Away:
					return Availability.Away;
				case JabberContactStatus.Availability.Chat:
					return Availability.Chat;
				case JabberContactStatus.Availability.Dnd:
					return Availability.Dnd;
				case JabberContactStatus.Availability.Online:
					return Availability.Online;
				default:
					return Availability.Xa;
			}
		}

		/// <summary>
		///		Añade un contacto
		/// </summary>
		public void AddContact(string jid, string nickName)
		{
			XmppClient.AddContact(new Jid(jid), nickName);
		}

		/// <summary>
		///		Añade una sala de chat
		/// </summary>
		public void AddChatRoom(string jid, string chatRoom)
		{
			XmppClient.Im.RequestSubscription(new Jid(jid));
			XmppClient.AddContact(new Jid(jid), chatRoom);
		}

		/// <summary>
		///		Elimina un contacto
		/// </summary>
		public void DeleteContact(JabberContact contact)
		{
			if (contact != null)
			{
				XmppClient.Im.RefuseSubscriptionRequest(new Jid(contact.Jid));
				XmppClient.RemoveContact(new Jid(contact.Jid));
			}
		}

		/// <summary>
		///		Obtiene un contacto del diccionario
		/// </summary>
		private JabberContact GetContact(Jid jid)
		{
			return Contacts.Search(jid.Domain, jid.Node);
		}

		/// <summary>
		///		Envía un mensaje de chat a un contacto
		/// </summary>
		public void SendMessage(JabberContact contact, string text)
		{
			XmppClient.SendMessage(contact.FullJid, text, null, null, MessageType.Chat);
		}

		/// <summary>
		///		Desconecta del servidor
		/// </summary>
		public void Disconnect()
		{ 
			// Cierra la conexión XMPP
			if (XmppClient != null)
				XmppClient.Close();
			// Libera la memoria
			if (Groups != null)
				Groups.Clear();
			if (Contacts != null)
				Contacts.Clear();
		}

		/// <summary>
		///		Libera la memoria
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		///		Libera la memoria
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{   
				// Desconecta
				if (disposing)
					Disconnect();
				// Indica que ya se ha liberado
				IsDisposed = true;
			}
		}

		/// <summary>
		///		Manager de Jabber
		/// </summary>
		private JabberManager Manager { get; }

		/// <summary>
		///		Servidor
		/// </summary>
		public Servers.JabberServer Host { get; }

		/// <summary>
		///		Cliente de la librería de acceso Xmpp
		/// </summary>
		private Sharp.Xmpp.Client.XmppClient XmppClient { get; set; } = null;

		/// <summary>
		///		Usuario conectado al servidor
		/// </summary>
		public JabberUser User { get; }

		/// <summary>
		///		Grupos de contactos
		/// </summary>
		public JabberGroupsCollection Groups { get; } = new JabberGroupsCollection();

		/// <summary>
		///		Contactos
		/// </summary>
		public JabberContactsDictionary Contacts { get; } = new JabberContactsDictionary();

		/// <summary>
		///		Indica si está conectado
		/// </summary>
		public bool IsConnected
		{
			get
			{
				if (XmppClient == null)
					return false;
				else
					return XmppClient.Connected;
			}
		}

		/// <summary>
		///		Indica si se ha liberado la memoria
		/// </summary>
		public bool IsDisposed { get; private set; }
	}
}
