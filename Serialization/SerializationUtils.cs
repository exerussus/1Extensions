using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

namespace Exerussus._1Extensions.Serialization
{
    public static class SerializationUtils
    {
        /// <summary> Глобальные настройки JSON для корректной сериализации полиморфных структур. </summary>
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            Formatting = Formatting.Indented,
        };

        /// <summary> Сериализует объект в JSON или бинарные данные. </summary>
        public static bool TrySerialize(this object value, out SerializationData data, bool forceReflected = false)
        {
            try
            {
                data = Serialize(value, forceReflected);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }

            data = null;
            return false;
        }
        
        /// <summary> Сериализует объект в JSON или бинарные данные. </summary>
        public static SerializationData Serialize(this object value, bool forceReflected = false)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var data = new SerializationData();

            if (forceReflected)
            {
                using var stream = new MemoryStream();
#pragma warning disable SYSLIB0011
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
#pragma warning restore SYSLIB0011
                data.binaryData = stream.ToArray();
            }
            else
            {
                data.jsonData = JsonConvert.SerializeObject(value, JsonSettings);
            }

            return data;
        }

        /// <summary> Десериализует объект из SerializationData. </summary>
        public static T Deserialize<T>(this SerializationData data, bool forceReflected = false)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (forceReflected)
            {
                using var stream = new MemoryStream(data.binaryData);
#pragma warning disable SYSLIB0011
                var formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(data.jsonData, JsonSettings);
            }
        }
    }

    /// <summary> Класс для хранения сериализованных данных. </summary>
    [Serializable]
    public class SerializationData
    {
        public string jsonData;
        public byte[] binaryData;

        public override string ToString()
        {
            return jsonData;
        }
    }
}
