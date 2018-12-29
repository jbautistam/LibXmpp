using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibXmppClient.Core.Register
{
	/// <summary>
	///		Clase de ayuda para el registro de un usuario
	/// </summary>
	/// <remarks>
	///		Se separa de <see cref="JabberConnection"/> para facilitar el desarrollo
	/// </remarks>
	public class JabberRegisterConnection : IDisposable
	{
		internal JabberRegisterConnection(JabberManager manager, Servers.JabberServer host)
		{
			Manager = manager;
			Host = host;
		}

		/// <summary>
		///		Comienza el registro de un usuario
		/// </summary>
		internal bool RegisterBegin(out string error)
		{ 
			// Inicializa los argumentos de salida
			error = "";
			// Desconecta
			Disconnect();
			// Conecta al servidor
			XmppClient = new Sharp.Xmpp.Client.XmppClient(Host.Address);
			XmppClient.Connect();
			// Asigna el callback de registro
			try
			{
				XmppClient.Register(OnRegisterCallback);
			}
			catch (Sharp.Xmpp.XmppErrorException exception)
			{
				error = "Error en el registro: " + exception.Message;
			}
			catch (Exception exception)
			{
				error = "Error general: " + exception.Message;
			}
			// Si se ha cancelado el registro, no se debe devolver el error de la excepción si no uno más particular
			if (IsCancel)
				error = "Registro cancelado por el usuario";
			// Devuelve el valor que indica si todo ha ido bien
			return error.IsEmpty();
		}

		/// <summary>
		///		Callback de registro
		/// </summary>
		internal Sharp.Xmpp.Extensions.Dataforms.SubmitForm OnRegisterCallback(Sharp.Xmpp.Extensions.Dataforms.RequestForm dataForm)
		{
			Forms.JabberForm form = new Forms.FormConversor().Convert(dataForm);
			EventArguments.FormRequestedEventArgs evntArgs = new EventArguments.FormRequestedEventArgs(form);

				// Lanza el evento de solicitud de formulario
				Manager.RaiseEventRequestForm(evntArgs);
				// Guarda el valor que indica si es una cancelación
				IsCancel = evntArgs.Cancel;
				// Obtiene los datos
				if (!evntArgs.Cancel)
					return new Sharp.Xmpp.Extensions.Dataforms.SubmitForm(new Forms.FormSubmitConversor().Convert(form));
				else
					return null;
		}

		/// <summary>
		///		Desconecta del servidor
		/// </summary>
		internal void Disconnect()
		{
			if (XmppClient != null)
				XmppClient.Close();
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
		///		Indica si se ha cancelado el registro (y por tanto no se debe devolver la excepción)
		/// </summary>
		public bool IsCancel { get; private set; }

		/// <summary>
		///		Indica si se ha liberado la memoria
		/// </summary>
		public bool IsDisposed { get; private set; }
	}
}
