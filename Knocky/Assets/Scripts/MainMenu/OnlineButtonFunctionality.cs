using UnityEngine;
using System.Collections;

/// <summary>
/// Online button functionality.
/// </summary>
public class OnlineButtonFunctionality : MonoBehaviour {

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		GameCenterManager.playerAuthenticated += AuthorizeLocalUserSuccess;
		GameCenterManager.playerFailedToAuthenticate += AuthorizeUserSuccessFailed;
		
		//Start in In-active state
		AuthorizeUserSuccessFailed("");
	}
	
	#region GC Callbacks
	/// <summary>
	/// Authorizes has been successful.
	/// </summary>
	void AuthorizeLocalUserSuccess(){
		GameCenterManager.playerAuthenticated -= AuthorizeLocalUserSuccess;
		GameCenterManager.playerFailedToAuthenticate -= AuthorizeUserSuccessFailed;
		
		Color ActiveColor = new Color();
		ActiveColor = this.gameObject.GetComponent<tk2dSprite>().color;
		ActiveColor.a = 0.784f;
		gameObject.GetComponent<tk2dSprite>().color = ActiveColor;
		
		ActiveColor = this.gameObject.GetComponentInChildren<tk2dTextMesh>().color;
		ActiveColor.a = 0.784f;
		gameObject.GetComponentInChildren<tk2dTextMesh>().color = ActiveColor;
		
		gameObject.AddComponent<ButtonFunctionality>();
	}
	/// <summary>
	/// Authorizes the user success failed.
	/// </summary>
	void AuthorizeUserSuccessFailed(string error){
		
		Color ActiveColor = new Color();
		ActiveColor = this.gameObject.GetComponent<tk2dSprite>().color;
		ActiveColor.a = 0.392f;
		gameObject.GetComponent<tk2dSprite>().color = ActiveColor;
				
		ActiveColor = this.gameObject.GetComponentInChildren<tk2dTextMesh>().color;
		ActiveColor.a = 0.392f;
		gameObject.GetComponentInChildren<tk2dTextMesh>().color = ActiveColor;
		
	}
	#endregion
	

}
