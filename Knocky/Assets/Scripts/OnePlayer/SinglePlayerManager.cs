using UnityEngine;
using System.Collections;
using System;

public class SinglePlayerManager : MonoBehaviour {
	
	public SinglePlayerAssets assets;
	public Action puckReleaseCallback;
	private IPlayerEntity player1;
	private IPlayerEntity player2;
	private IPlayerEntity currentPlayer;
	private bool isWaitingForPuckStop;
	
	/// <summary>
	/// CONSTRUCT: Build the one player game
	/// </summary>
	void Start () {
		isWaitingForPuckStop = false;
		
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
	void OnDestroy(){
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
			StartCoroutine("WaitForComputer");
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
	/// <summary>
	/// Waits for computer, to take its shot.
	/// </summary>
	private IEnumerator WaitForComputer(){
		yield return new WaitForSeconds(assets.SHOT_DELAY);
		currentPlayer.OnTurn(puckReleaseCallback);
	}
	
	#endregion
	
	#region Callbacks
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
		if( player1.Score >= assets.ScoreToWin ){
			EndGame(true);
		}else if( player2.Score >= assets.ScoreToWin ){
			EndGame(false);
		}else{
			SwitchTurns();
		}
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
	
	#region endgame
	void EndGame(bool player)
    {
		assets.endScreen.transform.position = assets.PuckCenter;
		EasyTouch.On_TouchUp += (gesture) => {
			if(gesture.pickObject == assets.menuButton )
				GameObject.Find("ApplicationManager").GetComponent<ApplicationManager>().ApplicationState = ApplicationStates.MainMenu;
			else if (gesture.pickObject == assets.replayButton)
				GameObject.Find("ApplicationManager").GetComponent<ApplicationManager>().ApplicationState = ApplicationStates.OnePlayerGame;
		};
    }
 

//    void OnGUI()
//    {
//        if (assets.gameOver)
//        {
//            if(GUI.Button(new Rect(Screen.width/2 + 40, Screen.height/2 + 30, 4500, 80), assets.replayButton, GUIStyle.none))
//            {
//                Application.LoadLevel(1);
//            }
//
//            if (GUI.Button(new Rect(Screen.width / 2 + 40, Screen.height / 2 + 100, 4500, 80), assets.menuButton, GUIStyle.none))
//            {
//                Application.LoadLevel(0);
//            }
//        }
//    }
	#endregion
}
