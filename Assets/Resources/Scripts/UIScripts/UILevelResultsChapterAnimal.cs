﻿
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{

    public class UILevelResultsChapterAnimal : MonoBehaviour
    {

        // Use this for initialization
        public Sprite[] animalImages;

        private Image animalImage;
        public Material winAnimal;
        public Material loseAnimal;

        private ScoreManager scoreManager;

        void Awake()
        {
            animalImage = GetComponent<Image>();
            scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        }
        void OnEnable()
        {
            // find out if player lost or won from the score manager
            // default is the loseAnimal material - shows outline of animal
            animalImage.sprite = animalImages[scoreManager.ChapterNumber - 1];


            if (scoreManager.NumberOfStars > 0)
            {

                // if the player has 1 or more stars, Player has won so change the material to winAnimal, to show the animal in full color	
                //animalImage.material = null;

                animalImage.material = winAnimal;
                animalImage.color = Color.white;
            }
            else
            {
                animalImage.material = loseAnimal;
                animalImage.color = scoreManager.ChapterMainColor;
            }
        }

    }
}