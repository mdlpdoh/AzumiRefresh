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


		//public Dictionary<int, StudentName> students = new Dictionary<int, StudentName>()
			
		void Awake ()
		{
			Initialize<SceneState>();
		}


		void Start () {

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



		public void ModalOpen(ButtonAction ModalName)
		{
			ChangeState(SceneState.Modal);
			AboutPangolinsWindow.Open();
		}

		public void ModalClose()
		{
			ChangeState(SceneState.Ready);
			AboutPangolinsWindow.Close();
			
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


		/// <summary>
		/// Receives user input.
		/// </summary>
		/// 
		///Receives user input from Input Manager and relays it to the appropriate objects. 
	/*	public void DoUserInput (UserAction newAction) 
		{
			if (!gameIsPaused) 
			{
					//do something to dog or horse
				if (newAction == UserAction.accelerate){
				//	horse.Accelerate();
				// 	dog.Accelerate();
				} else if (newAction == UserAction.decelerate){
				//	horse.Decelerate();
				// 	dog.Decelerate();
				} else if (newAction == UserAction.horsejump){
				//	horse.Jump();
				} else if (newAction == UserAction.dogjump){
				//	dog.Jump();
				} else if (newAction == UserAction.anykey){
					IdleText.StartExit();
					IdleText.onExitActionFinished += Idle_ExitCallback;
				
				}
			} else 
				//don't do anything
			{


		
			}

		}*/

	}

}

