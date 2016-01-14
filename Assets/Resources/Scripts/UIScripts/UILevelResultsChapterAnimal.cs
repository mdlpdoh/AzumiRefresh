
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


        private ScoreManager scoreManager;

        void Awake()
        {
            animalImage = GetComponent<Image>();
            scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        }
        void OnEnable()
        {

            animalImage.sprite = animalImages[scoreManager.ChapterNumber - 1];
            animalImage.color = scoreManager.ChapterMainColor;
        }
    }
}