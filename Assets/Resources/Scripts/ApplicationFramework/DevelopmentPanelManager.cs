using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
namespace com.dogonahorse
{
	public class DevelopmentPanelManager : MonoBehaviour
	{



		public int MaxTaps = 0;
		public int TwoStarLevel;
		public int TwoStarBonus;
		public int ThreeStarLevel;
		public int ThreeStarBonus;
		private float minVelocity;
		private float ballMass;
		private float ballDrag;
		private float maxVelocity;
		public DevSlider forceSlider;
		public DevSlider pointsSlider;
		public DevSlider massSlider;
		public DevSlider dragSlider;
		public DevSlider clampSlider;

		private GameObject ball;
		private ScoreManager scoreManager;

		public Toggle hitWallsCostsPointsToggle;
		public Toggle actionsCostPointsToggle;

		public ToggleGroup smushOrSwipeGroup;
		public Toggle smushToggle;
		public Toggle swipeToggle;


		private bool smushEnabled;
		private bool hitWallsCostsPoints;
		private bool actionsCostPoints;



	
		// Use this for initialization
		 void Start ()
		{
		
			Init ();
		}

		
		public void Init ()
		{
		
			if (!PlayerPrefs.HasKey("smushEnabled")){
				smushEnabled = false;
				PlayerPrefs.SetString("smushEnabled",Convert.ToString(smushEnabled));
			} else {
				smushEnabled = Convert.ToBoolean(PlayerPrefs.GetString("smushEnabled"));
			}

			if (!PlayerPrefs.HasKey("hitWallsCostsPoints")){
				hitWallsCostsPoints = true;
				PlayerPrefs.SetString("hitWallsCostsPoints",Convert.ToString(hitWallsCostsPoints));
			} else {
				hitWallsCostsPoints = Convert.ToBoolean(PlayerPrefs.GetString("hitWallsCostsPoints"));
			}

			if (!PlayerPrefs.HasKey("playerActionsCostPoints")){
				actionsCostPoints = false;
				PlayerPrefs.SetString("playerActionsCostPoints",Convert.ToString(actionsCostPoints));
			} else {
				actionsCostPoints = Convert.ToBoolean(PlayerPrefs.GetString("actionsCostPoints"));
			}

			if (!PlayerPrefs.HasKey("maxVelocity")){
				maxVelocity = 3F;
				PlayerPrefs.SetFloat("maxVelocity",maxVelocity);
			} else {
				maxVelocity = PlayerPrefs.GetFloat("maxVelocity");
			}
			if (!PlayerPrefs.HasKey("ballMass")){
				ballMass = 1f;
				PlayerPrefs.SetFloat("ballMass",ballMass);
			} else {
				ballMass = PlayerPrefs.GetFloat("ballMass");
			}
			if (!PlayerPrefs.HasKey("ballDrag")){
				ballDrag = 0F;
				PlayerPrefs.SetFloat("ballDrag",ballDrag);
			} else {
				ballDrag = PlayerPrefs.GetFloat("ballDrag");
			}
			if (!PlayerPrefs.HasKey("minVelocity")){
				minVelocity = 0.5F;
				PlayerPrefs.SetFloat("minVelocity",minVelocity);
			} else {
				minVelocity = PlayerPrefs.GetFloat("minVelocity");
			}
			ball = GameObject.Find ("Ball");
			LevelManager.InitializateDevPanelValues (this);
			scoreManager  = GameObject.Find ("ScoreManager").GetComponent<ScoreManager>();
	
			if (MaxTaps == 0) {
				MaxTaps = scoreManager.MaxTaps;
			}
			SetUpUI ();
		}
		public void ResetPhysics ()
		{
			//PlayerPrefs.DeleteAll();
			
			minVelocity = 2000F;
			PlayerPrefs.SetFloat("minVelocity",minVelocity);
			ballMass = 1f;
			PlayerPrefs.SetFloat("ballMass",ballMass);
			ballDrag = 0F;
			PlayerPrefs.SetFloat("ballDrag",ballDrag);
			maxVelocity = 3F;

			PlayerPrefs.SetFloat("ballClamp",maxVelocity);

			SetUpUI ();
		}

