using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PackagingCakeBoxes.UI {
    public class SelectLevelPanelManager : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField]
        private int columnObjCount = 3;
        [SerializeField]
        private int rowObjCount = 3;
        [Header("Asset References")]
        [SerializeField]
        private GameObject btnPrefab;
        [Header("Scene References")]
        [SerializeField]
        private RectTransform panelRect;
        [SerializeField]
        private GridLayoutGroup panelGridLayout;
        [SerializeField]
        private Button backBtn;

        private void Start()
        {
            // Get all level list
            LevelListData levelList = LevelListData.Load();
            // Spawn btn with corresponding data
            if (levelList != null)
            {
                SpawnBtns(levelList);
            }
            // Set up grid layout
            SetUpLayout();
            // Assign back btn
            if (backBtn != null)
            {
                backBtn.onClick.AddListener(() => SceneManager.LoadScene(SceneNameConstant.MAIN_MENU));
            }
        }

        /// <summary>
        /// Spawn buttons for all levels in the game
        /// </summary>
        /// <param name="levelList">All levels list in the game</param>
        private void SpawnBtns(LevelListData levelList)
        {
            bool isLevelAfterLocked = false;
            for (int i = 0; i < levelList.Count; i++)
            {
                var levelInfo = levelList[i];
                GameObject spawnedBtn = Instantiate(btnPrefab, panelRect.transform);
                SelectLevelBtnHook objHook = spawnedBtn.GetComponent<SelectLevelBtnHook>();
                // Check if the component and the data is valid
                if (objHook == null || levelInfo == null ||
                    levelInfo.levelData == null || !levelInfo.levelData.isGridValid())
                {
                    Destroy(spawnedBtn);
                }
                else
                {
                    // Default for all levels are failed except the first level
                    var result = i > 0 ? LevelResult.FAILED : LevelResult.STAR_0;
                    // Check if level is locked
                    if (!isLevelAfterLocked)
                    {
                        result = GameInfo.GetLevelResult(levelInfo);
                        if (result == LevelResult.FAILED)
                        {
                            isLevelAfterLocked = true;
                            result = LevelResult.STAR_0;
                        }
                    }
                    objHook.SetLevelInfo(levelInfo, result);
                }
            }
        }

        /// <summary>
        /// Set GridLayoutGroup for automatically align buttons
        /// </summary>
        private void SetUpLayout()
        {
            if (panelGridLayout != null && panelRect != null &&
                columnObjCount > 0 && rowObjCount > 0)
            {
                panelGridLayout.cellSize = new Vector2(panelRect.rect.width / columnObjCount,
                                                       panelRect.rect.height / rowObjCount);
            }
        }
    }
}