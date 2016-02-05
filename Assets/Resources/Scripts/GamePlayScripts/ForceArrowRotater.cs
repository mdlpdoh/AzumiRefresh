using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    public class ForceArrowRotater : MonoBehaviour
    {



        //public SpriteRenderer ArrowSpriteRenderer;

        public ForceArrowSprite[] ArrowLights;


        //private bool MainDirectionSelected = true;

        public float MaxFlickerInterval;
        public float MinFlickerInterval;
        private float currentFlickerInterval;
        public float MaxPullDistance;

        private Vector3 startLocation;
        private Vector3 currentLocation;

        private float lastFlickerTime = 0;
        private int lastFlickerSpriteNum = 0;

        public AnimationCurve SpeedCurve;
        private bool flickering;

        // Use this for initialization
        void Start()
        {

            EventManager.ListenForEvent(AzumiEventType.GamePress, OnGamePress);
            EventManager.ListenForEvent(AzumiEventType.GameShift, OnGameShift);
            EventManager.ListenForEvent(AzumiEventType.GameRelease, OnGameRelease);
            EventManager.ListenForEvent(AzumiEventType.LevelLost, OnGameRelease);
            EventManager.ListenForEvent(AzumiEventType.LevelWon, OnGameRelease);
        }

       void OnDestroy()
        {
            EventManager.Instance.RemoveListener(AzumiEventType.GamePress, OnGamePress);
            EventManager.Instance.RemoveListener(AzumiEventType.GameShift, OnGameShift);
            EventManager.Instance.RemoveListener(AzumiEventType.GameRelease, OnGameRelease);
            EventManager.Instance.RemoveListener(AzumiEventType.LevelLost, OnGameRelease);
            EventManager.Instance.RemoveListener(AzumiEventType.LevelWon, OnGameRelease);
        }

        void Update()
        {
            if (flickering)
            {
                Flicker();
            }
        }
        void Flicker()
        {

            if (Time.time > lastFlickerTime + currentFlickerInterval)
            {
                ArrowLights[lastFlickerSpriteNum].StartFadeIn();
                lastFlickerSpriteNum++;
                if (lastFlickerSpriteNum >= ArrowLights.Length)
                {
                    lastFlickerSpriteNum = 0;

                }
                lastFlickerTime = lastFlickerTime + currentFlickerInterval;
            }

        }

        public void OnGamePress(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            startLocation = (Vector3)Param;
            lastFlickerTime = Time.time;


        }
        public void OnGameShift(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            //print("ArrowSpriteRenderer " +  ArrowSpriteRenderer );
            if (!flickering)
            {
                flickering = true;
            }
            SetMarkerRotation((Vector3)Param);
            CalculateFlickerInterval();


        }
        void SetMarkerRotation(Vector3 shiftLocation)
        {
            var newLocation = shiftLocation;
            Vector2 vectorDirection = Vector3.Normalize(newLocation - startLocation);
            float angle = Vector3.Angle(vectorDirection, Vector2.up);

            if (InputManager.MainDirectionSelected)
            {
                if (newLocation.x < startLocation.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, angle - 90);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, -angle - 90);
                }
            }
            else
            {

                if (newLocation.x < startLocation.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, angle + 90);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, -angle + 90);
                }

            }

            currentLocation = newLocation;
        }

        void CalculateFlickerInterval()
        {
            float forceNormal;
            float currentMagnitude = (currentLocation - startLocation).magnitude;
            if (currentMagnitude >= MaxPullDistance)
            {
                forceNormal = 1f;
            }
            else
            {
                forceNormal = SpeedCurve.Evaluate(currentMagnitude / MaxPullDistance);
            }
            for (int i = 0; i < ArrowLights.Length; i++)
            {
                ArrowLights[i].AdjustFadeoutSpeed(forceNormal);
            }
            currentFlickerInterval = MaxFlickerInterval - ((MaxFlickerInterval - MinFlickerInterval) * forceNormal);

        }

        void HideArrow()
        {
            flickering = false;
            for (int i = 0; i < ArrowLights.Length; i++)
            {
                ArrowLights[i].StopAllActivity();
            }
        }

        public void OnGameRelease(AzumiEventType Event_Type, Component Sender, object Param = null)
        {

            HideArrow();

        }


    }
}
