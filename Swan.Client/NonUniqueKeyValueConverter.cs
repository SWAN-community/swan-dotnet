using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Swan.Client
{
    internal class NonUniqueKeyValueConverter: 
        JsonConverter<IList<KeyValuePair<string, string>>>
    {
        public override IList<KeyValuePair<string, string>> Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var collection = new List<KeyValuePair<string, string>>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return collection;
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                var property = reader.GetString();
                reader.Read();
                switch(reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        
                        break;
                }
                var value = reader.GetString();
                collection.Add(new KeyValuePair<string, string>(property, value));
            }

            throw new JsonException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            IList<KeyValuePair<string, string>> value, 
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
