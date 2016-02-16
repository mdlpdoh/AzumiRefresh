using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace com.dogonahorse
{

    public class LevelButtonBehavior : ButtonBehavior
    {

        public int chapterNumber;
        public int levelNumber;
        //public Text LevelNumberText;
        private int highScore;
        private int maxStarsEarned;

        private Text highScoreText;
        // Use this for initialization
        private UISecondaryButtonPressEffect whitePanel;
        private UILevelButtonLock lockIcon;
        private UIProgressButtonLevelStar[] stars = new UIProgressButtonLevelStar[3];
        private bool LevelIsNewlyOpen = false;
        override public void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.ResetProgress, OnResetProgress);
            buttonType = ButtonType.LevelButton;
            EventManager.ListenForEvent(AzumiEventType.UnlockAllLevels, OnUnlockLevel);
            EventManager.ListenForEvent(AzumiEventType.RelockLevels, OnLockLevel);
            whitePanel = GetComponent<UISecondaryButtonPressEffect>();
            lockIcon = GetComponentInChildren<UILevelButtonLock>();
            base.Start();
            Init();
        }


        public void OnResetProgress(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            Init();
        }

        public void Init()
        {
            LevelIsNewlyOpen = LevelManager.GetPlayerLevelStatusChanged(chapterNumber, levelNumber);
            if (LevelIsNewlyOpen)
            {
                button.interactable = false;
                if (UIChapterPanel.ActiveChapter == chapterNumber && chapterNumber < 4)
                {
                    lockIcon.PrepareToBreakOpen();
                }
                else
                {
                    lockIcon.PrepareToBreakOpenAfterShift();

                    UIChapterPanel.ShiftToNewActivePanel(chapterNumber);
                }

            }
            else
            {
                button.interactable = LevelManager.GetPlayerLevelStatus(chapterNumber, levelNumber); ;
            }


            whitePanel.SetActiveStatus(button.interactable);

            highScore = LevelManager.GetPlayerLevelHighScore(chapterNumber, levelNumber);
            maxStarsEarned = LevelManager.GetPlayerLevelMaxStars(chapterNumber, levelNumber);
            SetUpChildObjects();
            if (button.interactable)
            {
                for (int i = 0; i < stars.Length; i++)
                {
                    if (maxStarsEarned > i)
                    {
                        stars[i].ShowGold();
                    }
                    else
                    {
                        stars[i].ShowGrey();
                    }
                }
            }
        }

        void SetUpChildObjects()
        {
            if (button.interactable)
            {
                lockIcon.Hide();
            }
            else
            {
                lockIcon.Show();
            }
            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTransforms.Length; i++)
            {
                if (childTransforms[i].name == "LevelNumber")
                {
                    childTransforms[i].GetComponent<Text>().text = levelNumber.ToString();
                }
                else if (childTransforms[i].name == "ScoreNumber")
                {
                    highScoreText = childTransforms[i].GetComponent<Text>();


                    if (button.interactable)

                    {
                        highScoreText.text = highScore.ToString();
                        highScoreText.enabled = true;
                    }
                    else
                    {
                        highScoreText.text = highScore.ToString();
                        highScoreText.enabled = false;
                    }
                }
                else if (childTransforms[i].name == "GreyStar")
                {
                    UIProgressButtonLevelStar newStar = childTransforms[i].GetComponent<UIProgressButtonLevelStar>();
                    stars[newStar.StarNumber - 1] = newStar;
                    if (!button.interactable)
                    {
                        newStar.Hide();
                    }
                }

            }
        }

        override public void DoButtonAction()
        {
            EventManager.PostEvent(AzumiEventType.UITap, this);
            if (buttonType == ButtonType.LevelButton)
            {
                InputManager.Instance.LevelButtonClicked(levelNumber, chapterNumber);
            }
            else
            {
                // print("ERROR: Button type is " + buttonType);
            }
        }

        void OnDestroy()
        {
            EventManager.Instance.RemoveListener(AzumiEventType.UnlockAllLevels, OnUnlockLevel);
            EventManager.Instance.RemoveListener(AzumiEventType.RelockLevels, OnLockLevel);
            EventManager.Instance.RemoveListener(AzumiEventType.ResetProgress, OnResetProgress);
        }

        public void OnLockLevel(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            if (!LevelManager.GetPlayerLevelStatus(chapterNumber, levelNumber))
            {
                lockIcon.Show();
                highScoreText.enabled = false;
                button.interactable = false;
                whitePanel.SetActiveStatus(button.interactable);
                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].Hide();
                }
            }
        }
        public void OnUnlockLevel(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            Unlock();

        }
        public void Unlock()
        {
            lockIcon.Hide();
            highScoreText.enabled = true;
            button.interactable = true;
            whitePanel.SetActiveStatus(button.interactable);
            for (int i = 0; i < stars.Length; i++)
            {
                if (maxStarsEarned > i)
                {
                    stars[i].ShowGold();
                }
                else
                {
                    stars[i].ShowGrey();
                }
            }
        }
        string padWithZeroes(string numberString)
        {
            if (numberString.Length < 2)
            {
                return "0" + numberString;

            }
            return numberString;
        }
    }
}