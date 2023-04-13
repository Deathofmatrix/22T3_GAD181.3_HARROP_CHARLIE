using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class EventManager : MonoBehaviour
    {
        public delegate void SwitchToGolem();
        public static event SwitchToGolem SwitchedToGolem;

        public delegate void SwitchFromGolem();
        public static event SwitchToGolem SwitchedFromGolem;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SwitchedToGolem();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                SwitchedFromGolem();
            }
        }
    }
}

