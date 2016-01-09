using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace com.dogonahorse
{
    public class HealHurtWallExpireEffects : MonoBehaviour
    {

        public float fadeTime = 0.5f;

        public AzumiEventType eventType;

        public GameObject heartSpriteContainer;
        
           public int MaxParticles = 450;
        private MeshRenderer meshRenderer;
        public AnimationCurve fadeCurve;
        public AnimationCurve particleCurve;
        private WallBehavior myWall;
        private SpriteRenderer[] heartSprites;

        private ParticleSystem fadeParticles;
        // Use this for initialization
        
        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            EventManager.ListenForEvent(eventType, StartFade);
            myWall = GetComponent<WallBehavior>();
            heartSprites = heartSpriteContainer.GetComponentsInChildren<SpriteRenderer>();
            fadeParticles =  GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        public void StartFade(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            if (Sender == myWall)
            {
                StartCoroutine("FadeOut");
            }
        }


        private IEnumerator FadeOut()
        {

            float currentTime = 0f;
            while (currentTime < fadeTime)
            {
                float normalizedTime = currentTime / fadeTime;
                float fadeCurveProgress = 1 - fadeCurve.Evaluate(normalizedTime);
                float particleCurveProgress =  particleCurve.Evaluate(normalizedTime);
                FadeMainQuad(fadeCurveProgress);
                FadeHearts(fadeCurveProgress);
                EmitParticles(particleCurveProgress);
                currentTime += Time.deltaTime;
                yield return null;
            }
            FadeMainQuad(0);
            FadeHearts(0);
                   EmitParticles(0);
        }

        void FadeMainQuad(float fadeLevel)
        {
            Color color;
            color = meshRenderer.material.color;
            color.a =  fadeLevel;
            meshRenderer.material.color = color;
        }

        void FadeHearts(float fadeLevel)
        {
            int i;
            Color color;
            color = heartSprites[0].material.color;
            color.a = fadeLevel;
            for (i = 0; i < heartSprites.Length; i++)
            {
                heartSprites[i].material.color = color;
            }
        }
        
        
        void EmitParticles(float particleLevel)
        {
          

       
           fadeParticles.emissionRate = particleLevel * MaxParticles;
        }

        void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(eventType);
        }
    }
}
