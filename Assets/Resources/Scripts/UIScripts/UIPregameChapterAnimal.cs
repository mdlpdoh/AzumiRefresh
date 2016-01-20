﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;



namespace com.dogonahorse
{

    public class UIPregameChapterAnimal : MonoBehaviour
    {

        // Use this for initialization
        public Sprite[] animalImages;

        private Image animalImage;

        void Awake()
        {
            animalImage = GetComponent<Image>();
        }
        void OnEnable()
        {
          animalImage.sprite = animalImages[UIChapterPanel.ActiveChapter - 1];
          animalImage.color = LevelManager.GetChapterMainColor(UIChapterPanel.ActiveChapter);
        }

        // Update is called once per frame

    }
}