namespace NetLib
{
    using System.IO;
    using Newtonsoft.Json;

    public static class JsonExt
    {
        public static T Deserialize<T>(this string file)
        {
            var bsJson = ReadTextFile(file);
            return JsonConvert.DeserializeObject<T>(bsJson);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson<T>(this T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public static string ToJson<T>(this T item, bool pretty)
        {
            return JsonConvert.SerializeObject(item, pretty ? Formatting.Indented : Formatting.None);
        }

        public static void Serialize<T>(this T item, string file)
        {
            var json = ToJson(item);
            WriteText(file, json);
        }

        public static void Serialize<T>(this T item, string file, bool pretty)
        {
            var json = ToJson(item, pretty);
            WriteText(file, json);
        }

        private static void WriteText(string file, string json)
        {
            CreateDir(file);
            using var sw = new StreamWriter(file, false);
            sw.Write(json);
        }

        private static void CreateDir(string file)
        {
            var dir = Path.GetDirectoryName(file);
            if (Directory.Exists(file))
                return;
            Directory.CreateDirectory(dir);
        }

        private static string ReadTextFile(string filePath)
        {
            var tempFile = Path.GetTempFileName();
            File.Copy(filePath, tempFile, true);
            var text = File.ReadAllText(tempFile);
            IO.Path.TryDeleteFile(tempFile);
            return text;
        }
    }
}
