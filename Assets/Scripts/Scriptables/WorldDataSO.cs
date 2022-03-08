using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game Data/World Data")]
public class WorldDataSO : ScriptableObject
{
    public string WorldName;
    public List<LevelDataSO> LevelDatas;
}