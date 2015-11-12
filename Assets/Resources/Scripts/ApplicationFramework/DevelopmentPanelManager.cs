using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
namespace com.dogOnaHorse
{
	public class DevelopmentPanelManager : MonoBehaviour
	{



		public int MaxTaps = 0;
		public int TwoStarLevel;
		public int TwoStarBonus;
		public int ThreeStarLevel;
		public int ThreeStarBonus;
		private float swipeForce;
		private float ballMass;
		private float ballDrag;
		private float ballClamp;
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


			if (!PlayerPrefs.HasKey("smushEnabled")){
				smushEnabled = true;
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

			if (!PlayerPrefs.HasKey("swipeForce")){
				swipeForce = 2000F;
				PlayerPrefs.SetFloat("swipeForce",swipeForce);
			} else {
				swipeForce = PlayerPrefs.GetFloat("swipeForce");
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
			if (!PlayerPrefs.HasKey("ballClamp")){
				ballClamp = 3F;
				PlayerPrefs.SetFloat("ballClamp",ballClamp);
			} else {
				ballClamp = PlayerPrefs.GetFloat("ballClamp");
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
			
			swipeForce = 2000F;
			PlayerPrefs.SetFloat("swipeForce",swipeForce);
			ballMass = 1f;
			PlayerPrefs.SetFloat("ballMass",ballMass);
			ballDrag = 0F;
			PlayerPrefs.SetFloat("ballDrag",ballDrag);
			ballClamp = 3F;

			PlayerPrefs.SetFloat("ballClamp",ballClamp);

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
			LevelManager.Instance.WriteLevelSettings();
			PlayerPrefs.SetFloat("swipeForce",swipeForce);
			PlayerPrefs.SetFloat("ballMass",ballMass);
			PlayerPrefs.SetFloat("ballDrag",ballDrag);
			PlayerPrefs.SetFloat("ballClamp",ballClamp);
			PlayerPrefs.SetString("smushEnabled",Convert.ToString(smushEnabled));
			PlayerPrefs.SetString("hitWallsCostsPoints",Convert.ToString(hitWallsCostsPoints));	
			PlayerPrefs.SetString("actionsCostPoints",Convert.ToString(actionsCostPoints));
			PlayerPrefs.Save();
		}
		// Update is called once per frame
		void SetUpUI ()
		{
	
			forceSlider.onFloatChanged += onForceChanged;
			pointsSlider.onIntChanged += onPointsChanged;
			massSlider.onFloatChanged += onMassChanged;
			dragSlider.onFloatChanged += onDragChanged;
			clampSlider.onFloatChanged += onClampChanged;

			forceSlider.SetStartValue (swipeForce);
			pointsSlider.SetStartValue (MaxTaps);
			massSlider.SetStartValue (ballMass);
			dragSlider.SetStartValue (ballDrag);
			clampSlider.SetStartValue (ballClamp);
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

			print ("InputManager.MainDirectionSelected "+ InputManager.MainDirectionSelected);
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
			 
		//	print ("onHittingWallsSelected " + hitWallsCostsPointsToggle.isOn );
			hitWallsCostsPoints = hitWallsCostsPointsToggle.isOn;
			scoreManager.SetHitWallsCostsPoints(hitWallsCostsPoints);
		}
		
		public void onPlayerActionsCostPointsSelected ()
		{ 
		//	print ("onPlayerActionsCostPointsSelected "+ ActionsCostPointsToggle.isOn);
			actionsCostPoints = actionsCostPointsToggle.isOn;
			scoreManager.PlayerActionsCostPoints(actionsCostPoints);
		}

		public void onForceChanged (float newValue)
		{ 
			swipeForce = newValue;
			ball.GetComponent<AzumiBallRoll>().onTapSpeed = swipeForce;
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
			ballClamp = newValue;
			ball.GetComponent<AzumiBallRoll>().clampSpeed = ballClamp;
		}
	}
}