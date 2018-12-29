using System;

namespace Bau.Libraries.LibXmppClient.EventArguments
{
	/// <summary>
	///		Argumentos de los eventos de solicitud de datos de formulario
	/// </summary>
	public class FormRequestedEventArgs : EventArgs
	{
		public FormRequestedEventArgs(Core.Forms.JabberForm form)
		{
			Form = form;
		}

		/// <summary>
		///		Formulario
		/// </summary>
		public Core.Forms.JabberForm Form { get; }

		/// <summary>
		///		Indica si se ha cancelado
		/// </summary>
		public bool Cancel { get; set; }
	}
}
