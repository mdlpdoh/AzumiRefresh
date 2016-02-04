using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.dogonahorse
{
	public enum ButtonID
	{
		Unassigned,
		Play,
		AboutPangolins,
		Store,
		Achievements,
		LeaderBoards,
		Settings,
		PowerUps,
		Connect,
		LevelResults,
		DevelopmentSettings,
		PauseButton,
		
		PreGameModal,
		
		Instructions
	}
	public enum ButtonType
	{
		Unassigned,
		MainWindowButton, 
		ModalWindowButton,
		LevelButton
	}

	public enum ButtonAction
	{
		Unassigned,
		OpenModal, 
		CloseModal,
		Select,
		Buy,
		NextScreen,
		ResetLevel,
        Cancel,
        Save
	}
	public class ButtonBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public ButtonID buttonID;
		public ButtonAction buttonAction;
		public ButtonType buttonType;

		public Button button;
		// Use this for initialization
		public virtual void Start ()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(delegate { DoButtonAction(); });
		}
	
		public void OnPointerDown (PointerEventData eventData) {
			InputManager.Instance.ControlActive();

		}

		public void OnPointerUp (PointerEventData eventData)  {
			InputManager.Instance.ControlNotActive();
		}


		// Update is called once per frame
		public virtual void DoButtonAction ()
		{	
            EventManager.PostEvent(AzumiEventType.UITap, this);
            
    
            if (buttonType== ButtonType.MainWindowButton) {
				InputManager.Instance.MainButtonClicked(buttonID, buttonAction);
			} else if  (buttonType== ButtonType.ModalWindowButton) {
				InputManager.Instance.ModalButtonClicked(buttonID, buttonAction);
			}else {
				// print ("ERROR: Button type is "+ buttonType);
			}

		
		}
	}
}