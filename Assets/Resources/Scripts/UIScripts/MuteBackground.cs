using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace com.dogonahorse
{

    public class MuteBackground : MonoBehaviour
    {


     
        // Use this for initialization
        public Image myImage;

        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.ScreenShotReady, enableBlurBackGround);
            EventManager.ListenForEvent(AzumiEventType.BlurFadeOutComplete, disableBlurBackGround);  
            // renderCamera.ScreenReadyEvent += GetScreen;
            myImage = GetComponent<Image>();
        }
      

        void enableBlurBackGround(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
     
            myImage.enabled = true;
        }

        void disableBlurBackGround(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            myImage.enabled = false;
        }
        void OnDestroy()
        {
   
            EventManager.Instance.RemoveEvent(AzumiEventType.ScreenShotReady);
            EventManager.Instance.RemoveEvent(AzumiEventType.BlurFadeOutComplete);
        }
    }
}