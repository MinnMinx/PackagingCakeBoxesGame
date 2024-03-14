using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PackagingCakeBoxes.Editor
{
    public class LevelListDataEditor : UnityEditor.Editor
    {
        [MenuItem("PackagingCakeBoxes/Create all levels list data...", false, 0)]
        public static void CreateAsset()
        {
            string assetPath = "Assets/Resources/" + LevelListData.FILE_PATH + ".asset";
            var existedData = AssetDatabase.LoadAssetAtPath<LevelListData>(assetPath);
            if (existedData != null)
            {
                // Asset is already existed
                Debug.Log("There can only 1 instance of LevelListData in the project");
            }
            else
            {
                // Create new instance of scriptable object
                existedData = CreateInstance<LevelListData>();
                AssetDatabase.CreateAsset(existedData, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            Selection.activeObject = existedData;
        }
    }
}