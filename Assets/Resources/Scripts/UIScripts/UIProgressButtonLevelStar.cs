using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace com.dogonahorse
{
    public class UIProgressButtonLevelStar : MonoBehaviour
    {

        public int StarNumber = 1;

        private Image greyStar;
        private Image goldStar;
        // Use this for initialization
        void Awake()
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTransforms.Length; i++)
            {
                if (childTransforms[i].name == "GreyStar")
                {
                    greyStar = childTransforms[i].GetComponent<Image>();
                }
                else if (childTransforms[i].name == "GoldStar")
                {
                    goldStar = childTransforms[i].GetComponent<Image>();
                }
            }
        }


        // Update is called once per frame
        public void Hide()
        {
            greyStar.enabled = false;
            goldStar.enabled = false;
        }



        public void ShowGold()
        {
            greyStar.enabled = false;
            goldStar.enabled = true;
        }

        public void ShowGrey()
        {
            greyStar.enabled = true;
            goldStar.enabled = false;
        }
    }

}

