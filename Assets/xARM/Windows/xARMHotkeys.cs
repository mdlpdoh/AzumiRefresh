#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using xARM;

public class xARMHotkeys : MonoBehaviour {

	[MenuItem ("Window/xARM/Export ScreenCap as PNG %&e", false, 210)]
	public static void ExportScreenCap () {
		xARMManager.SaveScreenCapFile ();
	}

	[MenuItem ("Window/xARM/Export all ScreenCaps as PNGs %#&e", false, 211)]
	public static void ExportAllScreenCaps () {
		xARMManager.SaveAllScreenCapFiles ();
	}
	
	[MenuItem ("Window/xARM/(Un)Hide Game View %&g", false, 220)]
	public static void HideGameView () {
		xARMManager.SwitchHideGameView ();
	}
}
#endif