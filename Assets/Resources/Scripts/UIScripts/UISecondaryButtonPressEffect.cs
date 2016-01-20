using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

namespace com.dogonahorse
{
    public class UISecondaryButtonPressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Image secondaryImage;
        public Color secondaryEffectColor;
        private bool buttonIsActive = true;
        private Color originalColor;
        private Color inactiveColor;
        private bool pointerIsDown = false;
        void Start()
        {
            originalColor = secondaryImage.color;
            inactiveColor = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a / 2);
        }

        public void SetActiveStatus(bool  status)

        {
            buttonIsActive = status;
            if (buttonIsActive)
            {
                secondaryImage.color = originalColor;
            }
            else
            {
                secondaryImage.color = inactiveColor;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {

            if (buttonIsActive)
            {
                pointerIsDown = true;
                secondaryImage.color = secondaryEffectColor;
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
      
            if (buttonIsActive)
            {
                pointerIsDown = false;
                secondaryImage.color = originalColor;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
          
            if (buttonIsActive)
            {
                if (pointerIsDown == true)
                {
                    secondaryImage.color = secondaryEffectColor;
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {

            if (buttonIsActive)
            {
                if (pointerIsDown == true)
                {
                    secondaryImage.color = originalColor;
                }
            }
        }
    }
}
