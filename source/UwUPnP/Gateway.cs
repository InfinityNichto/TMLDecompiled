using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using Terraria.ModLoader;

namespace UwUPnP;

internal sealed class Gateway
{
	private readonly string serviceType;

	private readonly string controlURL;

	private static readonly string ssdpLineSep;

	private static readonly string[] searchMessageTypes;

	public IPAddress InternalClient { get; }

	public IPAddress ExternalIPAddress
	{
		get
		{
			if (!RunCommand("GetExternalIPAddress").TryGetValue("NewExternalIPAddress", out var ret))
			{
				return null;
			}
			return IPAddress.Parse(ret);
		}
	}

	static Gateway()
	{
		ssdpLineSep = ((Environment.GetEnvironmentVariable("SSDP_HEADER_USE_LF") == "1") ? "\n" : "\r\n");
		searchMessageTypes = new string[3] { "urn:schemas-upnp-org:device:InternetGatewayDevice:1", "urn:schemas-upnp-org:service:WANIPConnection:1", "urn:schemas-upnp-org:service:WANPPPConnection:1" };
		Logging.tML.Debug((object)("SSDP search line separator: " + ((ssdpLineSep == "\n") ? "LF" : "CRLF")));
	}

	private Gateway(IPAddress ip, string data)
	{
		InternalClient = ip;
		(serviceType, controlURL) = GetInfo(GetLocation(data));
	}

