using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad
{
    [System.Serializable]
    public class SaveData
    {
        public SaveData(SaveDataSo saveDataSo)
        {
            this.worldDatas = saveDataSo.WorldDatas;
        }
        
        Dictionary<string, string> = new Dictionary<string, string>();
        public SaveDataSo SaveDataSo;

        public List<WorldData> worldDatas;
    }
}