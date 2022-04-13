using UnityEditor;
using UnityEngine;
using World_Level;

namespace Editor
{
    [CustomEditor(typeof(LevelObject), true)]
    public class LevelObjectEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            var level = target as LevelObject;
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(level.ScenePath);

            serializedObject.Update();

            level.levelName = EditorGUILayout.TextField("Level Name", level.levelName);
            level.levelIndex = EditorGUILayout.IntField("Level Index", level.levelIndex);
            level.worldIndex = EditorGUILayout.IntField("World Index", level.worldIndex);
            level.RecordTime = EditorGUILayout.FloatField("Record Time", level.RecordTime);
            level.levelImage = (Sprite)EditorGUILayout.ObjectField("levelImage", level.levelImage, typeof(Sprite), false);

            EditorGUI.BeginChangeCheck();
            var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck())
            {
                var newPath = AssetDatabase.GetAssetPath(newScene);
                var scenePathProperty = serializedObject.FindProperty("ScenePath");
                scenePathProperty.stringValue = newPath;
            }
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(level);
        }
    }
}
