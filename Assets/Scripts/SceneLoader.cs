using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonCrawler_HarropCharlie
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string sceneToLoadName;
        [SerializeField] private string mainScene = "MainScene";

        [SerializeField] private TMP_Text scoreText;

        public void LoadThisScene(string sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        public void Restart()
        {
            SceneManager.LoadScene(mainScene);
        }

        private void Start()
        {
            scoreText.text = "Your Score: " + ScoreManager.finalScore.ToString();
        }
    }
}

