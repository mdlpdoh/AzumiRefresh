using UnityEngine;
using UnityEditor;
public class UIToolbar : EditorWindow {
   
    [MenuItem ("Window/UI Toolbar")]
    static void Init () {
        EditorWindow.GetWindow (typeof (UIToolbar));
    }
    private int previousLayers;
    void OnGUI () {
        bool visibleUI = ((Tools.visibleLayers & (1 << 5)) == 1);
        if (GUILayout.Button (visibleUI ? "Hide UI" : "Show UI")) {
            Tools.visibleLayers = Tools.visibleLayers ^ (1 << 5);
        }
        if (GUILayout.Button ("Toggle All")) {
            int layers = Tools.visibleLayers;
            for (int i = 0; i < 32; i++) {
                layers ^= (1 << i);
            }
            Tools.visibleLayers = layers;
        }
    }
}