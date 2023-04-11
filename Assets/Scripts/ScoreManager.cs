using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler_HarropCharlie
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreTextField;
        public static int score;
        public static int finalScore;

        private void Start()
        {
            score = 0;
        }

        private void Update()
        {
            scoreTextField.text = "Score: " + score.ToString();
        }
    }
}

