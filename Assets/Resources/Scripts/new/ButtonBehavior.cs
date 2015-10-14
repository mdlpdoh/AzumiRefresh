using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace com.dogOnaHorse
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
		Connect
	}
	public enum ButtonType
	{
		Unassigned,
		MainWindowButton, 
		ModalWindowButton
	}

	public enum ButtonAction
	{
		Unassigned,
		OpenModal, 
		CloseModal,
		Select,
		Buy,
		NextScreen
	}
	public class ButtonBehavior : MonoBehaviour
	{
		public ButtonID buttonID;
		public ButtonAction buttonAction;
		public ButtonType buttonType;

		private Button button;
		// Use this for initialization
		void Start ()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(delegate { DoButtonAction(); });
		}
	
		// Update is called once per frame
		void DoButtonAction ()
		{	if (buttonType== ButtonType.MainWindowButton) {
				InputManager.Instance.MainButtonClicked(buttonID, buttonAction);
			} else if  (buttonType== ButtonType.ModalWindowButton) {
				InputManager.Instance.ModalButtonClicked(buttonID, buttonAction);
			}else {
				print ("ERROR: Button type is "+ buttonType);
			}

		
		}
	}
}