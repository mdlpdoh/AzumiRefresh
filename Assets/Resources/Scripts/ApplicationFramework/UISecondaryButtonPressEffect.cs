using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

namespace com.dogonahorse
{
    public class UISecondaryButtonPressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Image secondaryImage;
        public Color secondaryEffectColor;

        private Color originalColor;

        private bool pointerIsDown = false;
        void Start()
        {
            originalColor = secondaryImage.color;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            pointerIsDown = true;
            secondaryImage.color = secondaryEffectColor;


        }
        public void OnPointerUp(PointerEventData eventData)
        {
            pointerIsDown = false;
            secondaryImage.color = originalColor;

        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (pointerIsDown == true)
            {
                secondaryImage.color = secondaryEffectColor;
            }

        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (pointerIsDown == true)
            {
                secondaryImage.color = originalColor;
            }
        }
    }
}
