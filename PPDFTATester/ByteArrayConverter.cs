using System;
using Newtonsoft.Json;

namespace TAServiceTester
{
    /// <summary>
    /// Forces Newtonsoft.Json to serialize byte[] as [37, 80, 68, 70...]
    /// instead of default Base64 string "JVBERi0x..."
    /// WCF DataContractJsonSerializer rejects Base64 string for byte[] fields
    /// but accepts integer array format
    /// </summary>
    public class ByteArrayAsIntArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(byte[]);

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            if (value == null) { writer.WriteNull(); return; }

            byte[] bytes = (byte[])value;
            writer.WriteStartArray();
            foreach (byte b in bytes)
                writer.WriteValue(b);  // ← writes as integer, not Base64
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
            => throw new NotImplementedException();
    }
}
