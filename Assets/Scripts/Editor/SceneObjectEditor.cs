using GameManagement;
using UnityEditor;

namespace Editor {
    [CustomEditor(typeof(SceneObject), true)]
    public class SceneObjectEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var currentScene = target as SceneObject;
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(currentScene.ScenePath);

            serializedObject.Update();

            currentScene.SceneName = EditorGUILayout.TextField("SceneName", currentScene.SceneName);
            EditorGUI.BeginChangeCheck();
            var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck()) {
                string newPath = AssetDatabase.GetAssetPath(newScene);
                var scenePathProperty = serializedObject.FindProperty("ScenePath");
                scenePathProperty.stringValue = newPath;
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(currentScene);
        }
    }
}