using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace com.dogonahorse
{
    public class UILevelResultsModalBorder : MonoBehaviour
    {
  
 
        private ScoreManager scoreManager;
        void Start()
        {

            scoreManager=GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

            GetComponent<Outline>().effectColor = scoreManager.ChapterMainColor;
        }

   
    }
}