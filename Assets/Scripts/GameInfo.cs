using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackagingCakeBoxes
{
    public class GameInfo
    {
        public LevelInfo curLevel { get; private set; }
        public LevelResult curLevelResult { get; private set; }
        public int showHowToCount { get; private set; }

        private static string HOWTO_KEY = "showHowTo";
        private static string LEVELRESULT_KEY = "Level_{0}";
        private static int TOTAL_HOWTO_SHOW = 3;

        /// <summary>
        /// Get saved level result of past play
        /// </summary>
        /// <param name="info">LevelInfo to find</param>
        /// <returns>Past result of given levelInfo, if player never play this level, return FAILED result</returns>
        public static LevelResult GetLevelResult(LevelInfo info)
        {
            var defaultResult = LevelResult.FAILED;
            int levelIndex;
            LevelListData allLevel = LevelListData.Load();
            // Not found level
            if (allLevel == null || (levelIndex = allLevel.IndexOf(info)) < 0)
                return defaultResult;
            string levelKey = string.Format(LEVELRESULT_KEY, levelIndex);
            // Cannot find saved result of this level
            if (!PlayerPrefs.HasKey(levelKey))
                return defaultResult;
            return (LevelResult)PlayerPrefs.GetInt(levelKey);
        }

        /// <summary>
        /// Save current level's result
        /// </summary>
        /// <param name="levelResult">Result of this level</param>
        public void SetCurLevelResult(LevelResult levelResult)
        {
            if (levelResult < curLevelResult)
                return;

            LevelListData allLevel = LevelListData.Load();
            int levelIndex = allLevel.IndexOf(curLevel);
            if (levelIndex < 0)
            {
                Debug.LogError("Cannot save level's result because level info is not found");
                return;
            }
            PlayerPrefs.SetInt(string.Format(LEVELRESULT_KEY, levelIndex), (int)levelResult);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Reset Game infos
        /// </summary>
        public void Reset()
        {
            curLevel = null;
            curLevelResult = LevelResult.FAILED;
            showHowToCount = 0;
        }

        /// <summary>
        /// Set level info for this Game, with given past result
        /// </summary>
        /// <param name="levelInfo">Input level info</param>
        /// <param name="pastResult">Past result of given level</param>
        public void SetLevel(LevelInfo levelInfo, LevelResult pastResult)
        {
            curLevel = levelInfo;
            curLevelResult = pastResult;
            CheckShowHowTo();
        }

        /// <summary>
        /// Call to increase 1 to shownHowTo
        /// </summary>
        public void IncreaseShownHowTo()
        {
            if (showHowToCount >= TOTAL_HOWTO_SHOW)
                return;

            showHowToCount++;
            PlayerPrefs.SetInt(HOWTO_KEY, showHowToCount);
            PlayerPrefs.Save();
        }

        private void CheckShowHowTo()
        {
            if (!PlayerPrefs.HasKey(HOWTO_KEY))
            {
                showHowToCount = 0;
                PlayerPrefs.SetInt(HOWTO_KEY, showHowToCount);
                PlayerPrefs.Save();
            }
            else
            {
                showHowToCount = PlayerPrefs.GetInt(HOWTO_KEY);
            }
        }

        #region Singleton Implement
        private static GameInfo _instance;
        public static GameInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameInfo();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        #endregion
    }
}