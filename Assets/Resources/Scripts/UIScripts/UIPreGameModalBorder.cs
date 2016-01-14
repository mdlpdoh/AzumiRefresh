using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace com.dogonahorse
{
    public class UIPreGameModalBorder : MonoBehaviour
    {
          private Outline outline;
        // Use this for initialization
     void Awake()
        {
            outline = GetComponent<Outline>();
        }

      void OnEnable()
        {

          outline.effectColor = LevelManager.GetChapterMainColor(UIChapterPanel.ActiveChapter);
        }
    }
}