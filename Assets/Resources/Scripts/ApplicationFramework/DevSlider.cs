using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace com.dogonahorse
{
	public class DevSlider : MonoBehaviour
	{

		private Text minLevelText;
		private Text maxLevelText;
		private InputField currentLevelInput;
		private Slider slider;

		private float minLevelFloat;
		private float maxLevelFloat;
		private float currentLevelFloat;
		

		private int minLevelInt;
		private int maxLevelInt;
		private int currentLevelInt;
		private bool isFloat;




		public delegate void floatValueChangedCallback (float newValue);
		public event  floatValueChangedCallback onFloatChanged;

		public delegate void intValueChangedCallback (int newValue);
		public event  intValueChangedCallback onIntChanged;


		// Use this for initialization
		void Awake ()
		{
			string myName = gameObject.name;
			minLevelText = GameObject.Find(myName+"/Min").GetComponent<Text>();
			maxLevelText = GameObject.Find(myName+"/Max").GetComponent<Text>();
			currentLevelInput = GameObject.Find(myName+"/InputField").GetComponent<InputField>();
			slider= GameObject.Find(myName+"/Slider").GetComponent<Slider>();

		}
	
		// Update is called once per frame
		public void SetStartValue(float startvalue)
		{
		

			isFloat = true;

			currentLevelFloat = startvalue;
			minLevelFloat = float.Parse(minLevelText.text);
			maxLevelFloat = float.Parse(maxLevelText.text);
		
			SetUpUIValues ();
		}

		public void  SetStartValue(int startvalue)
		{
			isFloat = false;	
			currentLevelInt = startvalue;
			minLevelInt =  int.Parse(minLevelText.text);
			maxLevelInt=  int.Parse(maxLevelText.text);

			SetUpUIValues ();
		}

		void SetUpUIValues ()
		{
			if (isFloat)
			{
				slider.minValue = minLevelFloat ;
				slider.maxValue = maxLevelFloat;
				slider.value = currentLevelFloat;
				currentLevelInput.text = currentLevelFloat.ToString();
			} else {
				slider.minValue =  (float)minLevelInt ;
				slider.maxValue =  (float)maxLevelInt;
				slider.value =  (float)currentLevelInt;
				currentLevelInput.text = currentLevelInt.ToString();
			}
		}
		public void SliderUpdate (float newValue)
		{
			if (isFloat)
			{
				currentLevelFloat = newValue;
				currentLevelInput.text = currentLevelFloat.ToString();
			
				onFloatChanged(currentLevelFloat);
			} else {
				currentLevelInt = (int)newValue;
				currentLevelInput.text = currentLevelInt.ToString();
			
				onIntChanged(currentLevelInt);
			}

		
		}
		public void InputUpdate ()
		{

			if (isFloat)
			{
				currentLevelFloat = float.Parse(currentLevelInput.text);
				slider.value  = currentLevelFloat;
				onFloatChanged(currentLevelFloat);
			} else {
				currentLevelInt = int.Parse(currentLevelInput.text);
				slider.value  =   (float)currentLevelInt;
				onIntChanged(currentLevelInt);
			}
		}
	}
}
