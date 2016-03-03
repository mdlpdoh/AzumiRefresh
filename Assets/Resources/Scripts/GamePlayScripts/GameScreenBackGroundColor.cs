using UnityEngine;
using System.Collections;


namespace com.dogonahorse
{
    /// <summary>
    /// This script gets info from the ScoreManager to change the chapter color.
    /// Each chapter has its own color.
    /// </summary>
    public class GameScreenBackGroundColor : MonoBehaviour
    {
        ScoreManager scoreManager;

        void Start()
        {
            scoreManager=GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            GetComponent<SpriteRenderer>().material.color = scoreManager.ChapterMainColor;
        }
   
    }//end class
}//end namespace