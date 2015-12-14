using UnityEngine;
using System.Collections;
namespace com.dogonahorse
{

    public class ScreenshotCamera : MonoBehaviour
    {

        public delegate void ScreenReadyEventDelegate();

        public event ScreenReadyEventDelegate ScreenReadyEvent;

        public Texture2D screenshot { get; private set; }

        private bool capturing = false;

        private Rect captureRect;

        private int oldAntiAliasingSettings;

        private int noAACountdown;

   
        public void TakeScreenshot(float startX, float startY, float endX, float endY)
        {
            oldAntiAliasingSettings = QualitySettings.antiAliasing;
            QualitySettings.antiAliasing = 0;
            captureRect = new Rect(startX, startY, endX - startX, endY - startY);
            noAACountdown = 2;
            capturing = true;
        }

        IEnumerator OnPostRender()
        {
            yield return new WaitForEndOfFrame();
            if (capturing)
            {
                noAACountdown--;
                if (noAACountdown > 0)
                    return true;

                screenshot = new Texture2D(Mathf.RoundToInt(captureRect.width), Mathf.RoundToInt(captureRect.height), TextureFormat.ARGB32, false);
                screenshot.ReadPixels(captureRect, 0, 0, false);
                screenshot.Apply();

                capturing = false;

                QualitySettings.antiAliasing = oldAntiAliasingSettings;

                if (ScreenReadyEvent != null)
                {
                    EventManager.PostEvent(AzumiEventType.ScreenShotReady, this, null);
                }

                //ScreenReadyEvent ();
            }
        }
    }
}
