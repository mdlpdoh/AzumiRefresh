using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.dogonahorse
{
    public class UIChapterPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {


        [SerializeField]
        private int chapterNumber = 1;
        [SerializeField]
        private float dragFriction = 1.5f;
        [SerializeField]
        private float springBackRatio = 2f;






        private static bool drag;
        // Use this for initialization
        private float pointStartY;
        private float pointCurrentY;
        // private float offsetY;
        private RectTransform rectTransform;
        private float rectStartY;
        private float restingY;

        private string ChapterAnimalName;
        private Color ChapterMainColor;
        private Color ChapterSecondColor;

        private static UIChapterPanel[] chapterPanels = new UIChapterPanel[4];


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

        static void ReInitPanelsRestingY(float adjustY)
        {
            for (int i = 0; i < chapterPanels.Length; i++)
            {
                chapterPanels[i].ReInitRestingY(adjustY);
            }
        }


        void InitStartY()
        {
            rectStartY = rectTransform.localPosition.y;
            
        }

        void ReInitRestingY (float adjustY)
        {
              restingY  = rectStartY + adjustY;
        }
        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
           // restingY = rectTransform.localPosition.y;
            chapterPanels[chapterNumber - 1] = this;
        }


        void Start()
        {
            ChapterAnimalName = LevelManager.GetChapterAnimalName(chapterNumber, 1);
            ChapterMainColor = LevelManager.GetChapterMainColor(chapterNumber);
            ChapterSecondColor = LevelManager.GetChapterSecondColor(chapterNumber);
            SetUpChildObjects();
            print ("rectTransform.localPosition.y " + rectTransform.localPosition.y);
               print ("rectTransform.anchoredPosition.y " + rectTransform.anchoredPosition.y);
            
            restingY = rectTransform.localPosition.y - (chapterNumber - 1) * 800;
            
            
            rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, restingY);
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
            if (!UIChapterPanel.drag && restingY != rectTransform.localPosition.y)
            {
                rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, restingY + (rectTransform.localPosition.y - restingY) / springBackRatio);
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
            UIChapterPanel.AdjustPanels((pointCurrentY - pointStartY) / dragFriction);
        }

        public void MovePanel(float newY)
        {
            rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, rectStartY + newY);

        }
        public void OnEndDrag(PointerEventData eventData)
        {
          
            
            if (Mathf.Abs(rectTransform.localPosition.y - restingY) > 400){
                if (rectTransform.localPosition.y > restingY)
                {
                    UIChapterPanel.ReInitPanelsRestingY(800);
                }
                else
                {
                    UIChapterPanel.ReInitPanelsRestingY(-800);
                }

            }
            UIChapterPanel.drag = false;
        }
    }
}