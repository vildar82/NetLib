﻿namespace NetLib
{
    using System.IO;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    [PublicAPI]
    public static class JsonExt
    {
        public static T Deserialize<T>([NotNull] this string file)
        {
            var bsJson = ReadTextFile(file);
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.DeserializeObject<T>(bsJson, settings);
        }

        public static T FromJson<T>([NotNull] this string json)
        {
            return json.Deserialize<T>();
        }

        [NotNull]
        public static string ToJson<T>(this T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        [NotNull]
        public static string ToJson<T>(this T item, bool pretty)
        {
            return JsonConvert.SerializeObject(item, pretty ? Formatting.Indented : Formatting.None);
        }

        public static void Serialize<T>(this T item, [NotNull] string file)
        {
            var json = ToJson(item);
            WriteText(file, json);
        }

        public static void Serialize<T>(this T item, [NotNull] string file, bool pretty)
        {
            var json = ToJson(item, pretty);
            WriteText(file, json);
        }

        private static void WriteText([NotNull] string file, string json)
        {
            using var sw = new StreamWriter(file, false);
            sw.Write(json);
        }

        [NotNull]
        private static string ReadTextFile([NotNull] string filePath)
        {
            var tempFile = Path.GetTempFileName();
            File.Copy(filePath, tempFile, true);
            var text = File.ReadAllText(tempFile);
            IO.Path.TryDeleteFile(tempFile);
            return text;
        }
    }
}
