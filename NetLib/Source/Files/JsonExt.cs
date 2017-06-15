using System.IO;
using Newtonsoft.Json;

namespace NetLib
{
	public static class JsonExt
	{
		public static T JsonDeserialize<T>(this string file)
		{
			var bsJson = File.ReadAllText(file);
			return JsonConvert.DeserializeObject<T>(bsJson);
		}

		public static void JsonSerialize<T>(this T item, string file)
		{
			var json = JsonConvert.SerializeObject(item);
			File.WriteAllText(file, json);
		}
	}
}
