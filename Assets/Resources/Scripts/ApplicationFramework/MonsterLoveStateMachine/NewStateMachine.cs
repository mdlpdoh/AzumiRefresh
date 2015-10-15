using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;



public class NewStateMachine : StateBehaviour {
	public enum States
	{
		Init, 
		Play, 
		Win, 
		Lose
	}
	// Use this for initialization
	void Awake () {
		Initialize<States>();
		ChangeState(States.Init);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void Init_Enter()
	{
		Debug.Log("We are now ready");
	}
	IEnumerator Play_Enter()
	{
		Debug.Log("Game Starting in 3");
		yield return new WaitForSeconds(1);
		
		Debug.Log("Game Starting in 2");
		yield return new WaitForSeconds(1);
		
		Debug.Log("Game Starting in 1");
		yield return new WaitForSeconds(1);
		
		Debug.Log("Start"); 
	}
	
	void Play_Update()
	{
		Debug.Log("Game Playing");
	}
	
	void Play_Exit()
	{
		Debug.Log("Game Over");
	}
}

