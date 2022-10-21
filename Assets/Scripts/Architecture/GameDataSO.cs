using System.Collections.Generic;
using Scriptable;
using UnityEngine;

namespace Architecture
{
    [CreateAssetMenu(menuName = "Game Data/Game Data")]
    public class GameDataSO : ScriptableObject
    {
        [field: SerializeField] public string Version { get; private set; } = "0.0";

        [field: SerializeField] public List<WorldDataSO> WorldDatas { get; private set; }

        [field: SerializeField] public string WorldsScenePath { get; private set; }

        [field: SerializeField] public string MainMenuScenePath { get; private set; }
    }
}