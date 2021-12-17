using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelObject), true)]
public class LevelObjectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var level = target as LevelObject;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(level.scenePath);

        serializedObject.Update();

        level.levelName = EditorGUILayout.TextField("levelName", level.levelName);
        level.levelIndex = EditorGUILayout.IntField("levelIndex", level.levelIndex);
        level.levelImage = (Sprite)EditorGUILayout.ObjectField("levelImage", level.levelImage, typeof(Sprite), false);

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("scenePath");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(level);
    }
}
