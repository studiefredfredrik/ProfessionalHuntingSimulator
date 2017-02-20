using System.Xml.Serialization;
using System.IO;

public static class XmlTools
{
	public static string ToXmlString<T>(this T input)
	{
		using (var writer = new StringWriter())
		{
			input.ToXml(writer);
			return writer.ToString();
		}
	}

	public static void ToXml<T>(this T objectToSerialize, Stream stream)
	{
		new XmlSerializer(typeof(T)).Serialize(stream, objectToSerialize);
	}

	public static void ToXml<T>(this T objectToSerialize, StringWriter writer)
	{
		new XmlSerializer(typeof(T)).Serialize(writer, objectToSerialize);
	}
}


