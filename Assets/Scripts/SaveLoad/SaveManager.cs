using UnityEngine;

namespace SaveLoad
{
    public class SaveManager : Singleton<SaveManager>
    {
        [SerializeField] private SaveDataSo _saveDataSo;
        
        public void OnSave()
        {
            SaveData saveData = new SaveData(_saveDataSo);
            SerializationManger.Save("save", saveData);
        }

        public bool OnLoad()
        {
            SaveData saveData = (SaveData) SerializationManger.Load(Application.persistentDataPath + "saves/save.save");
            if (saveData != null)
            {
                _saveDataSo = saveData.SaveDataSo;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}