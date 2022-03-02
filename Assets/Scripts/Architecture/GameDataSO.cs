using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game Data/Game Data")]
public class GameDataSO : ScriptableObject
{
    [field: SerializeField]
    public List<WorldDataSO> WorldDatas { get; private set; }
    [field: SerializeField]
    public string WorldsScenePath { get; private set; }
    [field: SerializeField]
    public string MainMenuScenePath { get; private set; }
}