using UnityEngine;
using System.Collections;

/// <summary>
/// Main menu manager: controls the main menu scene.
/// </summary>
public class MainMenuManager : MonoBehaviour {
	
	public AssetsMainMenu assets;
	
	/// <summary>
	/// Start this instance. CONSTRUCTOR
	/// </summary>
	void Start(){
		Messenger.AddListener<GameObject>("MenuButtonPressed", MainMenuButtonHandler);
	}
	void OnDestroy(){
		//Messenger.RemoveListener<GameObject>("MenuButtonPressed", MainMenuButtonHandler);
	}
	
	/// <summary>
	/// Handles the  Main Menu button inputs.
	/// </summary>
	/// <param name='button'>
	/// Button.
	/// </param>
	void MainMenuButtonHandler( GameObject button ){
		string _btnName = button.name.ToString();
		switch(_btnName){
		case "OnePlayerBtn":
			SetOnePlayerGame();
			break;
		case "TwoPlayerBtn":
			SetTwoPlayerGame();
			break;
		case "OptionsBtn":
			Debug.Log("options");
			SetOptions();
			break;
		case "OnlineBtn":
			Debug.Log("online");
			SetOnlineMatchMaking();
			break;
		}
	}
	
	#region Event Callbacks
	/// <summary>
	/// Call back function that sets the one player game.
	/// </summary>
	void SetOnePlayerGame(){
		assets.AppManager.ApplicationState = ApplicationStates.OnePlayerGame;
	}
	/// <summary>
	/// Call back function that sets the two player game.
	/// </summary>
	void SetTwoPlayerGame(){
		assets.AppManager.ApplicationState = ApplicationStates.TwoPlayerGame;
	}
	/// <summary>
	/// Call back function that sets the options.
	/// </summary>
	void SetOptions(){
		assets.AppManager.ApplicationState = ApplicationStates.MainMenu;
	}
	/// <summary>
	/// Sets the callback for online multiplayer;
	/// </summary>
	void SetOnlineMatchMaking(){
		//Matchmaking happens here
		if(GameCenterBinding.isPlayerAuthenticated())
			if(GameCenterTurnBasedBinding.isTurnBasedMultiplayerAvailable())
				GameCenterTurnBasedBinding.findMatch( 2, 2, true);
	}
	#endregion
}
