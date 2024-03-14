using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

namespace PackagingCakeBoxes.UI
{
    public class ResultPlayUIManager : MonoBehaviour
    {
        [SerializeField]
        private Image star1, star2, star3;
        [SerializeField]
        private Sprite starYellowSpr, emptyStarSpr;
        [SerializeField]
        private Button homeBtnWin, homeBtnLose, resetBtnWin, resetBtnLose, nextBtn;
        [SerializeField]
        private LevelManager levelMng;
        [SerializeField]
        private GameObject winGroup, loseGroup;
        [SerializeField]
        private GameObject resultGroupParent;

        private LevelListData allLevelData;
        private LevelInfo nextLevelInfo = null;

        // Start is called before the first frame update
        void Start()
        {
            try
            {
                CheckNextLevel();

                resultGroupParent.SetActive(false);
                homeBtnWin.onClick.AddListener(ReturnLevelChooser);
                homeBtnLose.onClick.AddListener(ReturnLevelChooser);
                resetBtnWin.onClick.AddListener(ResetLevel);
                resetBtnLose.onClick.AddListener(ResetLevel);
                nextBtn.onClick.AddListener(Switch2NextLevel);
            }
            catch
            {
                Debug.LogError("Cannot initialize ResultUIManager");
                Destroy(this);
            }
        }

        private void CheckNextLevel()
        {
            if (allLevelData == null)
                allLevelData = LevelListData.Load();
            int curLevelIndex = allLevelData.IndexOf(GameInfo.Instance.curLevel);
            if (curLevelIndex >= 0 && curLevelIndex < allLevelData.Count - 1)
            {
                nextLevelInfo = allLevelData[curLevelIndex + 1];
                nextBtn.interactable = true;
            }
            else
            {
                nextLevelInfo = null;
                nextBtn.interactable = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            LevelResult result = LevelResult.FAILED;
            if (levelMng != null && !levelMng.IsLevelEnded(out result))
            {
                return;
            }

            if (!resultGroupParent.activeSelf)
            {
                if (result != LevelResult.FAILED)
                    GameInfo.Instance.SetCurLevelResult(result);
                resultGroupParent.SetActive(true);

                star1.sprite = emptyStarSpr;
                star2.sprite = emptyStarSpr;
                star3.sprite = emptyStarSpr;
                if (result == LevelResult.FAILED)
                {
                    winGroup.SetActive(false);
                    loseGroup.SetActive(true);
                }
                else
                {
                    winGroup.SetActive(true);
                    loseGroup.SetActive(false);
                    switch (result)
                    {
                        case LevelResult.STAR_1:
                            star1.sprite = starYellowSpr;
                            break;
                        case LevelResult.STAR_2:
                            star1.sprite = starYellowSpr;
                            star2.sprite = starYellowSpr;
                            break;
                        case LevelResult.STAR_3:
                            star1.sprite = starYellowSpr;
                            star2.sprite = starYellowSpr;
                            star3.sprite = starYellowSpr;
                            break;
                    }
                }
            }
        }

        void ReturnLevelChooser()
        {
            SceneManager.LoadScene(SceneNameConstant.CHOOSE_LEVEL);
        }

        void Switch2NextLevel()
        {
            if (nextLevelInfo == null)
            {
                Debug.LogError("Cannot find level info to switch to next level");
                SceneManager.LoadScene(SceneNameConstant.CHOOSE_LEVEL);
                return;
            }
            // Set next level
            GameInfo.Instance.SetLevel(nextLevelInfo, LevelResult.FAILED);
            CheckNextLevel();
            ResetLevel();
        }

        void ResetLevel()
        {
            resultGroupParent.SetActive(false);
            levelMng.ResetLevel();
        }
    }
}