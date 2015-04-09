using UnityEngine;
using System.Collections;
using System;

public class OnlineGameManager : MonoBehaviour {
	public OnlineAssets assets;
	
	//players
	private NetworkedPlayer networkPlayer;
	private NetworkedPlayer localPlayer;
	private NetworkedPlayer currentPlayer;
	
	//flags
	private bool isWaitingForPuckStop;
	
	//Action
	private Action PlayerShotCallBack;
	private Action PuckStopedAction;
	
	
	#region Construct Destruct
	/// <summary>
	/// Constructor.
	/// </summary>
	void Start () {
		PlayerShotCallBack = PuckInMotion;
		isWaitingForPuckStop = false;
		//Events
		GameCenterTurnBasedManager.handleTurnEventEvent += HandlePlayerTurnCalled;
		GameCenterTurnBasedManager.loadMatchDataEvent += HandleLoadedData;
		Messenger.AddListener<GameObject>( "Goal", GoalHandler );
		Messenger.AddListener<bool>("HomeButtonPressed", OnHomeButton);
		//init players
		if(GCGame._currentGameData.playerOneID == GameCenterBinding.playerIdentifier()){
			localPlayer = assets.PlayerOne.AddComponent("OnlineLocalPlayer") as NetworkedPlayer;
			localPlayer.Construct(assets.PlayerOneName, assets.PlayerOneScore, GCGame._currentGameData.playerOneScore, assets, 1);
			networkPlayer = assets.PlayerTwo.AddComponent("OnlineNetworkedPlayer") as NetworkedPlayer;
			networkPlayer.Construct(assets.PlayerTwoName, assets.PlayerTwoScore, GCGame._currentGameData.playerTwoScore, assets, 2);
		}
		else{
			localPlayer = assets.PlayerTwo.AddComponent("OnlineLocalPlayer") as NetworkedPlayer;
			localPlayer.Construct(assets.PlayerTwoName, assets.PlayerTwoScore, GCGame._currentGameData.playerTwoScore, assets, 2);
			networkPlayer = assets.PlayerOne.AddComponent("OnlineNetworkedPlayer") as NetworkedPlayer;
			networkPlayer.Construct(assets.PlayerOneName, assets.PlayerOneScore, GCGame._currentGameData.playerOneScore, assets, 1);
		}
		
		//Set Turns Based on senario
		if(GameCenterTurnBasedBinding.isCurrentPlayersTurn()){
			currentPlayer = networkPlayer;
			PuckStopedAction = LocalPlayerTurn;
			SyncPuckPositionVelocity();
		}else{
			assets.Puck.position = new Vector3(GCGame._currentGameData.PosX,
											GCGame._currentGameData.PosY,
											GCGame._currentGameData.PosZ);
			currentPlayer = networkPlayer;
			Debug.Log("Wait on online player");
		}
	}
	
	private void switchTurns(){
		if(currentPlayer.Equals(localPlayer)){
			Debug.Log("switch to network player");
			currentPlayer = networkPlayer;
		}else{
			currentPlayer = localPlayer;
			Debug.Log("switch to local player");
		}
	}
	
	/// <summary>
	/// Destructor.
	/// </summary>
	void OnDestroy(){
		GameCenterTurnBasedManager.handleTurnEventEvent -= HandlePlayerTurnCalled;
		GameCenterTurnBasedManager.loadMatchDataEvent -= HandleLoadedData;
		Messenger.RemoveListener<GameObject>( "Goal", GoalHandler );
		Messenger.RemoveListener<bool>("HomeButtonPressed", OnHomeButton);
		if(isWaitingForPuckStop)
			StopCoroutine("WaitForPuckStop");

	}
	#endregion
	
	#region GC Callbacks
	/// <summary>
	/// Handles the player turn called.
	/// </summary>
	/// <param name='match'>
	/// Turn based match Match.
	/// </param>
	void HandlePlayerTurnCalled( GKTurnBasedMatch match){
		if(GameCenterTurnBasedBinding.isCurrentPlayersTurn()){
			if(GCGame._currentGame.matchId == match.matchId){
				GameCenterTurnBasedBinding.loadMatchData();
			}
		}
	}
	/// <summary>
	/// Handles the loaded data.
	/// </summary>
	/// <param name='bytes'>
	/// Bytes.
	/// </param>
	void HandleLoadedData( Byte[] bytes ){
		byte[] data = bytes;
		object d = NetworkHelper.DeserializeGamePkt<GCDataPacket>(data);
		GCGame._currentGameData = (GCDataPacket)d;
		currentPlayer = networkPlayer;
		PuckStopedAction = LocalPlayerTurn;
		SyncPuckPositionVelocity();
	}
	#endregion
	
	/// <summary>
	/// Syncs the puck position and shoots for the network player.
	/// </summary>
	private void SyncPuckPositionVelocity(){
		assets.Puck.position = new Vector3(GCGame._currentGameData.PosX,
											GCGame._currentGameData.PosY,
											GCGame._currentGameData.PosZ);
		assets.Puck.rigidbody.velocity = new Vector3(GCGame._currentGameData.VelX,
														GCGame._currentGameData.VelY,
														GCGame._currentGameData.VelZ);
		PuckInMotion();
		
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
		switchTurns();
		PuckStopedAction();
	}
	#endregion
	
	#region Actions
	/// <summary>
	/// Callback for the player to start corotine to test for 
	/// the puck stoping and change turns
	/// </summary>
	private void PuckInMotion(){
		Debug.Log("Puck In Motion");
		if(isWaitingForPuckStop)
			StopCoroutine("WaitForPuckStop");
		StartCoroutine("WaitForPuckStop");
	}
	private void LocalPlayerTurn(){
		//let the player shoot
		Debug.Log("run local player turn");
		currentPlayer = localPlayer;
		PuckStopedAction = TurnFinishedSendNetworkData;
		localPlayer.Shoot(PlayerShotCallBack);
		
	}
	private void TurnFinishedSendNetworkData(){
		
		//Adjust score accordingly
		Debug.Log("Data Sent");
		
		foreach( var p in GCGame._currentGame.participants ){
			if(p.playerId != GameCenterBinding.playerIdentifier() ){
				if( p.playerId == null )
					GameCenterTurnBasedBinding.endTurnWithNextParticipant( null, NetworkHelper.SerializeGamePkt<GCDataPacket>(GCGame._currentGameData));
				else
					GameCenterTurnBasedBinding.endTurnWithNextParticipant( p.playerId, NetworkHelper.SerializeGamePkt<GCDataPacket>(GCGame._currentGameData));
			}
		}
		
		
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
			if(currentPlayer.Goal == 1)
				currentPlayer.Score = 1;
			break;
		case "goal2":
			if(currentPlayer.Goal == 2)
				currentPlayer.Score = 1;
			break;
		}
		assets.Puck.rigidbody.velocity = Vector3.zero;
		assets.Puck.position = assets.PuckCenter;
		PuckStopedAction();
		switchTurns();
		
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

