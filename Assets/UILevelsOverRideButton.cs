using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{
    public class UILevelsOverRideButton : MonoBehaviour
    {
        public Toggle toggle;

        // Use this for initialization
        public void LockUnlock()
        {
            if (toggle.isOn)
            {
                InputManager.Instance.UnlockLevelButtons();
            }
            else
            {    
                InputManager.Instance.LockLevelButtons();
            }
        }

    }
}