		public void ResetGamePlays ()
		{
	
			//PlayerPrefs.DeleteAll();
			smushEnabled = true;
			PlayerPrefs.SetString("smushEnabled",Convert.ToString(smushEnabled));
			hitWallsCostsPoints = true;
			PlayerPrefs.SetString("hitWallsCostsPoints",Convert.ToString(hitWallsCostsPoints));
			actionsCostPoints = false;
			PlayerPrefs.SetString("playerActionsCostPoints",Convert.ToString(actionsCostPoints));
			LevelManager.InitializateDevPanelValues (this);
			if (MaxTaps == 0) {
				MaxTaps = scoreManager.MaxTaps;
			}
			SetUpUI ();
		}
		void OnDestroy(){
			//LevelManager.Instance.WriteLevelSettings();
			PlayerPrefs.SetFloat("minVelocity",minVelocity);
			PlayerPrefs.SetFloat("ballMass",ballMass);
			PlayerPrefs.SetFloat("ballDrag",ballDrag);
			PlayerPrefs.SetFloat("maxVelocity",maxVelocity);
			PlayerPrefs.SetString("smushEnabled",Convert.ToString(smushEnabled));
			PlayerPrefs.SetString("hitWallsCostsPoints",Convert.ToString(hitWallsCostsPoints));	
			PlayerPrefs.SetString("actionsCostPoints",Convert.ToString(actionsCostPoints));
			PlayerPrefs.Save();
		}
		// Update is called once per frame
		void SetUpUI ()
		{
			forceSlider.onFloatChanged += onForceChanged;
			clampSlider.onFloatChanged += onClampChanged;

			pointsSlider.onIntChanged += onPointsChanged;
			massSlider.onFloatChanged += onMassChanged;
			dragSlider.onFloatChanged += onDragChanged;


			forceSlider.SetStartValue (minVelocity);
			pointsSlider.SetStartValue (MaxTaps);
			massSlider.SetStartValue (ballMass);
			dragSlider.SetStartValue (ballDrag);
			clampSlider.SetStartValue (maxVelocity);
			hitWallsCostsPointsToggle.isOn = hitWallsCostsPoints;
			actionsCostPointsToggle.isOn = actionsCostPoints;
			scoreManager.SetHitWallsCostsPoints(hitWallsCostsPoints);
			scoreManager.PlayerActionsCostPoints(actionsCostPoints);
			if (smushEnabled) {
				smushToggle.isOn = true;
				swipeToggle.isOn = false;

			} else {
				smushToggle.isOn = false;
				swipeToggle.isOn = true;

			}
			InputManager.MainDirectionSelected =smushEnabled;

			//print ("InputManager.MainDirectionSelected "+ InputManager.MainDirectionSelected);
		}

		public void onSmushSelected ()
		{ 
			smushEnabled = true;
			InputManager.MainDirectionSelected =smushEnabled;
		}

		public void onSwipeSelected ()
		{ 
			smushEnabled = false;
			InputManager.MainDirectionSelected =smushEnabled;
		}

		public void onHittingWallsSelected ()
		{ 
			hitWallsCostsPoints = hitWallsCostsPointsToggle.isOn;
			scoreManager.SetHitWallsCostsPoints(hitWallsCostsPoints);
		}
		
		public void onPlayerActionsCostPointsSelected ()
		{ 
			actionsCostPoints = actionsCostPointsToggle.isOn;
			scoreManager.PlayerActionsCostPoints(actionsCostPoints);
		}

		public void onForceChanged (float newValue)
		{ 
			minVelocity = newValue;
			ball.GetComponent<AzumiBallRoll>().MinimumVelocity = minVelocity;
		}
		public void onPointsChanged (int newValue)
		{
			MaxTaps = newValue;
			scoreManager.ChangeMaxScore(MaxTaps);
			LevelManager.UpdateScore(MaxTaps);
		}
		public void onMassChanged (float newValue)
		{
			ballMass = newValue;
			ball.GetComponent<Rigidbody2D>().mass = ballMass;
		}
		public void onDragChanged (float newValue)
		{
			ballDrag = newValue;
			ball.GetComponent<Rigidbody2D>().drag = ballDrag;
		}
		public void onClampChanged (float newValue)
		{
			maxVelocity = newValue;
			ball.GetComponent<AzumiBallRoll>().MaximumVelocity = maxVelocity;
		}
	}
}