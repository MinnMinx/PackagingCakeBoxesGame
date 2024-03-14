using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PackagingCakeBoxes
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField]
        private Button startBtn, howToBtn, closeHowToBtn;
        [SerializeField]
        private GameObject howToPanel;

        // Start is called before the first frame update
        void Start()
        {
            howToPanel.SetActive(false);
            startBtn.onClick.AddListener(() => SceneManager.LoadScene(SceneNameConstant.CHOOSE_LEVEL));
            howToBtn.onClick.AddListener(() => howToPanel.SetActive(true));
            closeHowToBtn.onClick.AddListener(() => howToPanel.SetActive(false));
        }
    }
}