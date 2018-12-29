using System;

namespace Bau.Libraries.LibXmppClient.Core.Forms
{
	/// <summary>
	///		Clase con los datos de un formulario de Jabber
	/// </summary>
	public class JabberForm
	{
		/// <summary>
		///		Tipo de formulario
		/// </summary>
		public enum FormType
		{
			/// <summary>Formulario para introducción de datos</summary>
			Form,
			/// <summary>Formulario con datos para enviar</summary>
			Submit,
			/// <summary>Formulario de cancelación de envío de datos</summary>
			Cancel,
			/// <summary>Resultado de una entidad de procesamiento de datos (por ejemplo, resultados de búsqueda)</summary>
			Result
		}

		public JabberForm(FormType type, string title, string instructions = null)
		{
			Type = type;
			Title = title;
			Instructions = instructions;
		}

		/// <summary>
		///		Tipo del formulario
		/// </summary>
		public FormType Type { get; }

		/// <summary>
		///		Título del formulario
		/// </summary>
		public string Title { get; }

		/// <summary>
		///		Instrucciones
		/// </summary>
		public string Instructions { get; }

		/// <summary>
		///		Indica si el formulario tiene un captcha
		/// </summary>
		public bool HasCaptcha { get; internal set; }

		/// <summary>
		///		Elementos del formulario
		/// </summary>
		public System.Collections.Generic.Dictionary<string, JabberFormItem> Items { get; } = new System.Collections.Generic.Dictionary<string, JabberFormItem>();
	}
}
