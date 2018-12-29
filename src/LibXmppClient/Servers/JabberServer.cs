using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibXmppClient.Servers
{
	/// <summary>
	///		Clase con los datos de un servidor
	/// </summary>
	public class JabberServer
	{
		public JabberServer(string address, int port = 5222, bool useTls = true)
		{
			Address = address;
			Port = port;
			UseTls = useTls;
		}

		/// <summary>
		///		Comprueba si dos elementos son iguales
		/// </summary>
		public bool EqualsTo(JabberServer host)
		{
			return Address.EqualsIgnoreCase(host.Address) && Port == host.Port && UseTls == host.UseTls;
		}

		/// <summary>
		///		Dirección del servidor
		/// </summary>
		public string Address { get; }

		/// <summary>
		///		Puerto de conexión
		/// </summary>
		public int Port { get; } = 5222;

		/// <summary>
		///		Indica si se va a utilizar TLS para la conexión
		/// </summary>
		public bool UseTls { get; } = true;
	}
}
