using UnityEngine;
using System.Collections;
using System;

public class TwoPlayerManager : MonoBehaviour {
	
	#region memeber variables
	public TwoPlayerAssets assets;
	public Action puckReleaseCallback;
	private IPlayerEntity player1;
	private IPlayerEntity player2;
	private IPlayerEntity currentPlayer;
	private bool isWaitingForPuckStop;
	#endregion
	
	/// <summary>
	/// CONSTRUCT: Build the one player game
	/// </summary>
	void Start () {
		puckReleaseCallback = shotReleased;
		assets.Puck.position = assets.PuckCenter;
		
		player1 = assets.PlayerOne;
		player2 = assets.PlayerTwo;
		
		currentPlayer = player1;
		currentPlayer.OnTurn(this.puckReleaseCallback);
		Messenger.AddListener<GameObject>( "Goal", GoalHandler );
		Messenger.AddListener<bool>("HomeButtonPressed", OnHomeButton);
		
	}
	
	/// <summary>
	/// DECONSTRUCT.
	/// </summary>
	void OnDisable(){
		if(isWaitingForPuckStop)
			StopCoroutine("WaitForPuckStop");
		Messenger.RemoveListener<GameObject>( "Goal", GoalHandler );
		Messenger.RemoveListener<bool>("HomeButtonPressed", OnHomeButton);
		
	}
	
	/// <summary>
	/// Switchs the players turn.
	/// </summary>
	private void SwitchTurns(){
		if( currentPlayer == player1 ){
			currentPlayer = player2;
			currentPlayer.OnTurn(puckReleaseCallback);
		}else{
			currentPlayer = player1;
			currentPlayer.OnTurn(puckReleaseCallback);
		}
		
	}
	
	
	#region Coroutines
	/// <summary>
	/// Waits for puck stop after it has been released by a player.
	/// </summary>
	private IEnumerator WaitForPuckStop(){
		isWaitingForPuckStop = true;
		while( assets.Puck.rigidbody.velocity.magnitude > 0){
			if(assets.Puck.rigidbody.velocity.magnitude <= assets.PUCK_SPEED_MIN){
				assets.Puck.rigidbody.velocity = Vector3.zero;
			}
			yield return null;
		}
		isWaitingForPuckStop = false;
		SwitchTurns();
	}
	
	#endregion
	
	#region Callbacks
	/// <summary>
	/// The turn is over and the shot has been released.
	/// </summary>
	public void shotReleased(){
		if(isWaitingForPuckStop)
			StopCoroutine("WaitForPuckStop");
		StartCoroutine("WaitForPuckStop");
	}
	#endregion
	
	#region EventListeners
	/// <summary>
	/// Handles the goal case. 
	/// </summary>
	/// <param name='goal'>
	/// Goal: the goal of which the puck was scored in.
	/// </param>
	void GoalHandler( GameObject goal ){
		if(isWaitingForPuckStop){
			StopCoroutine("WaitForPuckStop");
			isWaitingForPuckStop = false;
		}
		
		switch(goal.name.ToString()){
		default:
			Debug.Log("Error: No goal Connected");
			break;
		case "goal1":
			if(currentPlayer.Equals(player1))
				currentPlayer.Score = 1;
			break;
		case "goal2":
			if(currentPlayer.Equals(player2))
				currentPlayer.Score = 1;
			break;
		}
		
		assets.Puck.rigidbody.velocity = Vector3.zero;
		assets.Puck.position = assets.PuckCenter;
		
		SwitchTurns();
	}
	
	/// <summary>
	/// Raises the home button event.
	/// </summary>
	/// <param name='flag'>
	/// Flag.
	/// </param>
	void OnHomeButton(bool flag){
		GameObject.Find("ApplicationManager").GetComponent<ApplicationManager>().ApplicationState = ApplicationStates.MainMenu;
	}
	
	#endregion
	
}
