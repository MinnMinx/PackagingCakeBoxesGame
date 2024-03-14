using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PackagingCakeBoxes
{
    public class LevelListData : ScriptableObject
    {
        public static string FILE_PATH = "Data/LevelsList";
        private static LevelListData instance;

        [SerializeField]
        private List<LevelInfo> data;

        /// <summary>
        /// Get the game's all level list data by loading with constant path
        /// </summary>
        /// <returns>List of all levels in the game</returns>
        public static LevelListData Load()
        {
            if (instance == null)
                instance = Resources.Load<LevelListData>(FILE_PATH);
            return instance;
        }

        public int Count => data.Count;
        public LevelInfo this[int index] => data[index];

        /// <summary>
        /// Find level based on its info and return its index
        /// </summary>
        /// <param name="info">LevelInfo to find</param>
        /// <returns>Index of the level info found, -1 if not found</returns>
        public int IndexOf(LevelInfo info) => data.IndexOf(info);
    }
}