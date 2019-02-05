﻿namespace NetLib
{
    using System.IO;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    [PublicAPI]
    public static class JsonExt
    {
        public static T Deserialize<T>([NotNull] this string file)
        {
            var bsJson = ReadTextFile(file);
            return JsonConvert.DeserializeObject<T>(bsJson);
        }

        public static T FromJson<T>([NotNull] this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        [NotNull]
        public static string ToJson<T>(this T item)
        {
            return JsonConvert.SerializeObject(item, Formatting.Indented);
        }

        public static void Serialize<T>(this T item, [NotNull] string file)
        {
            var json = JsonConvert.SerializeObject(item, Formatting.Indented);
            WriteText(file, json);
        }

        private static void WriteText([NotNull] string file, string json)
        {
            using (var sw = new StreamWriter(file, false))
            {
                sw.Write(json);
            }
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
