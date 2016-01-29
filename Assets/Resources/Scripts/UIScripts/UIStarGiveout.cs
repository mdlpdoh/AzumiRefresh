using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{

    public class UIStarGiveout : MonoBehaviour {
        
/* Script to give out correct number of stars and animate them from big to small
1. get red grey star giveout from Level results modal window
2. invoke timers for a delay and make public so we can change them in the inspector
3. get the scale and make it large and small with a coroutine

// get info from the score manager to display correct num of stars
 private ScoreManager scoreManager;
 
 // create a public float so we can time it right
 [SerializeField]private float enlargeTime = 1f;
 
// get the broken and full stars
Transform[] allStarTransforms = GetComponentsInChildren<Transform>();
for (int i = 0; i < allStarTransforms.Length; i++)
{
    if (allStarTransforms[i].name == "")
    {}
}

*/ 

// get info from the score manager to display correct num of stars
 private ScoreManager scoreManager;
 
 // create a public float so we can time it right
//  [SerializeField]private float enlargeTime = 1f;
 
public void GiveOutStars(int numberOfStars)
{
     StartCoroutine(GrowStars(numberOfStars));
}

private IEnumerator GrowStars(int numStars)
{
    if (numStars == 0)
    {
        // transform.Find("BrokenStar1").GetComponent<Image>().enabled = true;
        print("NO STARS FOR YOUUUUUUUUUUUUUUU");
    }
    else
    {
        print("YOU HAVE " + numStars);
    }

    // for (float f = 1f; f >= 0; f += 0.1f) {
    //     Vector2 size  = transform.localScale;
    //     size = new Vector2(f, f);
    //     yield return new WaitForSeconds(.1f);
    // }
    
    // float currentTime = 0f;
    // float incrementSize = 0.1f;
    // float finalSize = 1f;
    // while (incrementSize < finalSize)
    // {
    //     transform.localScale = new Vector2(0.1f, 0.1f);
    //     yield return new WaitForSeconds(.5f);
    // }
     yield return new WaitForSeconds(.5f);
}
   


 
      
        // Use this for initialization
        void Start () {
        
        }
        
        // Update is called once per frame
        void Update () {
        
        }
    }
}
