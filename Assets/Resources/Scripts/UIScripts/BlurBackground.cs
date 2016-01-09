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
        public int blurPixels = 10;
        
         public int downSampleAmount = 4;
        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.OpenModal, DisplayBlurBackgroundEvent);
            EventManager.ListenForEvent(AzumiEventType.ScreenShotReady, enableBlurBackGround);
            EventManager.ListenForEvent(AzumiEventType.BlurFadeOutComplete, disableBlurBackGround);
            
            renderCamera = GameObject.Find("RenderCamera").GetComponent<ScreenshotCamera>();
            // renderCamera.ScreenReadyEvent += GetScreen;
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
            //Texture2D
            Texture2D blurTexture = renderCamera.screenshot;

            TextureScale.Bilinear(blurTexture, blurTexture.width / downSampleAmount, blurTexture.height / downSampleAmount);
            blurTexture = myBlur.Blur(blurTexture, blurPixels, 2);
            blurTexture.Apply();
            myImage.material.mainTexture = blurTexture;

        }

        void enableBlurBackGround(AzumiEventType Event_Type, Component Sender, object Param = null)
        {


            GetScreen();
            myImage.enabled = true;
        }

        void disableBlurBackGround(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            myImage.enabled = false;
        }
        void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(AzumiEventType.OpenModal);
            EventManager.Instance.RemoveEvent(AzumiEventType.ScreenShotReady);
            EventManager.Instance.RemoveEvent(AzumiEventType.BlurFadeOutComplete);
        }
    }
}