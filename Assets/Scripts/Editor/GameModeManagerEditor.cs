using Architecture;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(GameModeManager))]
    public class GameModeManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var manager = target as GameModeManager;
            var oldMainMenuScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(manager.MainMenuScenePath);
            var oldWorldsScreenScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(manager.WorldsScenePath);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            var newMainMenuScene = EditorGUILayout.ObjectField("Main Menu Scene", oldMainMenuScene, typeof(SceneAsset), false) as SceneAsset;
            var newWorldScreenScene = EditorGUILayout.ObjectField("Worlds Screen Scene", oldWorldsScreenScene, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck())
            {
                var newPath = AssetDatabase.GetAssetPath(newMainMenuScene);

                var scenePathProperty = serializedObject.FindProperty("MainMenuScenePath");
                scenePathProperty.stringValue = newPath;

                newPath = AssetDatabase.GetAssetPath(newWorldScreenScene);
                scenePathProperty = serializedObject.FindProperty("WorldsScenePath");
                scenePathProperty.stringValue = newPath;
                EditorUtility.SetDirty(manager);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}