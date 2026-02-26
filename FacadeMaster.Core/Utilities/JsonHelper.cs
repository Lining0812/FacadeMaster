using System;
using System.IO;
using Newtonsoft.Json;

namespace FacadeMaster.Core.Utilities
{
    /// <summary>
    /// Newtonsoft序列化Json
    /// </summary>
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// 序列化Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(obj, settings ?? DefaultSettings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(json, settings ?? DefaultSettings);
        }

        /// <summary>
        /// 序列化并写入至文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        /// <param name="settings"></param>
        public static void SerializeToFile<T>(T obj, string filePath, JsonSerializerSettings settings = null)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = Serialize(obj, settings);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// 从文件读取并反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static T DeserializeFromFile<T>(string filePath, JsonSerializerSettings settings = null)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"JSON file not found: {filePath}");
            }

            var json = File.ReadAllText(filePath);
            return Deserialize<T>(json, settings);
        }

        /// <summary>
        /// 尝试从文件读取并反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="result"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static bool TryDeserializeFromFile<T>(string filePath, out T result, JsonSerializerSettings settings = null)
        {
            result = default(T);
            try
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }

                result = DeserializeFromFile<T>(filePath, settings);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
