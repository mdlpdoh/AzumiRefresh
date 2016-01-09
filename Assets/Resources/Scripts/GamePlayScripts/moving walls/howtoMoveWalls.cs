using UnityEngine;
using System.Collections;

public class howtoMoveWalls : MonoBehaviour {
	//To implement moving walls:

	// 1. create an empty game object and call it for ex "Platform Path1"

	// 2. Give Platform Path1 the script called "wallMovement"

	// 3. create empty game object children to Platform Path1 (as many for as many points you need on path)
	//    for example call one "Start" and another one "End" and place them where you want the wall to start and stop moving.

	// 4. Drag from the Prefab folder a "movingWall" prefab onto the Scene and place it over the Start empty Game Object.

	// 5. It should have a script on it called "followPath" (it has several public variables such as Type, Path, Speed etc))

	// 6. In the Inspector drag the game object "Platform Path1" onto the Path. 

	// 7. Start the game and you should see the movingWall prefab move from the start to end game objects.

}
