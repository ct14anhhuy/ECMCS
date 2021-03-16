using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ECMCS.Utilities.FileFolderExtensions
{
    public class JsonHelper
    {
        public readonly string _jsonFile;

        public JsonHelper(int jsonIndex = CommonConstants.JSON_FILE)
        {
            switch (jsonIndex)
            {
                case CommonConstants.JSON_FILE:
                    _jsonFile = $"{SystemParams.FILE_PATH_ROOT}{SystemParams.FILE_PATH_MONITOR}{SystemParams.JSON_FILES}";
                    break;

                case CommonConstants.USER_FILE:
                    _jsonFile = $"{SystemParams.FILE_PATH_ROOT}{SystemParams.FILE_PATH_MONITOR}{SystemParams.JSON_USERS}";
                    break;

                default:
                    break;
            }
        }

        public List<TEntity> Get<TEntity>(Func<TEntity, bool> condition = null)
        {
            List<TEntity> objs;
            using (StreamReader sr = new StreamReader(_jsonFile))
            {
                string json = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
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
                sr.Close();
                return null;
            }
        }

        public void AddDefault<TEntity>(TEntity entity)
        {
            string newJson;
            using (StreamReader sr = new StreamReader(_jsonFile))
            {
                List<TEntity> objs = new List<TEntity> { entity };
                newJson = JsonConvert.SerializeObject(objs, Formatting.Indented);
                sr.Close();
            }
            File.WriteAllText(_jsonFile, newJson);
        }

        public void Add<TEntity>(TEntity entity)
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
                newJson = JsonConvert.SerializeObject(objs, Formatting.Indented);
                sr.Close();
            }
            File.WriteAllText(_jsonFile, newJson);
        }

        public void Update<TEntity>(TEntity entity, Predicate<TEntity> match)
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
                sr.Close();
            }
            File.WriteAllText(_jsonFile, newJson);
        }

        public void Remove<TEntity>(Predicate<TEntity> match)
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
                sr.Close();
            }
            File.WriteAllText(_jsonFile, newJson);
        }

        public void RemoveAll()
        {
            string newJson = "";
            File.WriteAllText(_jsonFile, newJson);
        }
    }
}