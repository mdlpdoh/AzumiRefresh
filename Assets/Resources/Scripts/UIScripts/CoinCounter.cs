using UnityEngine;
using System.Collections;

public class CoinCounter : MonoBehaviour {

private GameObject [] wallet;
private int coinAmt;
	// Use this for initialization
	void Start () {
        wallet = GameObject.FindGameObjectsWithTag("ant");
        print("The amount of ants in this level = " + wallet.Length);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
