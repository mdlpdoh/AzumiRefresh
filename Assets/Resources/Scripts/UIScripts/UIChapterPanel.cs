using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.dogonahorse
{
    public class UIChapterPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        [SerializeField]
        private bool dummyChapterPanel = false;
        private static int activeChapter = 1;
          public static int ActiveChapter
        {
            // return reference to private instance 
            get
            {
                return activeChapter;
            }
        }
        private static bool drag;
        //location f hatever panel is currently active
        private static float activePanelY;
        private static UIChapterPanel[] chapterPanels = new UIChapterPanel[5];
        private static float[] restPositions = null;
        [SerializeField]
        private int chapterNumber = 1;
        [SerializeField]
        //rate at which panels transition to resting position
        private float dragFriction = 1.5f;
        [SerializeField]
        private float endPanelDragFriction = 5f;
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

        private Color ChapterThirdColor;

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
            if (!dummyChapterPanel)
            {
                ChapterAnimalName = LevelManager.GetChapterAnimalName(chapterNumber, 1);
                ChapterMainColor = LevelManager.GetChapterMainColor(chapterNumber);
                ChapterSecondColor = LevelManager.GetChapterSecondColor(chapterNumber);
                ChapterThirdColor = LevelManager.GetChapterThirdColor(chapterNumber);
                SetUpChildObjects();
            }
            InitRestPositionsArray();
            UpdateRestingY();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, restingY);
        }
        void InitRestPositionsArray()
        {
            if (restPositions == null)
            {
                restPositions = new float[10];
                int basePosition = -4;
                float restingYOffset = 0;

                for (int i = 0; i < restPositions.Length; i++)
                {
                    restingYOffset = basePanelSeparation - ((basePosition - 1) * 50);
                    restPositions[i] = activePanelY - basePosition * restingYOffset;
                    basePosition++;
                }
            }
            else
            {
                print("REST POSITIONS ALREADY CALCULATED");
            }
        }
        void SetUpChildObjects()
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTransforms.Length; i++)
            {
                if (childTransforms[i].name == "ChapterPanel" || childTransforms[i].name == "Foliage")
                {
                    childTransforms[i].GetComponent<Image>().color = ChapterThirdColor;
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

            if (activeChapter == 1 && pointCurrentY < pointStartY)
            {
                UIChapterPanel.AdjustPanels(((pointCurrentY - pointStartY) / endPanelDragFriction) / Screen.height);
            }
            else if (activeChapter == 4 && pointCurrentY > pointStartY)
            {
                UIChapterPanel.AdjustPanels(((pointCurrentY - pointStartY) / endPanelDragFriction) / Screen.height);
            }
            else
            {
                UIChapterPanel.AdjustPanels(((pointCurrentY - pointStartY) / dragFriction) / Screen.height);
            }
        }


        public void MovePanel(float newY)
        {
            float difference = 800;
            //   print (4 + (chapterNumber -activeChapter) );
            //  difference = restingY - restPositions[4 + (chapterNumber -activeChapter) ];

            if (newY > 0)
            {

                difference = restingY - restPositions[chapterNumber - activeChapter + 5];
            }
            else
            {
                difference = restPositions[chapterNumber - activeChapter + 3] - restingY;

            }

            if (chapterNumber == activeChapter)
            {
                //     print ("activeChapter " + activeChapter + " | " + (restingY - restPositions[chapterNumber - activeChapter + 5]));
            }
            // print(chapterNumber - activeChapter + 4 );

            // 
            //print (difference);
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