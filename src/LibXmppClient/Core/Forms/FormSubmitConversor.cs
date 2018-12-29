using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Sharp.Xmpp.Extensions.Dataforms;

namespace Bau.Libraries.LibXmppClient.Core.Forms
{
	/// <summary>
	///		Convierte los datos de un formulario para enviarlos
	/// </summary>
	public class FormSubmitConversor
	{
		/// <summary>
		///		Convierte los datos de un formulario
		/// </summary>
		internal DataField[] Convert(JabberForm form)
		{
			List<DataField> result = new List<DataField>();

				// Convierte los resultados
				foreach (KeyValuePair<string, JabberFormItem> keyValue in form.Items)
					if (MustSend(keyValue.Value))
						result.Add(Convert(keyValue.Value));
				// Devuelve los datos
				return result.ToArray();
		}

		/// <summary>
		///		Comprueba si se debe enviar un resultado
		/// </summary>
		private bool MustSend(JabberFormItem formItem)
		{
			return formItem.Type != JabberFormItem.FormItemType.Fixed;
		}

		/// <summary>
		///		Convierte los valores de un elemento de formulario
		/// </summary>
		private DataField Convert(JabberFormItem formItem)
		{
			switch (formItem.Type)
			{
				case JabberFormItem.FormItemType.Hidden:
					return new HiddenField(formItem.Name, formItem.Values.ToArray());
				case JabberFormItem.FormItemType.Boolean:
					return new BooleanField(formItem.Name, formItem.GetFirstResult().GetBool());
				case JabberFormItem.FormItemType.TextMultiple:
					return new TextMultiField(formItem.Name, formItem.Results.ToArray());
				case JabberFormItem.FormItemType.TextPrivate:
					return new PasswordField(formItem.Name, formItem.GetFirstResult());
				case JabberFormItem.FormItemType.TextSingle:
					return new TextField(formItem.Name, formItem.GetFirstResult());
				default:
					return null;
			}
		}
	}
}
