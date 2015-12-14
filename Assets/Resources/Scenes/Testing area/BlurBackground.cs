using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace com.dogonahorse
{

    public class BlurBackground : MonoBehaviour
    {
       

        public ScreenshotCamera renderCamera;
        // Use this for initialization
            public Image myImage;
        
       void Start(){
            EventManager.ListenForEvent(AzumiEventType.OpenModal, DisplayBlurBackgroundEvent);
             EventManager.ListenForEvent(AzumiEventType.ScreenShotReady, enableBlurBackGround);
             EventManager.ListenForEvent(AzumiEventType.CloseModal, disableBlurBackGround);
               renderCamera.ScreenReadyEvent += GetScreen;
               myImage = GetComponent<Image>();
        }
         void DisplayBlurBackgroundEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
       
              
                renderCamera.TakeScreenshot(0, 0, Screen.width, Screen.height);
       
        }

        // Update is called once per frame
        void GetScreen()
        {

           // Image myImage = GetComponent<Image>();

            LinearBlur myBlur = new LinearBlur();
            Texture2D blurTexture = myBlur.Blur(renderCamera.screenshot, 20, 2);
            blurTexture.Apply();
            myImage.material.mainTexture = blurTexture;

        }
        
          void enableBlurBackGround(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
             GetScreen();
                myImage.enabled = true;
        }
        
         void disableBlurBackGround(AzumiEventType Event_Type, Component Sender, object Param = null )
        {
                myImage.enabled = false;
        }
        void OnDestroy(){
            EventManager.Instance.RemoveEvent(AzumiEventType.OpenModal);
                 EventManager.Instance.RemoveEvent(AzumiEventType.ScreenShotReady);
                      EventManager.Instance.RemoveEvent(AzumiEventType.CloseModal);
        }
    }
}