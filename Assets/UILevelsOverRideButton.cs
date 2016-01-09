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

                EventManager.PostEvent(AzumiEventType.UnlockAllLevels, this);
            }
            else
            {
                EventManager.PostEvent(AzumiEventType.RelockLevels, this);
            }
        }
        

        // Update is called once per frame
    }
}