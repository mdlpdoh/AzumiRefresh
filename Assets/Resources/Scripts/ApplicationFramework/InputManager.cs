using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace com.dogonahorse
{
    /**
	 *  
	 */
    //-----------------------------------------------------------
    /// <summary>
    /// Singleton Manages all player input. 
    /// InputManager controls any touch, keyboard or mouse input by the user, and passes it on to the  SceneManager for interpretation.
    /// </summary>
    /// <remarks>
    /// Attached to GameScripts gameObject
    /// Should be posting events, rather than calling methods in sceneScripts directly
    /// </remarks>
    public class InputManager : MonoBehaviour
    {
        //public GameManager gameManager;
        private SceneManager sceneManager;
        private bool UIControlIsActive;
      //-----------------------------------------------------------
        /// <summary>
        /// Flag as to whether level progress has been overridden 
        /// </summary
        /// <remarks>
        /// Only used for development--prolly should not be public
        /// </remarks>>
        public bool LevelProgressOverride = false;

        private bool InitialMouseDownIsValid = false;

        private Vector3 lastMousePosition;

        //Internal reference to Notifications Manager instance (singleton design pattern)
        private static InputManager instance = null;

        //-----------------------------------------------------------
        //private  EventManager eventManager;
        /// <summary>
        /// Controls orientation of swipe
        /// </summary>
        /// <remarks>
        /// Obsolete--should be eliminated
        /// </remarks>
        public static bool MainDirectionSelected = false;


        //-----------------------------------------------------------
        /// <summary>
        /// Public access to instance
        /// </summary>
        /// <returns>unique instance of class</returns>
        public static InputManager Instance
        {
            // return public reference to private instance 
            get
            {
                return instance;
            }
        }

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
        }

        void Start()
        {
            sceneManager = GameObject.Find("SceneScripts").GetComponent<SceneManager>();
        }


        //swipe version
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !UIControlIsActive)
            {
                // print("foo "+ UIControlIsActive);
                InitialMouseDownIsValid = true;
                lastMousePosition = Camera.main.ViewportToWorldPoint(FixCoordinates(Camera.main.ScreenToViewportPoint(Input.mousePosition)));
                //start game if it hasn't already started kejf
                if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState() == SceneState.Ready)
                {

                    //lastMousePosition = Camera.main.ScreenToViewportPoint (Input.mousePosition);
                    sceneManager.StartGamePlay();
                    EventManager.PostEvent(AzumiEventType.GamePress, this, lastMousePosition);
                }
                //prepare Arrow for activation
                if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState() == SceneState.Playing)
                {
                    lastMousePosition = Camera.main.ViewportToWorldPoint(FixCoordinates(Camera.main.ScreenToViewportPoint(Input.mousePosition)));
                    EventManager.PostEvent(AzumiEventType.GamePress, this, lastMousePosition);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {

                //      print("Fee "+ UIControlIsActive);
            }

            //Update active arrow
            Vector3 newMousePosition;
            if (Input.GetMouseButton(0) && !UIControlIsActive)
            {

                if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState() == SceneState.Playing)
                {
                    newMousePosition = Camera.main.ViewportToWorldPoint(FixCoordinates(Camera.main.ScreenToViewportPoint(Input.mousePosition)));
                    if (lastMousePosition != newMousePosition)
                    {
                        EventManager.PostEvent(AzumiEventType.GameShift, this, newMousePosition);
                    }

                }

            }
            //Release mouse 
            if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState() == SceneState.Playing)
            {

                if (Input.GetMouseButtonUp(0) && !UIControlIsActive && InitialMouseDownIsValid)
                {
                    //					print ("GetMouseButtonUp");
                    InitialMouseDownIsValid = false;
                    newMousePosition = Camera.main.ViewportToWorldPoint(FixCoordinates(Camera.main.ScreenToViewportPoint(Input.mousePosition)));
                    EventManager.PostEvent(AzumiEventType.GameRelease, this, newMousePosition);
                    if (MainDirectionSelected)
                    {
                        EventManager.PostEvent(AzumiEventType.GameSwipe, this, lastMousePosition - newMousePosition);
                    }
                    else
                    {
                        EventManager.PostEvent(AzumiEventType.GameSwipe, this, newMousePosition - lastMousePosition);
                    }
                }
            }
        }

        //screenCoordinates start as a Vector2D, but need to be put into 3D worldspace 
        Vector3 FixCoordinates(Vector3 screenCoordinates)
        {
            return new Vector3(screenCoordinates.x, screenCoordinates.y, 10);
        }

        //-----------------------------------------------------------
        /// <summary>
        /// Called whenever a main UI button other than a level button is tapped
        /// </summary>
        /// <param name="buttonID">ID of button (really of modal)</param>
        /// <param name="buttonAction">Action dictated by button</param>
        /// <remarks>
        /// Called directly by buttons--should be event-driven
        /// </remarks>
        #region Button Input
        public void MainButtonClicked(ButtonID buttonID, ButtonAction buttonAction)
        {
            if (sceneManager.GetCurrentState() == SceneState.Ready || sceneManager.GetCurrentState() == SceneState.Playing)
            {
                sceneManager.ButtonClicked(buttonID, buttonAction);

            }
        }

        //-----------------------------------------------------------
        /// <summary>
        /// Called whenever a modal window  button  is tapped
        /// </summary>
        /// <param name="buttonID">ID of button (really of modal)</param>
        /// <param name="buttonAction">Action dictated by button</param>
        /// <remarks>
        /// Called directly by buttons--should be event-driven
        /// </remarks>
        public void ModalButtonClicked(ButtonID buttonID, ButtonAction buttonAction)
        {
            SceneState sceneState = sceneManager.GetCurrentState();
            if (sceneState == SceneState.Modal || sceneState == SceneState.GameOver)
            {
                sceneManager.ButtonClicked(buttonID, buttonAction);

            }
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Called whenever a "select level"   button  is tapped
        /// </summary>
        /// <param name="levelNumber">level value for prospective level</param>
        /// <param name="chapterNumber">chapter value for prospective level</param>
        /// <remarks>
        ///  Called directly by InputManager. Ought to be event-driven rather than called directly
        /// </remarks>
        public void LevelButtonClicked(int LevelNumber, int chapterNumber)
        {
            if (sceneManager.GetCurrentState() == SceneState.Ready)
            {
                sceneManager.LevelButtonClicked(LevelNumber, chapterNumber);

            }
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Called by various controls to lock other button and other UI activity while a partiular control is active
        /// </summary>
        /// <remarks>
        ///  Called directly by controls. Ought to be event-driven rather than called directly
        /// </remarks>
        public void ControlActive()
        {
            //print ("ControlActive");
            UIControlIsActive = true;
        }
        /// <summary>
        /// Called by various controls to unlock other button and other UI activity while a partiular control is active
        /// </summary>
        /// <remarks>
        ///  Called directly by controls. Ought to be event-driven rather than called directly
        /// </remarks>
        public void ControlNotActive()
        {
            //print ("ControlNotActive");
            UIControlIsActive = false;
        }

        #endregion
        //-----------------------------------------------------------
        /// <summary>
        /// Called by level unlock override button to resume normal behavior
        /// </summary>
        /// <remarks>
        /// Note: Only used during development 
        ///  Called directly by controls. Ought to be event-driven rather than called directly
        /// </remarks>
        public void LockLevelButtons()
        {
            //print ("ControlActive");
            LevelProgressOverride = false;
            EventManager.PostEvent(AzumiEventType.RelockLevels, this);
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Called by level unlock override button to artificially unlock all level button
        /// </summary>
        /// <remarks>
        /// Note: Only used during development 
        ///  Called directly by controls. Ought to be event-driven rather than called directly
        /// </remarks>
        public void UnlockLevelButtons()
        {
            //print ("LevelProgressOverride");
            LevelProgressOverride = true;
            EventManager.PostEvent(AzumiEventType.UnlockAllLevels, this);
        }
    }
}
