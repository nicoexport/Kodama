using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveLoad
{
    public class SerializationManger
    {
        public static bool Save(string saveName, object saveData)
        {
            var formatter = GetBinaryFormatter();

            if (!Directory.Exists(Application.persistentDataPath + "/saves"))
                Directory.CreateDirectory(Application.persistentDataPath + "/saves");

            var path = Application.persistentDataPath + "/saves/" + saveName + ".save";

            var file = File.Create(path);
            formatter.Serialize(file, saveData);
            file.Close();

            return true;
        }

        public static object Load(string path)
        {
            if (!File.Exists(path))
                return null;

            var formatter = GetBinaryFormatter();
            var file = File.Open(path, FileMode.Open);

            try
            {
                var save = formatter.Deserialize(file);
                file.Close();
                return save;
            }
            catch
            {
                Debug.LogErrorFormat("Failed to load file at {0}", path);
                file.Close();
                return null;
            }
        }


        private static BinaryFormatter GetBinaryFormatter()
        {
            var formatter = new BinaryFormatter();

            return formatter;
        }
    }
}