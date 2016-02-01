using UnityEngine;
using System.Collections;


namespace com.dogonahorse
{
    public class GameScreenBackGroundColor : MonoBehaviour
    {
        ScoreManager scoreManager;
        // Use this for initialization
        void Start()
        {
            scoreManager=GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            GetComponent<SpriteRenderer>().material.color = scoreManager.ChapterMainColor;
        }

   
    }
}