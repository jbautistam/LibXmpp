using System;
using Sharp.Xmpp.Extensions.Dataforms;

namespace Bau.Libraries.LibXmppClient.Core.Forms
{
	/// <summary>
	///		Conversor de un formulario devuelto por la librería
	/// </summary>
	internal class FormConversor
	{
		/// <summary>
		///		Convierte un DataForm de Xmpp en un formulario de la librería
		/// </summary>
		internal JabberForm Convert(DataForm dataForm)
		{
			JabberForm form = new JabberForm(ConvertType(dataForm.Type), dataForm.Title, dataForm.Instructions);

				// Convierte los tipos
				for (int index = 0; index < dataForm.Fields.Count; index++)
				{
					string name = GetName(dataForm.Fields[index].Name, index);

						// Añade el elemento convertido
						form.Items.Add(name, ConvertField(dataForm.Fields[index], name));
				}
				// Comprueba si el formulario tiene un captcha
				form.HasCaptcha = CheckHasCaptcha(form);
				// Devuelve el formulario
				return form;
		}

		/// <summary>
		///		Comprueba si el formulario tiene un captcha
		/// </summary>
		private bool CheckHasCaptcha(JabberForm form)
		{ 
			// Comprueba si el formulario tiene un captcha
			foreach (System.Collections.Generic.KeyValuePair<string, JabberFormItem> formItem in form.Items)
				if (formItem.Value.Name == "FORM_TYPE" && formItem.Value.FirstValue == "urn:xmpp:captcha")
					return true;
			// Devuelve el valor que indica si tiene un captcha
			return false;
		}

		/// <summary>
		///		Obtiene el nombre
		/// </summary>
		private string GetName(string name, int index)
		{
			if (!string.IsNullOrEmpty(name))
				return name;
			else
				return $"__Fixed_{index}";
		}

		/// <summary>
		///		Convierte el tipo
		/// </summary>
		private JabberForm.FormType ConvertType(DataFormType type)
		{
			switch (type)
			{
				case DataFormType.Cancel:
					return JabberForm.FormType.Cancel;
				case DataFormType.Result:
					return JabberForm.FormType.Result;
				case DataFormType.Submit:
					return JabberForm.FormType.Submit;
				default:
					return JabberForm.FormType.Form;
			}
		}

		/// <summary>
		///		Convierte un campo
		/// </summary>
		private JabberFormItem ConvertField(DataField dataField, string name)
		{
			JabberFormItem formItem = new JabberFormItem(ConvertFieldType(dataField.Type), name, dataField.Label, dataField.Required);

				// Añade los valores
				formItem.Values.AddRange(dataField.Values);
				// Devuelve el campo
				return formItem;
		}

		/// <summary>
		///		Convierte el tipo de campo
		/// </summary>
		private JabberFormItem.FormItemType ConvertFieldType(DataFieldType? type)
		{
			if (type == null)
				return JabberFormItem.FormItemType.TextSingle;
			else
				switch (type)
				{
					case DataFieldType.Boolean:
						return JabberFormItem.FormItemType.Boolean;
					case DataFieldType.Fixed:
						return JabberFormItem.FormItemType.Fixed;
					case DataFieldType.Hidden:
						return JabberFormItem.FormItemType.Hidden;
					case DataFieldType.JidMulti:
						return JabberFormItem.FormItemType.JidMulti;
					case DataFieldType.JidSingle:
						return JabberFormItem.FormItemType.JidSingle;
					case DataFieldType.ListMulti:
						return JabberFormItem.FormItemType.ListMultiple;
					case DataFieldType.ListSingle:
						return JabberFormItem.FormItemType.ListSingle;
					case DataFieldType.TextMulti:
						return JabberFormItem.FormItemType.TextMultiple;
					case DataFieldType.TextPrivate:
						return JabberFormItem.FormItemType.TextPrivate;
					case DataFieldType.TextSingle:
						return JabberFormItem.FormItemType.TextSingle;
					default:
						return JabberFormItem.FormItemType.Hidden;
				}
		}
	}
}
