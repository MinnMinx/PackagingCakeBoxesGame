using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PackagingCakeBoxes
{
    public class PlayLevelUIManager : MonoBehaviour
    {
        [SerializeField]
        private Button homeBtn, resetBtn;
        [SerializeField]
        private TextMeshProUGUI timerTxt;
        [SerializeField]
        private LevelManager levelMng;

        private static string TIMER_FORMAT = "{0:00}:{1:00}";

        // Start is called before the first frame update
        void Start()
        {
            if (levelMng == null)
            {
                Debug.Log("Cannot initialize PlayLevel UI because missing LevelManager");
                Destroy(this);
                return;
            }
            homeBtn.onClick.AddListener(() => SceneManager.LoadScene(SceneNameConstant.CHOOSE_LEVEL));
            resetBtn.onClick.AddListener(levelMng.ResetLevel);
        }

        // Update is called once per frame
        void Update()
        {
            if (timerTxt != null && levelMng != null)
            {
                int minute = (int)(levelMng.TimeLeft / 60);
                int sec = Mathf.RoundToInt(levelMng.TimeLeft - minute * 60);
                timerTxt.text = string.Format(TIMER_FORMAT, minute, sec);
            }
        }
    }
}