﻿using UnityEngine;
using System.Collections;

namespace com.dogOnaHorse
{
	public enum WindowState
	{
		closed,
		open
	}

	public class ModalWindow : MonoBehaviour
	{
		public  WindowState myWindowState = WindowState.closed;
		public ButtonID buttonID;

		// Use this for initialization
		void Awake ()
		{
			implementCurrentWindowState ();
		}

		void implementCurrentWindowState ()
		{
			if (myWindowState == WindowState.closed) {
				gameObject.SetActive (false);
			} else {
				gameObject.SetActive (true);
			}
		}


		public void DoButtonAction (ButtonAction buttonAction)
		{
			switch (buttonAction) {
			case ButtonAction.OpenModal:
				Open ();
				break;
			case ButtonAction.CloseModal:
				Close ();
				break;
			 default:
				break;
			
			}
		}


		public void Open ()
		{
			myWindowState = WindowState.open;
			implementCurrentWindowState ();
		}

		public void Close ()
		{

			myWindowState = WindowState.closed;
			implementCurrentWindowState ();
		}
			
	}
}
