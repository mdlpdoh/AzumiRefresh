using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.dogonahorse
{
    public class UIChapterPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private static int activeChapter = 1;
        private static bool drag;
        //location f hatever panel is currently active
        private static float activePanelY;


        private static UIChapterPanel[] chapterPanels = new UIChapterPanel[4];


        private static float[] restPositions = null;

        [SerializeField]
        private int chapterNumber = 1;
        [SerializeField]
        //rate at which panels transition to resting position
        private float dragFriction = 1.5f;
        [SerializeField]
        //rate at which panels transition to resting position
        private float springBackRatio = 2f;
        //distance between active panel and the one below it
        [SerializeField]
        private float basePanelSeparation = 800f;
        //point where touch started
        private float pointStartY;
        //point where touch is now
        private float pointCurrentY;
        // private float offsetY;
        private RectTransform rectTransform;
        //current  location, at time when drag started
        private float rectStartY;
        //current rest location, based on which chapter is active-- panel seeks this location when released
        public float restingY;


        private string ChapterAnimalName;
        private Color ChapterMainColor;
        private Color ChapterSecondColor;


        static void AdjustPanels(float newY)
        {
            for (int i = 0; i < chapterPanels.Length; i++)
            {
                chapterPanels[i].MovePanel(newY);
            }
        }

        static void InitPanelsStartY()
        {
            for (int i = 0; i < chapterPanels.Length; i++)
            {
                chapterPanels[i].InitStartY();
            }
        }

        static void UpdateAllRestingY(float adjustY)
        {
            for (int i = 0; i < chapterPanels.Length; i++)
            {
                chapterPanels[i].UpdateRestingY();
            }
        }


        void InitStartY()
        {
            rectStartY = rectTransform.anchoredPosition.y;

        }

        void UpdateRestingY()
        {
           /*
            int difference = chapterNumber - activeChapter;
            float restingYOffset = basePanelSeparation - ((difference - 1) * 50);
            restingY = activePanelY - difference * restingYOffset;
*/

            int difference = 4 + chapterNumber - activeChapter;
            restingY = restPositions[difference];
        }

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            chapterPanels[chapterNumber - 1] = this;
        }

        void Start()
        {
            activePanelY = rectTransform.anchoredPosition.y;
            ChapterAnimalName = LevelManager.GetChapterAnimalName(chapterNumber, 1);
            ChapterMainColor = LevelManager.GetChapterMainColor(chapterNumber);
            ChapterSecondColor = LevelManager.GetChapterSecondColor(chapterNumber);
            SetUpChildObjects();
            InitRestPositionsArray();
            UpdateRestingY();

            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, restingY);



        }
        void InitRestPositionsArray()
        {
        
            if (restPositions == null)
            {
                restPositions = new float[9];
                int basePosition = -4;
                float restingYOffset = 0;

                for (int i = 0; i < restPositions.Length; i++)
                {
                    restingYOffset = basePanelSeparation - ((basePosition -1)* 50);
                    restPositions[i] = activePanelY - basePosition * restingYOffset;
                    basePosition++;
                }
            } else {
                print ("REST POSITIONS ALREADY CALCULATED");
            }
        }
        void SetUpChildObjects()
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTransforms.Length; i++)
            {
                if (childTransforms[i].name == "ChapterPanel" || childTransforms[i].name == "Foliage")
                {
                    childTransforms[i].GetComponent<Image>().color = ChapterMainColor;
                }
                if (childTransforms[i].name == "LevelLabel")
                {
                    childTransforms[i].GetComponent<Text>().color = ChapterMainColor;
                }
                if (childTransforms[i].name.Contains("LevelButton"))
                {
                    childTransforms[i].GetComponent<Image>().color = ChapterSecondColor;
                }
                if (childTransforms[i].name == "AnimalName")
                {
                    childTransforms[i].GetComponent<Text>().text = ChapterAnimalName.ToUpper();
                }
                if (childTransforms[i].name == "ChapterNumber")
                {
                    childTransforms[i].GetComponent<Text>().text = "CHAPTER " + chapterNumber;
                }
            }
        }


        void Update()
        {
            if (!UIChapterPanel.drag && restingY != rectTransform.anchoredPosition.y)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, restingY + (rectTransform.anchoredPosition.y - restingY) / springBackRatio);
            }
        }



        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            InitPanelsStartY();
            pointStartY = eventData.pressPosition.y;
        }


        public void OnDrag(PointerEventData eventData)
        {
            pointCurrentY = eventData.position.y;

            UIChapterPanel.drag = true;

            UIChapterPanel.AdjustPanels((pointCurrentY - pointStartY) / Screen.height);
            if (pointCurrentY > pointStartY)
            {
                print("UP " + ((pointCurrentY - pointStartY) / Screen.height));

            }
            else
            {
                print("down " + ((pointCurrentY - pointStartY) / Screen.height));
            }

        }


        public void MovePanel(float newY)
        {
            float difference = 800;
            if (newY > 0 && chapterNumber < 4)
            {

              //  difference = restingY - UIChapterPanel.chapterPanels[chapterNumber].restingY;
              //  difference = restingY - restPositions[4 + activeChapter - chapterNumber ];
             // PRINT ()
            }
            else if (chapterNumber > 1)
            {
                
                 //        difference = restingY - restPositions[activeChapter - chapterNumber];


            }
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectStartY + newY * difference);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Mathf.Abs(rectTransform.anchoredPosition.y - restingY) > basePanelSeparation / 2)
            {
                if (activeChapter < chapterPanels.Length && rectTransform.anchoredPosition.y > restingY)
                {

                    activeChapter++;
                    UIChapterPanel.UpdateAllRestingY(basePanelSeparation);
                }
                else if (activeChapter > 1 && rectTransform.anchoredPosition.y <= restingY)
                {
                    activeChapter--;
                    UIChapterPanel.UpdateAllRestingY(-basePanelSeparation);
                }

            }
            // print("activeChapter " + activeChapter);
            UIChapterPanel.drag = false;
        }
    }
}