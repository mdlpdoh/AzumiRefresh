using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{
    public enum WinOrLoseCondition
    {
        // Win/Lose Conditions
        Won,
        LostNotEnufAnts,
        LostNoEnergy,
        LostNoTimeLeft

    }

    public class LevelResultsModalWindow : ModalWindow
    {
        //victory messages
        private string victoryMessage1 = "You freed the @A\nwith @B energy left and\ngot @C Ants!";
        //failure messages
        private string failureMessage1 = "Aargh!\nYou didn't get out in time!\n The ants got you! Try again!";
        private string failureMessage2 = "Oh No!\nYou ran out of energy and\ndid not free the @A! Try again!";
        private string failureMessage3 = "The ants got you!\nYou didn't eat enough of them! Try again!";

        private ScoreManager scoreManager;

        private UIStarGiveout starGiveout;

        public Material failPandaMat;


        public UILevelResultsStar[] resultsStars;


        override public void InitWindow()
        {
            scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            // starGiveout = GameObject.Find("SetStars").GetComponent<UIStarGiveout>();//put  INSIDE LRModal instead
            //starGiveout = gameObject.GetComponent<UIStarGiveout>();
            // int correctWinOrLoseMessage = scoreManager.WinAndLoseMessageInfo;
            int numberOfSwipes = scoreManager.numberOfBounces;
            int numberOfCoins = scoreManager.CoinsEarned;
            int totalScore = scoreManager.TotalScore;
            string typeOfAnimal = scoreManager.ChapterAnimalName;
            string resultsMessage;

            //  resultsStars = GetComponentsInChildren<UILevelResultsStar>();

            // pairs the win or lose condition with a message
            if (GetWinOrLoseCondition() == WinOrLoseCondition.Won)
            {
                resultsMessage = ParseMessageString(victoryMessage1, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);

            }
            else if (GetWinOrLoseCondition() == WinOrLoseCondition.LostNoTimeLeft)
            {
                resultsMessage = ParseMessageString(failureMessage1, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
            }
            else if (GetWinOrLoseCondition() == WinOrLoseCondition.LostNotEnufAnts)
            {
                resultsMessage = ParseMessageString(failureMessage3, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
            }
            else
            {
                resultsMessage = ParseMessageString(failureMessage2, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
            }

            //Sends the various text information about score and swipes to the modal 
            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTransforms.Length; i++)
            {
                if (childTransforms[i].name == "TitleText")
                {
                    Text messageText = childTransforms[i].GetComponent<Text>();
                    messageText.text = resultsMessage;
                    SetStars(scoreManager.NumberOfStars); // what we used to give out stars using this script
                                                          //get the number of stars from the score manager into a variable
                                                          // int stars = scoreManager.NumberOfStars;
                                                          //use the setstars method in the UIstargiveout script instead  
                                                          //  starGiveout.GiveOutStars(stars);
                }
                if (childTransforms[i].name == "FinalScoreTextNumeral")
                {
                    Text messageText = childTransforms[i].GetComponent<Text>();
                    messageText.text = totalScore.ToString();
                }

            }//end for loop
        }//end method


        WinOrLoseCondition GetWinOrLoseCondition()
        {
            if (scoreManager.NumberOfStars > 0)
            {
                return WinOrLoseCondition.Won;
            }
            else if (scoreManager.RanOutOfTime == true)
            {
                return WinOrLoseCondition.LostNoTimeLeft;
            }
            else if (scoreManager.TotalScore == 0)
            {
                return WinOrLoseCondition.LostNotEnufAnts;
            }
            else
            {
                return WinOrLoseCondition.LostNoTimeLeft;
            }
        }



        void SetStars(int theNumberOfStars)
        {
            // put this in the UIstargiveout 
            if (scoreManager.NumberOfStars == 0)
            {
                //It will give 3 broken grey stars
                resultsStars[0].ShowFailStar();
                resultsStars[1].ShowFailStar();
                resultsStars[2].ShowFailStar();
            }
            else if (scoreManager.NumberOfStars == 1)
            {
                resultsStars[0].ShowWinStar();
                resultsStars[1].ShowFailStar();
                resultsStars[2].ShowFailStar();
            }
            else if (scoreManager.NumberOfStars == 2)
            {
                resultsStars[0].ShowWinStar();
                resultsStars[1].ShowWinStar();
                resultsStars[2].ShowFailStar();
            }
            else if (scoreManager.NumberOfStars == 3)
            {
                resultsStars[0].ShowWinStar();
                resultsStars[1].ShowWinStar();
                resultsStars[2].ShowWinStar();

            }
            else
            {
                //It will give 3 broken grey stars
                print("Number of Stars does not make any sense ");
                // show the player the filled in red panda				

            }
        }

        /*
                void SetStars(int theNumberOfStars) 
                {
                    // put this in the UIstargiveout 
                    if (scoreManager.NumberOfStars == 0)
                    {
                        //It will give 3 broken grey stars
                        transform.Find("BrokenStar1").GetComponent<Image>().enabled = true;
                        transform.Find("BrokenStar2").GetComponent<Image>().enabled = true;
                        transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;
                    }
                    else if (scoreManager.NumberOfStars == 3)
                    {
                        transform.Find("Star3").GetComponent<Image>().color = Color.red;
                        transform.Find("Star2").GetComponent<Image>().color = Color.red;
                        transform.Find("Star1").GetComponent<Image>().color = Color.red;
                    }
                    else if (scoreManager.NumberOfStars == 2)
                    {
                        transform.Find("Star2").GetComponent<Image>().color = Color.red;
                        transform.Find("Star1").GetComponent<Image>().color = Color.red;
                        transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;
                    }
                    else if (scoreManager.NumberOfStars == 1)
                    {
                        transform.Find("Star1").GetComponent<Image>().color = Color.red;
                        transform.Find("BrokenStar2").GetComponent<Image>().enabled = true;
                        transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;

                    }
                    else
                    {
                        //It will give 3 broken grey stars
                        transform.Find("BrokenStar1").GetComponent<Image>().enabled = true;
                        transform.Find("BrokenStar2").GetComponent<Image>().enabled = true;
                        transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;
                        // show the player the filled in red panda				

                    }
                }
        */
        string ParseMessageString(string rawMessage, int numberOfSwipes, int numberOfCoins, int totalScore, string typeOfAnimal)
        {
            rawMessage = rawMessage.Replace("@A", typeOfAnimal);
            rawMessage = rawMessage.Replace("@B", numberOfSwipes.ToString());
            rawMessage = rawMessage.Replace("@C", numberOfCoins.ToString());
            return rawMessage;
        }


    }// end class
}//end namespace
