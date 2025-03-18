
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Exerussus._1Extensions.Serialization
{
    public static class SerializationUtils
    {
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
                using (MemoryStream stream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, value);
                    data.BinaryData = stream.ToArray();
                }
            }
            else
            {
                data.JsonData = JsonConvert.SerializeObject(value, Formatting.None);
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
                using (MemoryStream stream = new MemoryStream(data.BinaryData))
                {
                    var formatter = new BinaryFormatter();
                    return (T)formatter.Deserialize(stream);
                }
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(data.JsonData);
            }
        }
    }

    /// <summary> Класс для хранения сериализованных данных. </summary>
    [Serializable]
    public class SerializationData
    {
        public string JsonData;
        public byte[] BinaryData;
    }
}