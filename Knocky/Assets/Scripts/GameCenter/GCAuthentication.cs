using UnityEngine;
using System.Collections;

/// <summary>
/// Handles Login.
/// </summary>
public class GCAuthentication: MonoBehaviour {
	#if UNITY_IPHONE
	public static bool isPersistent = false;
	
	/// <summary>
	/// Awake this instance: listens for player auth.
	/// </summary>
	void Awake(){
		GameCenterManager.playerAuthenticated += AuthorizeLocalUserSuccess;
		GameCenterManager.playerFailedToAuthenticate += AuthorizeUserSuccessFailed;
	}
	/// <summary>
	/// CONSTRUCTOR: Authorizes the player.
	/// </summary>
	void Start(){
		if(!GameCenterBinding.isPlayerAuthenticated())
			GameCenterBinding.authenticateLocalPlayer();
	}
	/// <summary>
	/// DECONSTRUCTOR: removes events. Only needed in removed at run time
	/// </summary>
	void OnDisable(){
//		GameCenterManager.playerAuthenticated -= AuthorizeLocalUserSuccess;
//		GameCenterManager.playerFailedToAuthenticate -= AuthorizeUserSuccessFailed;
	}
	
	#region GC Callbacks
	/// <summary>
	/// Authorizes has been successful.
	/// </summary>
	void AuthorizeLocalUserSuccess(){
		GameCenterManager.playerAuthenticated -= AuthorizeLocalUserSuccess;
		GameCenterManager.playerFailedToAuthenticate -= AuthorizeUserSuccessFailed;
	}
	/// <summary>
	/// Authorizes the user success failed.
	/// </summary>
	void AuthorizeUserSuccessFailed(string error){
		Debug.Log(error);
	}
	#endregion
	
	
#endif
	
}
