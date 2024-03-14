using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PackagingCakeBoxes.Editor
{
    public class PlayerPrefEditor : UnityEditor.Editor
    {
        [MenuItem("PackagingCakeBoxes/Clear all player's saved data", false, 1)]
        public static void ClearAllData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Cleared all saved data");
        }
    }
}