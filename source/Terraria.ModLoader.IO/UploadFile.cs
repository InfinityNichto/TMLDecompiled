using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Terraria.ModLoader.IO;

public class UploadFile
{
	public string Name { get; set; }

	public string Filename { get; set; }

	public string ContentType { get; set; }

	public byte[] Content { get; set; }

	public UploadFile()
	{
		ContentType = "application/octet-stream";
	}

	public static byte[] UploadFiles(string address, IEnumerable<UploadFile> files, NameValueCollection values)
	{
		WebRequest request = WebRequest.Create(address);
		request.Method = "POST";
		string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
		request.ContentType = "multipart/form-data; boundary=" + boundary;
		boundary = "--" + boundary;
		using (Stream requestStream = request.GetRequestStream())
		{
			WriteValues(requestStream, values, boundary);
			WriteFiles(requestStream, files, boundary);
			byte[] boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
			requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
		}
		using WebResponse response = request.GetResponse();
		using Stream responseStream = response.GetResponseStream();
		using MemoryStream stream = new MemoryStream();
		responseStream.CopyTo(stream);
		return stream.ToArray();
	}

	public static byte[] GetUploadFilesRequestData(IEnumerable<UploadFile> files, NameValueCollection values, string boundary)
	{
		boundary = "--" + boundary;
		using MemoryStream requestStream = new MemoryStream();
		WriteValues(requestStream, values, boundary);
		WriteFiles(requestStream, files, boundary);
		byte[] boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
		requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
		return requestStream.ToArray();
	}

	private static void WriteValues(Stream requestStream, NameValueCollection values, string boundary)
	{
		if (values == null)
		{
			return;
		}
		foreach (string name in values.Keys)
		{
			byte[] buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
			requestStream.Write(buffer, 0, buffer.Length);
			buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
			requestStream.Write(buffer, 0, buffer.Length);
			buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
			requestStream.Write(buffer, 0, buffer.Length);
		}
	}

	private static void WriteFiles(Stream requestStream, IEnumerable<UploadFile> files, string boundary)
	{
		if (files == null)
		{
			return;
		}
		foreach (UploadFile file in files)
		{
			byte[] buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
			requestStream.Write(buffer, 0, buffer.Length);
			buffer = Encoding.UTF8.GetBytes($"Content-Disposition: form-data; name=\"{file.Name}\"; filename=\"{file.Filename}\"{Environment.NewLine}");
			requestStream.Write(buffer, 0, buffer.Length);
			buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
			requestStream.Write(buffer, 0, buffer.Length);
			requestStream.Write(file.Content, 0, file.Content.Length);
			buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
			requestStream.Write(buffer, 0, buffer.Length);
		}
	}
}
