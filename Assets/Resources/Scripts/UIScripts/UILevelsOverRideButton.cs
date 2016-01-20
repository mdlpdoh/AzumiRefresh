using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{
    public class UILevelsOverRideButton : MonoBehaviour
    {
        public Toggle toggle;


     public void OnEnable()
        {
            print ("============OnEnable " +InputManager.Instance.LevelProgressOverride );
            if(InputManager.Instance.LevelProgressOverride) {
                toggle.isOn = true;
            } else {
                toggle.isOn = false;
            }
        }
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