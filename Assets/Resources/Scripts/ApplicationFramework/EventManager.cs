using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.dogOnaHorse
{
//-----------------------------------------------------------
//Enum defining all possible game events
//More events should be added to the list
	public enum AzumiEventType 	{
		//Level events
		unassigned,
		HitWall,
		HitCollectible,
		HitPowerUp,
		HitTrigger,
		HitDoor, 
		GameTap,
		GameSwipe,
		SetCoins,
		SetBounces,
		OutOfBounces


	}
//-----------------------------------------------------------
//Singleton EventManager to send events to listeners
//Works with IListener implementations
public class EventManager : MonoBehaviour
{
	#region C# properties
	//-----------------------------------------------------------
	//Public access to instance
	public static EventManager Instance
	{
		get{return instance;}
		set{}
	}
	#endregion

	#region variables
	//Internal reference to Notifications Manager instance (singleton design pattern)
	private static EventManager instance = null;

	// Declare a delegate type for events
	public delegate void OnEvent(AzumiEventType AzumiEventType, Component Sender, object Param = null);

	//Array of listener objects (all objects registered to listen for events)
	private Dictionary<AzumiEventType, List<OnEvent>> Listeners = new Dictionary<AzumiEventType, List<OnEvent>>();
	#endregion
	//-----------------------------------------------------------
	#region methods
	//Called at start-up to initialize
	void Awake()
	{
		//If no instance exists, then assign this instance
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject); //Prevent object from being destroyed on scene exit
		}
		else //Instance already exists, so destroy this one. This should be a singleton object
			DestroyImmediate(this);
	}
	//-----------------------------------------------------------
	/// <summary>
	/// Function to add specified listener-object to array of listeners
	/// </summary>
	/// <param name="AzumiEventType">Event to Listen for</param>
	/// <param name="Listener">Object to listen for event</param>
	public void AddListener(AzumiEventType AzumiEventType, OnEvent Listener)
	{
		//List of listeners for this event
		List<OnEvent> ListenList = null;

		//New item to be added. Check for existing event type key. If one exists, add to list
		if(Listeners.TryGetValue(AzumiEventType, out ListenList))
		{
			//List exists, so add new item
			ListenList.Add(Listener);
			return;
		}

		//Otherwise create new list as dictionary key
		ListenList = new List<OnEvent>();
		ListenList.Add(Listener);
		Listeners.Add(AzumiEventType, ListenList); //Add to internal listeners list
	}
	//-----------------------------------------------------------
	/// <summary>
	/// Function to post event to listeners
	/// </summary>
	/// <param name="AzumiEventType">Event to invoke</param>
	/// <param name="Sender">Object invoking event</param>
	/// <param name="Param">Optional argument</param>
		/// 
	public static void PostEvent(AzumiEventType azumiEventType, Component Sender, object Param = null) {
		Instance.PostNotification( azumiEventType,  Sender,  Param );
	}

	public static void ListenForEvent(AzumiEventType azumiEventType, OnEvent Listener) {
		Instance.AddListener( azumiEventType, Listener);
	}

	public void PostNotification(AzumiEventType AzumiEventType, Component Sender, object Param = null)
	{
		//Notify all listeners of an event

		//List of listeners for this event only
		List<OnEvent> ListenList = null;

		//If no event entry exists, then exit because there are no listeners to notify
		if(!Listeners.TryGetValue(AzumiEventType, out ListenList))
			return;

		//Entry exists. Now notify appropriate listeners
		for(int i=0; i<ListenList.Count; i++)
		{
			if(!ListenList[i].Equals(null)) //If object is not null, then send message via interfaces
				ListenList[i](AzumiEventType, Sender, Param);
		}
	}

	public static void	ClearGameLevelListeners(){
		print ("Clearing Game Listeners");
		Instance.RemoveEvent(AzumiEventType.GameTap);
		Instance.RemoveEvent(AzumiEventType.GameSwipe);
		Instance.RemoveEvent(AzumiEventType.SetCoins);
		Instance.RemoveEvent(AzumiEventType.SetBounces);
		Instance.RemoveEvent(AzumiEventType.OutOfBounces);
		Instance.RemoveEvent(AzumiEventType.HitWall);
		Instance.RemoveEvent(AzumiEventType.HitCollectible);
		Instance.RemoveEvent(AzumiEventType.HitPowerUp);
		Instance.RemoveEvent(AzumiEventType.HitTrigger);
		Instance.RemoveEvent(AzumiEventType.HitDoor);
	}


	//-----------------------------------------------------------
	//Remove event type entry from dictionary, including all listeners
	public void RemoveEvent(AzumiEventType AzumiEventType)
	{
		//Remove entry from dictionary
		Listeners.Remove(AzumiEventType);
	}
	//-----------------------------------------------------------
	//Remove all redundant entries from the Dictionary
	public void RemoveRedundancies()
	{
		//Create new dictionary
		Dictionary<AzumiEventType, List<OnEvent>> TmpListeners = new Dictionary<AzumiEventType, List<OnEvent>>();
		
		//Cycle through all dictionary entries
		foreach(KeyValuePair<AzumiEventType, List<OnEvent>> Item in Listeners)
		{
			//Cycle through all listener objects in list, remove null objects
			for(int i = Item.Value.Count-1; i>=0; i--)
			{
				//If null, then remove item
		
				if(Item.Value[i].Equals(null))
					Item.Value.RemoveAt(i);
			}
			
			//If items remain in list for this notification, then add this to tmp dictionary
			if(Item.Value.Count > 0)
				TmpListeners.Add (Item.Key, Item.Value);
		}
		
		//Replace listeners object with new, optimized dictionary
		Listeners = TmpListeners;
	}
	//-----------------------------------------------------------
	//Called on scene change. Clean up dictionary
	void OnLevelWasLoaded()
	{

		RemoveRedundancies();
	}
	//-----------------------------------------------------------
	#endregion
}
}
