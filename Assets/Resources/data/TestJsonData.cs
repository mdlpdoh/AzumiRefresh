using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
public class TestJsonData : MonoBehaviour {
	private string m_InGameLog = "";
	private Vector2 m_Position = Vector2.zero;

	private string datapath;
	// Use this for initialization
	void Start () {

		TextAsset bindata = Resources.Load("data/leveldata") as TextAsset;

		datapath = Application.persistentDataPath;
		print (datapath);
		var json = JSONNode.Parse(bindata.text);
		var jsonzero = json[0][0]["Levels"][0];




		var I = new JSONClass();

		I["version"].AsInt = 5;
		I["author"]["name"] = "Bunny83";
		I["author"]["phone"] = "0123456789";
		I["data"][-1] = "First item\twith tab";
		I["data"][-1] = "Second item";
		I["data"][-1]["value"] = "class item";
		I["data"].Add("Forth item");
		I["data"][1] = I["data"][1] + " 'addition to the second item'";
		//I.Add("version", "1.0");
		System.IO.File.WriteAllText(datapath + "/Bar.txt", I.ToString ());
		//var data = I.SaveToBase64();
		//I.SaveToFile (datapath + "//Foo.txt");
		print (I);
		P(I.ToString (""));

	}
	void P(string aText)
	{
		m_InGameLog += aText + "\n";
	}
	// Update is called once per frame
	void OnGUI()
	{
		m_Position = GUILayout.BeginScrollView(m_Position);
		GUILayout.Label(m_InGameLog);
		GUILayout.EndScrollView();
	}
}



