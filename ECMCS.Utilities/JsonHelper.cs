using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace ECMCS.Utilities
{
    public static class JsonHelper
    {
        public static JObject Read(string filePath)
        {
            filePath = AppDomain.CurrentDomain.BaseDirectory + filePath;
            using (StreamReader file = File.OpenText(filePath))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject jObject = (JObject)JToken.ReadFrom(reader);
                    return jObject;
                }
            }
        }

        public static void Write(string filePath, string key, object value)
        {
            filePath = AppDomain.CurrentDomain.BaseDirectory + filePath;
            JObject jOject = new JObject(new JProperty(key, value));
            File.WriteAllText(filePath, jOject.ToString());
            using (StreamWriter file = File.CreateText(filePath))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    jOject.WriteTo(writer);
                }
            }
        }

        public static void AddObjectsToJson<T>(string filePath, T entity)
        {
            string newJson;
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                List<T> objs = JsonConvert.DeserializeObject<List<T>>(json);
                if (objs == null)
                {
                    objs = new List<T>();
                }
                objs.Add(entity);
                newJson = JsonConvert.SerializeObject(objs);
            }
            File.WriteAllText(filePath, newJson);
        }
    }
}