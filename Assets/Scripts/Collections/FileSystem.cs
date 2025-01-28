using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Collections
{
    public static class FileSystem
    {
        public static T Load<T>(string fileName)
        {
            var filePath = Application.persistentDataPath + "/" + fileName;
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }

            throw new Exception($"File Load Error : {filePath}");
        }

        public static bool Save<T>(string fileName, T saveData)
        {
            var jsonSaveData = JsonConvert.SerializeObject(saveData);
            var filePath = Application.persistentDataPath + "/" + fileName;
            Debug.Log(filePath);
            File.WriteAllText(filePath, jsonSaveData);
            return true;
        }

        public static string[] GetSaveFileNames()
        {
            var path = Application.persistentDataPath + "/";
            if (Directory.Exists(path)) return Directory.GetFiles(path).Select(it => Path.GetFileName(it)).ToArray();
            return Array.Empty<string>();
        }
    }
}