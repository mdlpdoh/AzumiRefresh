using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using System;

namespace com.dogonahorse
{
    public enum SceneState
    {
        Unassigned,
        DebugMode,
        Init,
        PreGame,
        Ready,
        Playing,
        Modal,
        Locked,
        GameOver,
        Closing,
        GameWinSequence

    }

    /**
	 *  Manages all scene assets and behaviors. SceneManager mediates between the GameManager, SceneManager and other high level objects associated with an individual scene.
	 */


    public class SceneManager : StateBehaviour
    {
        private static SceneManager instance = null;

        public static SceneManager Instance
        {
            // return reference to private instance 
            get
            {
                return instance;
            }
        }

        public ModalWindow AboutPangolinsWindow;
        public UIMessageBehavior TitleText;
        public bool developMode = false;
        public InputManager inputManager;
        public Dictionary<ButtonID, ModalWindow> modalWindowDictionary = new Dictionary<ButtonID, ModalWindow>();
        public GameObject devSettingsPanel;

        // private SceneState nextState;

        private int nextChapter;
        private int nextLevel;

        //  private  BlurBackground blurBackground;

        public static int NextChapter
        {
            get
            {
                return instance.nextChapter;
            }
        }
        public static int NextLevel
        {
            get
            {
                return instance.nextLevel;
            }
        }

        void Awake()
        {
            if (instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            // Make this active and only instance
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize<SceneState>();
        }

        public void InitScene()
        {

            Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
            devSettingsPanel = GameObject.Find("DevelopmentSettings");
            ModalWindow[] modals = canvas.GetComponentsInChildren<ModalWindow>(true);
            modalWindowDictionary.Clear();
            for (int i = 0; i < modals.Length; i++)
            {
                modalWindowDictionary.Add(modals[i].buttonID, modals[i]);
            }
            inputManager = GameObject.Find("GameScripts").GetComponent<InputManager>();

            // blurBackground = GameObject.Find("BlurPanel").GetComponent<BlurBackground>();
            ChangeState(SceneState.Init);

            EventManager.ListenForEvent(AzumiEventType.LevelLost, OnLevelLostEvent);
            EventManager.ListenForEvent(AzumiEventType.LevelWon, OnLevelWonEvent);
            EventManager.ListenForEvent(AzumiEventType.FinishEndGameSequence, OnOpenLevelResults);

        }


        public SceneState GetCurrentState()
        {
            return (SceneState)Enum.Parse(typeof(SceneState), GetState().ToString());

        }

        public void StartGamePlay()
        {
            ChangeState(SceneState.Playing);
        }

        public void ButtonClicked(ButtonID buttonID, ButtonAction buttonAction)
        {
            if (buttonAction == ButtonAction.OpenModal)
            {
                EventManager.PostEvent(AzumiEventType.OpenModal, this, buttonID);
                modalWindowDictionary[buttonID].DoButtonAction(buttonAction);
                //pause Game if opening modal while game is running re ready to start
                if (GameManager.GetCurrentState() == GameState.GameLevel && (GetCurrentState() == SceneState.Ready || GetCurrentState() == SceneState.Playing))
                {
           
                    Time.timeScale = 0;
                    //nextState = GetCurrentState();
                }
                ChangeState(SceneState.Modal);

            }
            else if (buttonAction == ButtonAction.CloseModal || buttonAction == ButtonAction.Save || buttonAction == ButtonAction.Cancel)
            {

                if (buttonAction == ButtonAction.Cancel)
                {
                    EventManager.PostEvent(AzumiEventType.CancelSettings, this);
                }
                else if (buttonAction == ButtonAction.Save)
                {
                    EventManager.PostEvent(AzumiEventType.SaveSettings, this);
                }
                // }
                EventManager.PostEvent(AzumiEventType.CloseModal, this, buttonID);
                Time.timeScale = 1;

                ChangeState(SceneState.Ready);


                // modalWindowDictionary[buttonID].DoButtonAction(buttonAction);
            }
            else if (buttonAction == ButtonAction.NextScreen)
            {

                if (buttonID == ButtonID.PreGameModal)
                {
                    ChangeState(SceneState.Closing);
                    GameManager.ChangeScene(nextLevel, NextChapter);
                }
                else
                {
                    Time.timeScale = 1;
                    ChangeState(SceneState.Closing);
                    GameManager.ChangeScene(buttonID, buttonAction);
                }
            }

            else if (buttonAction == ButtonAction.ResetLevel)
            {

                if (GameManager.GetCurrentState() == GameState.EndGame || GameManager.GetCurrentState() == GameState.GameLevel)
                {
                    Time.timeScale = 1;
                    ChangeState(SceneState.Ready);
                    modalWindowDictionary[buttonID].DoButtonAction(ButtonAction.CloseModal);
                    GameManager.ReloadScene();
                }
                else if (GameManager.GetCurrentState() == GameState.Progress)
                {
                    EventManager.PostEvent(AzumiEventType.ResetProgress, this);
                }
            }
        }

        public void LevelButtonClicked(int levelNumber, int chapterNumber)
        {
            nextChapter = chapterNumber;
            nextLevel = levelNumber;
            ChangeState(SceneState.Modal);
            modalWindowDictionary[ButtonID.PreGameModal].DoButtonAction(ButtonAction.OpenModal);
            EventManager.PostEvent(AzumiEventType.OpenModal, this, null);
        }



        public void OnLevelWonEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            ChangeState(SceneState.GameWinSequence);
        }

        public void OnLevelLostEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            ChangeState(SceneState.GameOver);
        }

        public void OnOpenLevelResults(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            ChangeState(SceneState.GameOver);
        }


        #region State methods
        //Enter Actions
        void Init_Enter()
        {
            // Debug.Log("Scene Manager:  Inited");
            ChangeState(SceneState.PreGame);

        }
        void PreGame_Enter()
        {
            if (modalWindowDictionary.ContainsKey(ButtonID.Instructions))
            {
                //nextState = SceneState.Ready;
                ChangeState(SceneState.Modal);

                modalWindowDictionary[ButtonID.Instructions].DoButtonAction(ButtonAction.OpenModal);

            }
            else
            {
                ChangeState(SceneState.Ready);

            }
        }
        void Ready_Enter()
        {
            //  Debug.Log("Scene Manager: Ready");
            if (GameManager.GetCurrentState() == GameState.GameLevel)
            {

            }
        }

        // void Playing_Enter()
        // {
        //      Debug.Log("Scene Manager: Playing");

        // }



        void LevelReset()
        {
            EventManager.ClearGameLevelListeners();
        }


        void GameWinSequence_Enter()
        {

            // Debug.Log("Scene Manager: GameWinSequence_Enter");
            EventManager.PostEvent(AzumiEventType.StartEndGameSequence, this);
            //  ChangeState(SceneState.GameOver);


        }


        void GameOver_Enter()
        {
            // Debug.Log("Scene Manager: GameOver");
            //LevelReset();
            GameManager.GameOver();

            Time.timeScale = 0;
            //devSettingsPanel.SetActive(false);
            modalWindowDictionary[ButtonID.LevelResults].DoButtonAction(ButtonAction.OpenModal);
            EventManager.PostEvent(AzumiEventType.OpenModal, this, null);
        }

        // void Modal_Enter()
        // {
        //      Debug.Log("Modal open");
        // }

        void DebugMode_Enter()
        {
            ChangeState(SceneState.Init);
        }
        #endregion


    }

}