	public static bool TryNew(IPAddress ip, out Gateway gateway)
	{
		IPEndPoint endPoint = IPEndPoint.Parse("239.255.255.250:1900");
		using Socket socket = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp)
		{
			ReceiveTimeout = 3000,
			SendTimeout = 3000
		};
		socket.Bind(new IPEndPoint(ip, 0));
		byte[] buffer = new byte[1536];
		string[] array = searchMessageTypes;
		foreach (string type in array)
		{
			string request = string.Join(ssdpLineSep, "M-SEARCH * HTTP/1.1", "HOST: 239.255.255.250:1900", "ST: " + type, "MAN: \"ssdp:discover\"", "MX: 2", "", "");
			byte[] req = Encoding.ASCII.GetBytes(request);
			try
			{
				socket.SendTo(req, endPoint);
			}
			catch (SocketException)
			{
				gateway = null;
				return false;
			}
			int receivedCount = 0;
			for (int i = 0; i < 20; i++)
			{
				try
				{
					receivedCount = socket.Receive(buffer);
				}
				catch (SocketException)
				{
					break;
				}
				try
				{
					gateway = new Gateway(ip, Encoding.ASCII.GetString(buffer, 0, receivedCount));
					return true;
				}
				catch
				{
					gateway = null;
				}
			}
		}
		gateway = null;
		return false;
	}

	private static string GetLocation(string data)
	{
		foreach (string line in from l in data.Split('\n')
			select l.Trim() into l
			where l.Length > 0
			select l)
		{
			if (line.StartsWith("HTTP/1.") || line.StartsWith("NOTIFY *"))
			{
				continue;
			}
			int colonIndex = line.IndexOf(':');
			if (colonIndex < 0)
			{
				continue;
			}
			string name = line.Substring(0, colonIndex);
			object obj;
			if (line.Length < name.Length)
			{
				obj = null;
			}
			else
			{
				string text = line;
				int num = colonIndex + 1;
				obj = text.Substring(num, text.Length - num).Trim();
			}
			string val = (string)obj;
			if (name.ToLowerInvariant() == "location")
			{
				if (val.IndexOf('/', 7) == -1)
				{
					throw new Exception("Unsupported Gateway");
				}
				return val;
			}
		}
		throw new Exception("Unsupported Gateway");
	}

	private static (string serviceType, string controlURL) GetInfo(string location)
	{
		IEnumerable<XElement> enumerable = from d in XDocument.Load(location).Descendants()
			where d.Name.LocalName == "service"
			select d;
		(string, string) ret = (null, null);
		foreach (XElement item in enumerable)
		{
			string serviceType = null;
			string controlURL = null;
			foreach (XNode item2 in item.Nodes())
			{
				if (!(item2 is XElement { FirstNode: XText i } ele))
				{
					continue;
				}
				string text = ele.Name.LocalName.Trim().ToLowerInvariant();
				if (!(text == "servicetype"))
				{
					if (text == "controlurl")
					{
						controlURL = i.Value.Trim();
					}
				}
				else
				{
					serviceType = i.Value.Trim();
				}
			}
			if (serviceType != null && controlURL != null && (serviceType.ToLowerInvariant().Contains(":wanipconnection:") || serviceType.ToLowerInvariant().Contains(":wanpppconnection:")))
			{
				ret.Item1 = serviceType;
				ret.Item2 = controlURL;
			}
		}
		if (ret.Item2 == null)
		{
			throw new Exception("Unsupported Gateway");
		}
		if (!ret.Item2.StartsWith('/'))
		{
			ret.Item2 = "/" + ret.Item2;
		}
		ret.Item2 = location[..location.IndexOf('/', 7)] + ret.Item2;
		return ret;
	}

	private static string BuildArgString((string Key, object Value) arg)
	{
		return $"<{arg.Key}>{arg.Value}</{arg.Key}>";
	}

	private Dictionary<string, string> RunCommand(string action, params (string Key, object Value)[] args)
	{
		string requestData = GetRequestData(action, args);
		return GetResponse(SendRequest(action, requestData));
	}

	private string GetRequestData(string action, (string Key, object Value)[] args)
	{
		return string.Concat("<?xml version=\"1.0\"?>\n", "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">", "<SOAP-ENV:Body>", $"<m:{action} xmlns:m=\"{serviceType}\">", string.Concat(args.Select(BuildArgString)), "</m:" + action + ">", "</SOAP-ENV:Body>", "</SOAP-ENV:Envelope>");
	}

	private HttpWebRequest SendRequest(string action, string requestData)
	{
		byte[] data = Encoding.ASCII.GetBytes(requestData);
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(controlURL);
		request.Method = "POST";
		request.ContentType = "text/xml";
		request.ContentLength = data.Length;
		request.Headers.Add("SOAPAction", $"\"{serviceType}#{action}\"");
		using Stream requestStream = request.GetRequestStream();
		requestStream.Write(data);
		return request;
	}

	private static Dictionary<string, string> GetResponse(HttpWebRequest request)
	{
		Dictionary<string, string> ret = new Dictionary<string, string>();
		try
		{
			using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK)
			{
				return ret;
			}
			foreach (XNode item in XDocument.Load(response.GetResponseStream()).DescendantNodes())
			{
				if (item is XElement { FirstNode: XText txt } ele)
				{
					ret[ele.Name.LocalName] = txt.Value;
				}
			}
		}
		catch
		{
		}
		if (ret.TryGetValue("errorCode", out var errorCode))
		{
			throw new Exception(errorCode);
		}
		return ret;
	}

	public bool SpecificPortMappingExists(ushort externalPort, Protocol protocol)
	{
		return RunCommand("GetSpecificPortMappingEntry", ("NewRemoteHost", ""), ("NewExternalPort", externalPort), ("NewProtocol", protocol)).ContainsKey("NewInternalPort");
	}

	public void AddPortMapping(ushort externalPort, Protocol protocol, ushort? internalPort = null, string description = null)
	{
		RunCommand("AddPortMapping", ("NewRemoteHost", ""), ("NewExternalPort", externalPort), ("NewProtocol", protocol), ("NewInternalClient", InternalClient), ("NewInternalPort", internalPort ?? externalPort), ("NewEnabled", 1), ("NewPortMappingDescription", description ?? "UwUPnP"), ("NewLeaseDuration", 0));
	}

	public void DeletePortMapping(ushort externalPort, Protocol protocol)
	{
		RunCommand("DeletePortMapping", ("NewRemoteHost", ""), ("NewExternalPort", externalPort), ("NewProtocol", protocol));
	}

	/// <summary>2.4.14.GetGenericPortMappingEntry</summary>
	public Dictionary<string, string> GetGenericPortMappingEntry(int portMappingIndex)
	{
		return RunCommand("GetGenericPortMappingEntry", ("NewPortMappingIndex", portMappingIndex));
	}
}
