using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using System;
using UnityEngine.Analytics;

namespace com.dogonahorse
{
    public enum GameState
    {
        Unassigned,
        DebugMode,
        Init,
        Title,
        Progress,
        GameLevel,
        EndGame,
        Reset
    }

    //-----------------------------------------------------------
    /// <summary>
    /// Singleton Game Manager to oversee top level Game states
    /// </summary>
    /// <remarks>
    /// Subclass of StateBehaviour, part of the MonsterLove State Machine
    /// Attached to GameScripts gameObject
    /// </remarks>
    public class GameManager : StateBehaviour
    {
        private static GameManager instance = null;
        //-----------------------------------------------------------
        /// <summary>
        /// Public access to instance
        /// </summary>
        /// <returns>unique instance of class</returns>
        public static GameManager Instance
        {
            // return reference to private instance 
            get
            {
                return instance;
            }
        }
        [SerializeField]
        private GameState defaultState;
        private SceneManager sceneManager;
        public int frameRate = 60;
        void Awake()
        {
            // Check if existing instance of class exists in scene 35 
            // If so, then destroy this instance
            if (instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            // Make this active and only instance
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize<GameState>();
        }

        void Start()
        {
            Application.targetFrameRate = frameRate;
            sceneManager = GameObject.Find("SceneScripts").GetComponent<SceneManager>();
            ChangeState(GameState.Init);
            Instance.sceneManager.InitScene();
        }

        //-----------------------------------------------------------
        /// <summary>
        /// Gets GameManager's current state's Enumerated ID from the state machine
        /// </summary>
        /// <returns>Enum of type GameState</returns>
        public static GameState GetCurrentState()
        {
            return (GameState)Enum.Parse(typeof(GameState), instance.GetState().ToString());
        }

        //-----------------------------------------------------------
        /// <summary>
        /// Changes Unity scene, and game state. Overloaded with different params for changing levels
        /// </summary>
        /// <param name="buttonID">ID of button--typically the screen it belongs to. Screen ID or Window ID would be a better name</param>
        /// <param name="buttonAction">Type of action called by button</param>
        public static void ChangeScene(ButtonID buttonID, ButtonAction buttonAction)
        {
            GameState currentState = GetCurrentState();
            GameState newState;
            switch (currentState)
            {
                //transition to Title Scene/State
                case GameState.Title:
                    newState = GameState.Progress;
                    UnityEngine.SceneManagement.SceneManager.LoadScene(newState.ToString());
                    Instance.ChangeState(newState);
                    break;
                //transition to Progress Scene/State
                case GameState.EndGame:
                case GameState.GameLevel:
                    newState = GameState.Progress;
                    EventManager.ClearGameLevelListeners();
                    UnityEngine.SceneManagement.SceneManager.LoadScene(newState.ToString());
                    Instance.ChangeState(newState);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
       /// Changes Unity scene, and game state. Overloaded with different params for navigating to the progress screen
        /// </summary>
        /// <param name="levelNumber">Level Number of the level to be opened</param>
        /// <param name="chapterNumber">Chapter Number of the level to be opened</param>
        /// <returns>true if level is available, false if not</returns>
        public static bool ChangeScene(int levelNumber, int chapterNumber)
        {
            GameState currentState = GetCurrentState();

            if (currentState == GameState.Progress)
            {
                //concatenate scene name from level and chapter numbers
                string levelName = "Level_" + padWithZeroes(chapterNumber.ToString()) + padWithZeroes(levelNumber.ToString()); ;
                if (Application.CanStreamedLevelBeLoaded(levelName))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
                    Instance.ChangeState(GameState.GameLevel);
                    LevelManager.SetLevelIDNumbers(chapterNumber, levelNumber);
                    return true;
                }
                else
                {
                    // print("ERROR: Can't find level " + levelName);
                }
            }
            return false;
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Reload current scene
        /// </summary>
        /// <remarks>
        /// called by SceneManager, in response to user pressing level reset button      
        /// </remarks>
        public static void ReloadScene()
        {
            Instance.ChangeState(GameState.Reset);
        }

        static string padWithZeroes(string numberString)
        {
            if (numberString.Length < 2)
            {
                return "0" + numberString;

            }
            return numberString;
        }
        /// <summary>
        /// Change to endgame state
        /// </summary>
        /// <remarks>
        /// called by SceneManager, in response to game ending. 
        /// The change of GameState is a kludge to allow the level to 
        /// reinitialize correctly without having transitioned from another scene     
        /// </remarks>
        public static void GameOver()
        {
            Instance.ChangeState(GameState.EndGame);
        }

        #region State methods
        //Enter Actions
        void Init_Enter()
        {
            // Debug.Log("Game Manager:  Inited");
            ChangeState(defaultState);
        }

        void Title_Enter()
        {
            EventManager.PostEvent(AzumiEventType.EnterTitle, this, null);
            Instance.sceneManager.InitScene();
            // Debug.Log("Game Manager: Title Screen");
        }

        void Progress_Enter()
        {
            EventManager.PostEvent(AzumiEventType.EnterProgress, this, null);
            Instance.sceneManager.InitScene();
            // Debug.Log("Game Manager: Progress Screen");
            if (InputManager.Instance.LevelProgressOverride)
            {
                EventManager.PostEvent(AzumiEventType.UnlockAllLevels, this);
            }
        }
        void GameLevel_Enter()
        {

            EventManager.PostEvent(AzumiEventType.EnterLevel, this, null);
            // Debug.Log("Game Manager: GameLevel");
            Instance.sceneManager.InitScene();
        }

        void EndGame_Enter()
        {
            // Debug.Log("Game Manager: Game is Over");
        }
        void Reset_Enter()
        {
            // Debug.Log("Game Manager: Reset_Enter" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            EventManager.ClearGameLevelListeners();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            Instance.ChangeState(GameState.GameLevel);
        }
        #endregion
    }
}