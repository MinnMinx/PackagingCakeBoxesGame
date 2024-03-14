using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackagingCakeBoxes
{
    [PreferBinarySerialization, Serializable,
    CreateAssetMenu(fileName = "Level_1", menuName = "PackagingCakeBoxes/New Level data...", order = 0)]
    public class LevelData : ScriptableObject
    {
        // Grid setting
        public int ColumnCount;
        public int RowCount;

        // Obstacle setting
        public Vector2Int[] ObstaclesPos;

        // Cake setting
        public Vector2Int[] CakesPos;

        // Gift setting
        public Vector2Int GiftPos;

        /// <summary>
        /// Check if this level's grid setting is valid for playing
        /// </summary>
        /// <returns>True if level's grid is playable, otherwise false</returns>
        public bool isGridValid()
        {
            // Include case which have either row = 1 or col = 1
            //return ColumnCount * RowCount > 1;

            // This make sure there're at least 2 row or col
            return ColumnCount > 1 && RowCount > 1;
        }
    }
}