using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using System;

namespace com.dogOnaHorse
{
	public enum TitleSceneState
	{
		Unassigned,
		DebugMode, 
		Init,
		Ready,
		About,
		Locked,
		Closing
	}
	/**
	 *  Manages all scene assets and behaviors. SceneManager mediates between the GameManager, SceneManager and other high level objects associated with an individual scene.
	 */
	public class TitleSceneManager : StateBehaviour 
	{

		public ModalWindow AboutPangolinsWindow;
		public UIMessageBehavior TitleText;




		
		public bool developMode = false;

		void Awake () {
			Initialize<TitleSceneState>();
		}

		void Start () {
			if (developMode) {
				ChangeState(TitleSceneState.DebugMode);
			}else {

			ChangeState(TitleSceneState.Init);
			}
		}

		public TitleSceneState GetCurrentState() {
			
			return  (TitleSceneState)Enum.Parse(typeof(TitleSceneState), GetState().ToString());
			
		}

		void DebugMode_Enter () {
			ChangeState(TitleSceneState.Init);
		}



		public void AboutPangolinsOpen()
		{
			ChangeState(TitleSceneState.About);
			AboutPangolinsWindow.Open();
		}

		public void AboutPangolinsClose()
		{
			ChangeState(TitleSceneState.Ready);
			AboutPangolinsWindow.Close();
			
		}


		#region State methods
		//Enter Actions
		void Init_Enter()
		{
			Debug.Log("Scene is now Inited");
			ChangeState(TitleSceneState.Ready);
		}

		void Ready_Enter()
		{
			Debug.Log("Scene is now Ready");
		}

		void About_Enter()
		{
			Debug.Log("About Modal open");
		
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

