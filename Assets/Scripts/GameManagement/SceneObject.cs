using UnityEngine;

namespace Kodama.GameManagement {
    [CreateAssetMenu(menuName = "SceneObjects/SceneObject")]
    public class SceneObject : ScriptableObject {
        public string SceneName;
        public string ScenePath;
    }
}