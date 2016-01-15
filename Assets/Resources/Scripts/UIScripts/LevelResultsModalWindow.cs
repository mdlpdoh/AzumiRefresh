using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{

    public class LevelResultsModalWindow : ModalWindow
    {
        //victory messages
        private string victoryMessage1 = "You freed the @A\nwith @B energy left and\ngot @C Ants!";
        //failure messages
        private string failureMessage1 = "Aargh!\nOut of energy and you didn't eat\nenough ants! Try again!"; // You lost, did not eat enuf ants and ran out of swipes.
        private string failureMessage2 = "Oh No!\nYou ran out of energy and\ndid not free the@A!\nTry again!"; // You lost, got the min amt of ants but you ran out of swipes. 
        private string failureMessage3 = "The ants got you!\nYou didn't eat enough of them! Try again!"; // You lost, did not eat min amt of ants but you still had swipes left.

        private ScoreManager scoreManager;
        public Material failPandaMat;

        override public void InitWindow()
        {

            scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            int correctWinOrLoseMessage = scoreManager.WinAndLoseMessageInfo;
            int numberOfSwipes = scoreManager.numberOfBounces;
            int numberOfCoins = scoreManager.CoinsEarned;
            int totalScore = scoreManager.TotalScore;
            string typeOfAnimal = scoreManager.ChapterAnimalName;
            string resultsMessage;

            if (correctWinOrLoseMessage == 0)
            {
                // 0 stars. You lost, did not eat min amt of ants but you still had swipes left.
                resultsMessage = ParseMessageString(failureMessage3, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
            }
            else if (correctWinOrLoseMessage == 5)
            {
                // 0 stars. You lost, did not eat enuf ants and ran out of swipes.
                resultsMessage = ParseMessageString(failureMessage1, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
            }
            else if (correctWinOrLoseMessage == 4)
            {
                // 0 stars. You lost, got the min amt of ants but you ran out of swipes. 
                resultsMessage = ParseMessageString(failureMessage2, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
            }

            else if (correctWinOrLoseMessage > 0)
            {
                // 1 or more stars. You got min amt of ants in amt of swipes given.
                resultsMessage = ParseMessageString(victoryMessage1, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
            }
            else
            {
                //Players got out with 0 or more energy and the min amt of ants or more - 1 or more stars.
                resultsMessage = ParseMessageString(victoryMessage1, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
            }


            //Sends the various text information about score and swipes to the modal 
            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTransforms.Length; i++)
            {
                if (childTransforms[i].name == "TitleText")
                {
                    Text messageText = childTransforms[i].GetComponent<Text>();
                    messageText.text = resultsMessage;
                    SetStars(correctWinOrLoseMessage);
                }
                if (childTransforms[i].name == "FinalScoreTextNumeral")
                {
                    Text messageText = childTransforms[i].GetComponent<Text>();
                    messageText.text = totalScore.ToString();
                }

            }//end for loop
        }//end method
        public int WinAndLoseMessageInfo
        // This gives the LevelResultsModalWindow information so it can spit out correct win/lose message
        // number of stars given based on percentage of ants(coins) cleared in level
        {
            // return reference to private instance 
            get
            {


                if (scoreManager.NumberOfStars == 3)
                {
                    return 3;
                }
                else if (scoreManager.NumberOfStars == 2)
                {
                    return 2;
                }
                else if (scoreManager.NumberOfStars == 1)
                {
                    return 1;
                }
                else if (scoreManager.NumberOfStars == 0 && scoreManager.ExitedDoorSafely == true)
                {
                    return 4;
                }
                else if (scoreManager.NumberOfStars == 0 && scoreManager.ExitedDoorSafely == false )
                {

                    return 5;
                }
                else
                {
                    return 0;
                }
            }
        }

        void SetStars(int correctWinOrLoseMessage)
        {

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

        string ParseMessageString(string rawMessage, int numberOfSwipes, int numberOfCoins, int totalScore, string typeOfAnimal)
        {
            rawMessage = rawMessage.Replace("@A", typeOfAnimal);
            rawMessage = rawMessage.Replace("@B", numberOfSwipes.ToString());
            rawMessage = rawMessage.Replace("@C", numberOfCoins.ToString());
            return rawMessage;
        }


    }// end class
}//end namespace
