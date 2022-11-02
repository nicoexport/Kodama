using Scriptable;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(LevelDataSO), true)]
    public class LevelDataSOEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var level = target as LevelDataSO;
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(level.ScenePath);

            serializedObject.Update();

            level.LevelName = EditorGUILayout.TextField("Level Name", level.LevelName);
            level.RecordTime = EditorGUILayout.FloatField("Record Time", level.RecordTime);
            level.LevelImage =
                (Sprite)EditorGUILayout.ObjectField("Level Image", level.LevelImage, typeof(Sprite), false);
            level.Visited = EditorGUILayout.Toggle("Visited", level.Visited);
            level.Completed = EditorGUILayout.Toggle("Completed", level.Completed);
            level.LevelMusicAudioCueSo = (AudioCueSo)EditorGUILayout.ObjectField("Level Music",
                level.LevelMusicAudioCueSo, typeof(AudioCueSo), false);


            EditorGUI.BeginChangeCheck();
            var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck()) {
                string newPath = AssetDatabase.GetAssetPath(newScene);
                var scenePathProperty = serializedObject.FindProperty("ScenePath");
                scenePathProperty.stringValue = newPath;
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(level);
        }
    }
}