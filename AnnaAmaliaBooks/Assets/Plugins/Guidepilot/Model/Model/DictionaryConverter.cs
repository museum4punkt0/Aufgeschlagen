
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DictionaryConverter : JsonConverter
{
    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        IDictionary<string, object> result;

        if (reader.TokenType == JsonToken.StartArray)
        {
            JArray legacyArray = (JArray)JArray.ReadFrom(reader);

            result = (IDictionary<string, object>)legacyArray.ToDictionary(
                el => el["Key"].ToString(),
                el => el["Value"]);
        }
        else
        {
            result =
                (IDictionary<string, object>)
                    serializer.Deserialize(reader, typeof(IDictionary<string, object>));
        }

        return result;
    }

    public override void WriteJson(
        JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(IDictionary<string, object>).IsAssignableFrom(objectType);
    }

    public override bool CanWrite
    {
        get { return false; }
    }

// unused alternative way to get the data. Think about it! Test it!
    private static object deserializeToDictionaryOrList(string jo, bool isArray = false)
    {
        if (!isArray)
        {
            isArray = jo.Substring(0, 1) == "[";
        }

        if (isArray)
        {
            var values = JsonConvert.DeserializeObject<List<object>>(jo);
            var returnValues = new List<object>();
            foreach (var d in values)
            {
                if (d is JObject)
                {
                    returnValues.Add(deserializeToDictionaryOrList(d.ToString()));
                }
                else if (d is JArray)
                {
                    returnValues.Add(deserializeToDictionaryOrList(d.ToString(), true));
                }
                else
                {
                    returnValues.Add(d);
                }
            }
            return returnValues;
        }
        else
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jo);
            var returnValues = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> d in values)
            {
                if (d.Value is JObject)
                {
                    returnValues.Add(d.Key, deserializeToDictionaryOrList(d.Value.ToString()));
                }
                else if (d.Value is JArray)
                {
                    returnValues.Add(d.Key, deserializeToDictionaryOrList(d.Value.ToString(), true));
                }
                else
                {
                    returnValues.Add(d.Key, d.Value);
                }
            }
            return returnValues;
        }
    }

}

