using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PackagingCakeBoxes.UI
{
    public class SelectLevelBtnHook : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI levelLabel;
        [SerializeField]
        private Button btn;
        [SerializeField]
        private Image star1, star2, star3, lockImg;
        [SerializeField]
        private Sprite starSpr;
        private static string LEVEL_NAME = "Level {0}";

        /// <summary>
        /// Initialize this button with given level info
        /// </summary>
        /// <param name="lvInfo">Input level's info</param>
        /// <param name="result">Past result of this level</param>
        internal void SetLevelInfo(LevelInfo lvInfo, LevelResult result)
        {
            // If there's no result or is failed
            if (result == LevelResult.FAILED)
            {
                levelLabel.enabled = false;
                lockImg.enabled = true;
            }
            else
            {
                lockImg.enabled = false;
                if (levelLabel != null)
                {
                    // Default is disable level TextLabel
                    levelLabel.text = lvInfo.levelNumber.ToString();
                }
                if (btn != null)
                {
                    // Set onClick event to switch to given scene name
                    btn.onClick.AddListener(() =>
                    {
                        GameInfo.Instance.SetLevel(lvInfo, result);
                        SceneManager.LoadScene(SceneNameConstant.PLAY_LEVEL);
                    });
                }
                SetStarSprite(result);
            }
            // Change gameObject's name
            gameObject.name = string.Format(LEVEL_NAME, lvInfo.levelNumber);
        }

        /// <summary>
        /// Set button's star view based on past result
        /// </summary>
        /// <param name="result">Past result of this level</param>
        private void SetStarSprite(LevelResult result)
        {
            // Default sprite is the empty star so switch to yellow star sprite based on given starCount
            if (result == LevelResult.STAR_0)
                return;

            int resultVal = (int)result;
            if (star1 != null && resultVal >= 1)
            {
                star1.sprite = starSpr;
            }
            if (star2 != null && resultVal >= 2)
            {
                star2.sprite = starSpr;
            }
            if (star3 != null && resultVal >= 3)
            {
                star3.sprite = starSpr;
            }
        }
    }
}