#define DEVELOPMENT

using UnityEngine;
using System.Collections;


/// <summary>
/// Application manager.
/// Responsible for all major scenes and their managers.
/// Adds and removes managers according to the current state.
/// </summary>
public class ApplicationManager : MonoBehaviour {
	
	#if(DEVELOPMENT)
	public enum debugLevel{
		MainMenu,
		OnePlayer,
		TwoPlayer,
		Options,
		GameCenter
	};
	public debugLevel debugger;
	public static bool isPersistent = false;
	#endif
		
	//current state of the application
	private ApplicationStates _ApplicationState;
	
	
	/// <summary>
	/// On application Ready: make sure this stay a singleton object
	/// Unity tries to duplicate Don't Destroy On Load Objects
	/// </summary>
	void Awake(){
		//set persistance
		if(!isPersistent){
			DontDestroyOnLoad(transform.gameObject);
			isPersistent = true;
		}else{
			Destroy(this.gameObject);
		}
	}
	/// <summary>
	/// Start this instance. Preps game assets.
	/// </summary>
	void Start() {
		

	#if(DEVELOPMENT)
		switch(debugger){
		default: 
		case debugLevel.MainMenu:
			ApplicationState = ApplicationStates.MainMenu;
			break;
		case debugLevel.OnePlayer:
			ApplicationState = ApplicationStates.OnePlayerGame;
			break;
		case debugLevel.TwoPlayer:
			ApplicationState = ApplicationStates.TwoPlayerGame;
			break;
		case debugLevel.Options:
			ApplicationState = ApplicationStates.Options;
			break;
		case debugLevel.GameCenter:
			ApplicationState = ApplicationStates.OnlineMatch;
			break;
		}
	#else
		//PRODUCTION
		ApplicationState = ApplicationStates.MainMenu;
	#endif

	}
	
	/// <summary>
	/// Gets or sets the state of the game.
	/// </summary>
	/// <value>
	/// The state of the application.
	/// </value>
	public ApplicationStates ApplicationState{
		get{ 
			return _ApplicationState;
		}
		set{
			_ApplicationState = value;
			SwitchApplicationState();
		}
	}
	
	/// <summary>
	/// Switchs the state of the application. Broadcasts messages to all subscribers
	/// </summary>
	private void SwitchApplicationState(){
				
		switch (_ApplicationState) {
		default:
			Debug.Log("WARNING: No State");
			break;
		case ApplicationStates.MainMenu:
			if(Application.loadedLevel != 0)
				Application.LoadLevel("MainMenu");
			break;
		case ApplicationStates.OnePlayerGame:
			Application.LoadLevel("OnePlayer");
			break;
		case ApplicationStates.TwoPlayerGame:
			Application.LoadLevel("TwoPlayer");
			break;
		case ApplicationStates.OnlineMatch:
			Debug.Log("online scene");
			Application.LoadLevel("OnlineMultiPlayer");
			break;
		}
		
	}
}
