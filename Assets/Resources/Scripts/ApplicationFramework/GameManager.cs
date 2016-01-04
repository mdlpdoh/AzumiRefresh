using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using System;
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


    public class GameManager : StateBehaviour
    {

        private static GameManager instance = null;


        public static GameManager Instance
        {
            // return reference to private instance 

            get
            {
                return instance;
            }
        }

        public GameState defaultState;
        public SceneManager sceneManager;

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

        public static GameState GetCurrentState()
        {
            return (GameState)Enum.Parse(typeof(GameState), instance.GetState().ToString());
        }

        public static void ChangeScene(ButtonID buttonID, ButtonAction buttonAction)
        {
            GameState currentState = GetCurrentState();
            GameState newState;


            switch (currentState)
            {
                case GameState.Title:
                    newState = GameState.Progress;
                    Application.LoadLevel(newState.ToString());
                    Instance.ChangeState(newState);
                    break;
                case GameState.EndGame:
                case GameState.GameLevel:
                    newState = GameState.Progress;
                    EventManager.ClearGameLevelListeners();
                    Application.LoadLevel(newState.ToString());
                    Instance.ChangeState(newState);
                    break;
                default:
                    break;

            }

        }

        public static void ReloadScene()
        {

            Instance.ChangeState(GameState.Reset);


        }

        public static bool ChangeScene(int levelNumber, int chapterNumber)
        {

            GameState currentState = GetCurrentState();

            if (currentState == GameState.Progress)
            {
                string levelName = "Level_" + padWithZeroes(chapterNumber.ToString()) + padWithZeroes(levelNumber.ToString()); ;

                if (Application.CanStreamedLevelBeLoaded(levelName))
                {

                    Application.LoadLevel(levelName);
                    Instance.ChangeState(GameState.GameLevel);
                    LevelManager.SetLevelIDNumbers(chapterNumber, levelNumber);
                    return true;

                }
                else
                {
                    print("ERROR: Can't find level " + levelName);
                }
            }
            return false;
        }
        static string padWithZeroes(string numberString)
        {
            if (numberString.Length < 2)
            {
                return "0" + numberString;

            }
            return numberString;
        }
        /*
		public static void ReturnToProgressScreen(){
			EventManager.ClearGameLevelListeners();
			Instance.ChangeState (GameState.Progress );
			Application.LoadLevel("Progress");
			
		}*/
        public static void GameOver()
        {
            Instance.ChangeState(GameState.EndGame);
        }

        #region State methods
        //Enter Actions
        void Init_Enter()
        {
           
            Debug.Log("Game Manager:  Inited");
            ChangeState(defaultState);
        }

        void Title_Enter()
        {
            EventManager.PostEvent(AzumiEventType.EnterTitle, this, null); 
            Instance.sceneManager.InitScene();
            Debug.Log("Game Manager: Title Screen");
        }

        void Progress_Enter()
        {
            EventManager.PostEvent(AzumiEventType.EnterProgress, this, null); 
            Instance.sceneManager.InitScene();
            Debug.Log("Game Manager: Progress Screen");

        }
        void GameLevel_Enter()
        {
          EventManager.PostEvent(AzumiEventType.EnterLevel, this, null); 
            Debug.Log("Game Manager: GameLevel");
            Instance.sceneManager.InitScene();
      

        }

        void EndGame_Enter()
        {

            Debug.Log("Game Manager: Game is Over");

        }
        void Reset_Enter()
        {
            EventManager.ClearGameLevelListeners();
            Application.LoadLevel(Application.loadedLevel);

            Debug.Log("Game Manager: Reset_Enter");
            Instance.ChangeState(GameState.GameLevel);
        }

        #endregion
    }
}