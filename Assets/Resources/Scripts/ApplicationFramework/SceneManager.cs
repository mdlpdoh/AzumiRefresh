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

    //-----------------------------------------------------------
    /// <summary>
    /// Singleton Scene Manager manages all scene assets and behaviors.
    /// SceneManager mediates between the GameManager, SceneManager and other high level objects associated with an individual scene.
    /// </summary>
    /// <remarks>
    /// Subclass of StateBehaviour, part of the MonsterLove State Machine
    /// Attached to SceneScripts gameObject
    /// </remarks>
    public class SceneManager : StateBehaviour
    {
        private static SceneManager instance = null;
        //-----------------------------------------------------------
        /// <summary>
        /// Public access to instance
        /// </summary>
        /// <returns>unique instance of class</returns>
        /// <returns>Apparently not necessary as it is not called from anywhere</returns>
        public static SceneManager Instance
        {
            // return reference to private instance 
            get
            {
                return instance;
            }
        }
        //Stores references to all modal windows
        private Dictionary<ButtonID, ModalWindow> modalWindowDictionary = new Dictionary<ButtonID, ModalWindow>();
        private int nextChapter;
        private int nextLevel;

        //-----------------------------------------------------------
        /// <summary>
        /// Public access to prospective next chapter number
        /// </summary>
        /// <returns>integer number of chapter</returns>
        /// <remarks>
        /// stores ID of  chapter when user has tapped a level button, but not confirmed in modal window
        /// </remarks>
        public static int NextChapter
        {
            get
            {
                return instance.nextChapter;
            }
        }

        //-----------------------------------------------------------
        /// <summary>
        /// Public access to prospective next Level number
        /// </summary>
        /// <returns>integer number of level</returns>
        /// <remarks>
        /// stores ID of  level when user has tapped a level button, but not confirmed in modal window
        /// </remarks>
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

        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.LevelLost, OnLevelLostEvent);
            EventManager.ListenForEvent(AzumiEventType.LevelWon, OnLevelWonEvent);
            EventManager.ListenForEvent(AzumiEventType.FinishEndGameSequence, OnOpenLevelResults);
        }

        //-----------------------------------------------------------
        /// <summary>
        /// initialize UI references and state machine
        /// </summary>
        /// <remarks>
        /// Called by Game Manager upon enetering a new scene/state
        /// </remarks>
        public void InitScene()
        {
            Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
            ModalWindow[] modals = canvas.GetComponentsInChildren<ModalWindow>(true);
            modalWindowDictionary.Clear();
            for (int i = 0; i < modals.Length; i++)
            {
                modalWindowDictionary.Add(modals[i].buttonID, modals[i]);
            }
            ChangeState(SceneState.Init);
        }

        //-----------------------------------------------------------
        /// <summary>
        /// Gets SceneManager's current state's Enumerated ID from the state machine
        /// </summary>
        /// <returns>Enum of type SceneState</returns>
        /// <remarks>
        /// Called by Input Manager, ScoreManager and SceneManager
        /// </remarks>
        public SceneState GetCurrentState()
        {
            return (SceneState)Enum.Parse(typeof(SceneState), GetState().ToString());

        }
        //-----------------------------------------------------------
        /// <summary>
        /// Changes  SceneManager's current state to Playing
        /// </summary>
        /// <remarks>
        /// Called by Input Manager in level scenes when scene goes from idling to being in play
        /// </remarks>
        public void StartGamePlay()
        {
            ChangeState(SceneState.Playing);
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Interprets various button actions
        /// </summary>
        /// <param name="buttonID">ID of button (really of modal)</param>
        /// <param name="buttonAction">Action dictated by button</param>
        /// <remarks>
        /// Ca;;ed directly by InputManager. Ought to be event-driven rather than called directly
        /// </remarks>
        public void ButtonClicked(ButtonID buttonID, ButtonAction buttonAction)
        {
            if (buttonAction == ButtonAction.OpenModal)
            {
                EventManager.PostEvent(AzumiEventType.OpenModal, this, buttonID);
                modalWindowDictionary[buttonID].DoButtonAction(buttonAction);
                //pause Game if opening modal while game is running re ready to start
                if (GameManager.GetCurrentState() == GameState.GameLevel && (GetCurrentState() == SceneState.Ready || GetCurrentState() == SceneState.Playing))
                {
                    EventManager.PostEvent(AzumiEventType.PauseLevel, this);
                    Time.timeScale = 0;
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

                EventManager.PostEvent(AzumiEventType.CloseModal, this, buttonID);
                Time.timeScale = 1;

                ChangeState(SceneState.Ready);
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
                    EventManager.PostEvent(AzumiEventType.RestartLevel, this);
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

        //-----------------------------------------------------------
        /// <summary>
        /// Opens PreGameModal dialog box, and serts values of nextChapter and nextLevel properties
        /// </summary>
        /// <param name="levelNumber">level value for prospective level</param>
        /// <param name="chapterNumber">chapter value for prospective level</param>
        /// <remarks>
        ///  Called directly by InputManager. Ought to be event-driven rather than called directly
        /// </remarks>
        public void LevelButtonClicked(int levelNumber, int chapterNumber)
        {
            nextChapter = chapterNumber;
            nextLevel = levelNumber;
            ChangeState(SceneState.Modal);
            modalWindowDictionary[ButtonID.PreGameModal].DoButtonAction(ButtonAction.OpenModal);
            EventManager.PostEvent(AzumiEventType.OpenModal, this, null);
        }



        private void OnLevelWonEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {

            ChangeState(SceneState.GameWinSequence);
        }

        private void OnLevelLostEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            ChangeState(SceneState.GameOver);
        }

        private void OnOpenLevelResults(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            ChangeState(SceneState.GameOver);
        }


        #region State methods
        //Enter Actions
        void Init_Enter()
        {
            //Debug.Log("Scene Manager:  Inited");
            ChangeState(SceneState.PreGame);

        }
        void PreGame_Enter()
        {
            if (modalWindowDictionary.ContainsKey(ButtonID.Instructions))
            {
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
            //Debug.Log("Scene Manager: Ready");
            if (GameManager.GetCurrentState() == GameState.GameLevel)
            {

            }
        }

        void Playing_Enter()
        {
            // Debug.Log("Scene Manager: Playing");

        }



        void LevelReset()
        {
            EventManager.ClearGameLevelListeners();
        }


        void GameWinSequence_Enter()
        {


            EventManager.PostEvent(AzumiEventType.StartEndGameSequence, this);



        }


        void GameOver_Enter()
        {
            // Debug.Log("Scene Manager: GameOver");

            GameManager.GameOver();
            Time.timeScale = 0;
            modalWindowDictionary[ButtonID.LevelResults].DoButtonAction(ButtonAction.OpenModal);
            EventManager.PostEvent(AzumiEventType.OpenModal, this, null);
        }

        void Modal_Enter()
        {
            //Debug.Log("Modal open");
        }

        void DebugMode_Enter()
        {
            ChangeState(SceneState.Init);
        }
        #endregion


    }

}

