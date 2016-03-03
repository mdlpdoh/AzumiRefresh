using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    /// <summary>
    /// This script is used to know how many frames per second the game is running during a level. 
    /// That way we can begin to know if something in the scene is slowing the game down.
    /// </summary>
    public class FPSDisplay : MonoBehaviour
    {
        float deltaTime = 0.0f;
        bool inLevel = false;
        float totalTime = 0.0f;
        int numFrames = 0;
        void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            if (inLevel && Time.timeScale == 1)
            {
                totalTime+=Time.deltaTime;
                numFrames++;
            }
        }
        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.EnterLevel, onEnterLevel);
            EventManager.ListenForEvent(AzumiEventType.LevelWon, onEndLevel);
            EventManager.ListenForEvent(AzumiEventType.LevelLost, onEndLevel);
        }
        
        public float GetAverageFPS(){
            return numFrames/totalTime;
        }
        public void onEnterLevel(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            totalTime = 0;
            numFrames = 0;
            inLevel = true;
        }

        public void onEndLevel(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            inLevel = false;
        }
        /*
      void OnGUI()
      {
          int w = Screen.width, h = Screen.height;

          GUIStyle style = new GUIStyle();

          Rect rect = new Rect(0, 0, w, h * 2 / 100);
          style.alignment = TextAnchor.UpperLeft;
          style.fontSize = h * 2 / 100;
          style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
          float msec = deltaTime * 1000.0f;
          float fps = 1.0f / deltaTime;
          string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
          GUI.Label(rect, text, style);
      }*/
    }
}