using System;

namespace Bau.Libraries.LibXmppClient.Core.Forms
{
	/// <summary>
	///		Elemento de un formulario
	/// </summary>
	public class JabberFormItem
	{
		/// <summary>
		///		Tipo de elemento de un formulario
		/// </summary>
		public enum FormItemType
		{
			// boolean: The field enables an entity to gather or provide an either-or choice between two options. The default value is "false". [10]
			Boolean,
			// fixed: The field is ended for data description (e.g., human-readable text such as "section" headers) rather than data gathering or provision. The <value/> child SHOULD NOT contain newlines (the \n and \r characters); instead an application SHOULD generate multiple fixed fields, each with one <value/> child.
			Fixed,
			// hidden: The field is not shown to the form-submitting entity, but instead is returned with the form. The form-submitting entity SHOULD NOT modify the value of a hidden field, but MAY do so if such behavior is defined for the "using protocol".
			Hidden,
			// jid-multi: The field enables an entity to gather or provide multiple Jabber IDs. Each provided JID SHOULD be unique (as determined by comparison that includes application of the Nodeprep, Nameprep, and Resourceprep profiles of Stringprep as specified in XMPP Core), and duplicate JIDs MUST be ignored. *
			JidMulti,
			// jid-single: The field enables an entity to gather or provide a single Jabber ID. *
			JidSingle,
			// list-multi: The field enables an entity to gather or provide one or more options from among many. A form-submitting entity chooses one or more items from among the options presented by the form-processing entity and MUST NOT insert new options. The form-submitting entity MUST NOT modify the order of items as received from the form-processing entity, since the order of items MAY be significant.**
			ListMultiple,
			// list-single: The field enables an entity to gather or provide one option from among many. A form-submitting entity chooses one item from among the options presented by the form-processing entity and MUST NOT insert new options. **
			ListSingle,
			// text-multi: The field enables an entity to gather or provide multiple lines of text. ***
			TextMultiple,
			// text-private: The field enables an entity to gather or provide a single line or word of text, which shall be obscured in an interface (e.g., with multiple instances of the asterisk character). 
			TextPrivate,
			// text-single: The field enables an entity to gather or provide a single line or word of text, which may be shown in an interface. This field type is the default and MUST be assumed if a form-submitting entity receives a field type it does not understand. 
			TextSingle,
			// Imagen captcha
			Captcha
		}

		public JabberFormItem(FormItemType type, string name, string title, bool blnRequired)
		{
			Type = type;
			Name = name;
			Title = title;
			IsRequired = blnRequired;
		}

		/// <summary>
		///		Obtiene el primer resultado
		/// </summary>
		internal string GetFirstResult()
		{
			if (Results != null && Results.Count > 0)
				return Results[0];
			else
				return "";
		}

		/// <summary>
		///		Nombre del elemento
		/// </summary>
		public string Name { get; }

		/// <summary>
		///		Tipo del elemento
		/// </summary>
		public FormItemType Type { get; }

		/// <summary>
		///		Título del elemento del formulario
		/// </summary>
		public string Title { get; }

		/// <summary>
		///		Indica si es obligatorio
		/// </summary>
		public bool IsRequired { get; }

		/// <summary>
		///		Valor del elemento
		/// </summary>
		public System.Collections.Generic.List<string> Values { get; } = new System.Collections.Generic.List<string>();

		/// <summary>
		///		Primer valor de la lista de valores
		/// </summary>
		public string FirstValue
		{
			get
			{
				if (Values != null && Values.Count > 0)
					return Values[0];
				else
					return "";
			}
		}

		/// <summary>
		///		Valor introducido en el formulario
		/// </summary>
		public System.Collections.Generic.List<string> Results { get; } = new System.Collections.Generic.List<string>();

		/// <summary>
		///		Primer resultado introducido en el formulario
		/// </summary>
		public string FirstResult
		{
			get
			{
				if (Results != null && Results.Count > 0)
					return Results[0];
				else
					return "";
			}
		}
	}
}
