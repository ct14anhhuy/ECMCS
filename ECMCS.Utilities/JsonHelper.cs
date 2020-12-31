using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ECMCS.Utilities
{
    public static class JsonHelper
    {
        private static readonly string _jsonFile = $"{ConfigHelper.Read("SaveFilePath.Root")}{ConfigHelper.Read("SaveFilePath.Monitor")}{ConfigHelper.Read("JsonFileName")}";

        public static List<TEntity> Get<TEntity>(Func<TEntity, bool> condition = null)
        {
            List<TEntity> objs;
            using (StreamReader sr = new StreamReader(_jsonFile))
            {
                string json = sr.ReadToEnd();
                objs = JsonConvert.DeserializeObject<List<TEntity>>(json);
                if (objs == null || condition == null)
                {
                    return objs;
                }
                else
                {
                    return objs.Where(condition).ToList();
                }
            }
        }

        public static void Add<TEntity>(TEntity entity) where TEntity : class
        {
            string newJson;
            using (StreamReader sr = new StreamReader(_jsonFile))
            {
                string json = sr.ReadToEnd();
                List<TEntity> objs = JsonConvert.DeserializeObject<List<TEntity>>(json);
                if (objs == null)
                {
                    objs = new List<TEntity>();
                }
                objs.Add(entity);
                newJson = JsonConvert.SerializeObject(objs);
            }
            File.WriteAllText(_jsonFile, newJson);
        }

        public static void Update<TEntity>(TEntity entity, Predicate<TEntity> match) where TEntity : class
        {
            string newJson;
            using (StreamReader sr = new StreamReader(_jsonFile))
            {
                string json = sr.ReadToEnd();
                List<TEntity> objs = JsonConvert.DeserializeObject<List<TEntity>>(json);
                if (objs == null)
                {
                    return;
                }
                int idx = objs.FindIndex(match);
                if (idx >= 0)
                {
                    objs[idx] = entity;
                }
                newJson = JsonConvert.SerializeObject(objs);
            }
            File.WriteAllText(_jsonFile, newJson);
        }

        public static void Remove<TEntity>(Predicate<TEntity> match) where TEntity : class
        {
            string newJson;
            using (StreamReader sr = new StreamReader(_jsonFile))
            {
                string json = sr.ReadToEnd();
                List<TEntity> objs = JsonConvert.DeserializeObject<List<TEntity>>(json);
                if (objs == null)
                {
                    return;
                }
                int idx = objs.FindIndex(match);
                if (idx >= 0)
                {
                    objs.RemoveAt(idx);
                }
                newJson = JsonConvert.SerializeObject(objs);
            }
            File.WriteAllText(_jsonFile, newJson);
        }
    }
}