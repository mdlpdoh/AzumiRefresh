using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using System;

namespace com.dogOnaHorse
{
	public enum SceneState
	{
		Unassigned,
		DebugMode, 
		Init,
		Ready,
		Modal,
		Locked,
		Closing
	}

	/**
	 *  Manages all scene assets and behaviors. SceneManager mediates between the GameManager, SceneManager and other high level objects associated with an individual scene.
	 */
	public class SceneManager : StateBehaviour 
	{

		public ModalWindow AboutPangolinsWindow;
		public UIMessageBehavior TitleText;
		public bool developMode = false;
		public InputManager inputManager;
		public  Dictionary<ButtonID, ModalWindow> modalWindowDictionary = new Dictionary<ButtonID, ModalWindow>();
		
		//public Dictionary<int, StudentName> students = new Dictionary<int, StudentName>()
			
		void Awake ()
		{
			Initialize<SceneState>();
		}


		void Start () {

			if (Application.loadedLevelName == "Title" || Application.loadedLevelName == "Progress"){
		
			Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
			
			ModalWindow[] modals =  canvas.GetComponentsInChildren<ModalWindow>(true);
			for(int i=0;i<modals.Length; i++){
				modalWindowDictionary.Add(modals[i].buttonID, modals[i]);
			}

			}

			inputManager = GameObject.Find("GameScripts").GetComponent<InputManager>();
			inputManager.RegisterCurrentSceneManager(this);
			if (developMode) {
				ChangeState(SceneState.DebugMode);
			}else {
				ChangeState(SceneState.Init);
			}
		}

		public SceneState GetCurrentState() {
			
			return  (SceneState)Enum.Parse(typeof(SceneState), GetState().ToString());
			
		}

		void DebugMode_Enter () {
			ChangeState(SceneState.Init);
		}



		public void ButtonClicked(ButtonID buttonID, ButtonAction buttonAction) 
		{
			if (buttonAction == ButtonAction.OpenModal){
				ChangeState(SceneState.Modal);
				modalWindowDictionary[buttonID].DoButtonAction(buttonAction);
			} else if (buttonAction == ButtonAction.CloseModal){
				ChangeState(SceneState.Ready);
				modalWindowDictionary[buttonID].DoButtonAction(buttonAction);
			} else if (buttonAction == ButtonAction.NextScreen){
				ChangeState(SceneState.Closing);
				GameManager.ChangeScene( buttonID,  buttonAction);
			}
		}
		public void LevelButtonClicked(int levelNumber, int chapterNumber) 
		{
			if (GameManager.ChangeScene( levelNumber,  chapterNumber)){
				ChangeState(SceneState.Closing);
			};
		}


		#region State methods
		//Enter Actions
		void Init_Enter()
		{
			Debug.Log("Scene is now Inited");
			ChangeState(SceneState.Ready);
		}

		void Ready_Enter()
		{
			Debug.Log("Scene Manager is now Ready");
		}

		void Modal_Enter()
		{
			Debug.Log("Modal open");
		
		}
		#endregion


	}

}

