using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GCMatchMaking : MonoBehaviour {
	#if UNITY_IPHONE
	
	public AssetsMainMenu assets;
	
	//flags
	private bool dataLoaded;
	private bool matchLoaded;
	
	/// <summary>
	/// CONSTRUCT: Listen for events
	/// </summary>
	void Start(){
		dataLoaded = false;
		matchLoaded = false;
		GameCenterTurnBasedManager.turnBasedMatchmakerViewControllerWasCancelledEvent += turnBasedMatchmakerViewControllerWasCancelledEvent;
		GameCenterTurnBasedManager.turnBasedMatchmakerViewControllerFailedEvent += turnBasedMatchmakerViewControllerFailedEvent;
		GameCenterTurnBasedManager.turnBasedMatchmakerViewControllerPlayerQuitEvent += turnBasedMatchmakerViewControllerPlayerQuitEvent;
		GameCenterTurnBasedManager.turnBasedMatchmakerViewControllerDidFindMatchEvent += turnBasedMatchmakerViewControllerDidFindMatchEvent;
		GameCenterTurnBasedManager.loadMatchDataEvent += loadMatchDataEvent;
		GameCenterTurnBasedManager.loadMatchDataFailedEvent += loadMatchDataFailedEvent;
	}
	
	/// <summary>
	/// Destruct Remove all Listeners.
	/// </summary>
	void OnDisable(){
		GameCenterTurnBasedManager.turnBasedMatchmakerViewControllerWasCancelledEvent -= turnBasedMatchmakerViewControllerWasCancelledEvent;
		GameCenterTurnBasedManager.turnBasedMatchmakerViewControllerFailedEvent -= turnBasedMatchmakerViewControllerFailedEvent;
		GameCenterTurnBasedManager.turnBasedMatchmakerViewControllerPlayerQuitEvent -= turnBasedMatchmakerViewControllerPlayerQuitEvent;
		GameCenterTurnBasedManager.turnBasedMatchmakerViewControllerDidFindMatchEvent -= turnBasedMatchmakerViewControllerDidFindMatchEvent;
		GameCenterTurnBasedManager.loadMatchDataEvent -= loadMatchDataEvent;
		GameCenterTurnBasedManager.loadMatchDataFailedEvent -= loadMatchDataFailedEvent;
	}
	
	
	#region GCMatchMakingEvents

	void turnBasedMatchmakerViewControllerWasCancelledEvent()
	{
		Debug.Log( "turnBasedMatchmakerViewControllerWasCancelledEvent" );
	}


	void turnBasedMatchmakerViewControllerFailedEvent( string error )
	{
		Debug.Log( "turnBasedMatchmakerViewControllerFailedEvent: " + error );
	}


	void turnBasedMatchmakerViewControllerPlayerQuitEvent( GKTurnBasedMatch match )
	{
		Debug.Log( "turnBasedMatchmakerViewControllerPlayerQuitEvent: " + match );
	}


	void turnBasedMatchmakerViewControllerDidFindMatchEvent( GKTurnBasedMatch match )
	{
		Debug.Log( "turnBasedMatchmakerViewControllerDidFindMatchEvent: " + match );
		GCGame._currentGame = match;
		GameCenterTurnBasedBinding.changeCurrentMatch( GCGame._currentGame.matchId );
		
		matchLoaded = true;
		
		if(matchLoaded && dataLoaded)
			assets.AppManager.ApplicationState = ApplicationStates.OnlineMatch;
	}
	#endregion
	
	#region GCMatchDataListeners
	void loadMatchDataEvent( Byte[] bytes )
	{
		if(bytes.Length == 0){ //HANDLE NEW GAME CREATION
			GCGame._currentGameData = new GCDataPacket();
			GCGame._currentGameData.playerOneID = GameCenterBinding.playerIdentifier();
			GCGame._currentGameData.playerTwoID = "";
			foreach(var p in GCGame._currentGame.participants){
				if( p.playerId != GameCenterBinding.playerIdentifier() )
					GCGame._currentGameData.playerTwoID = p.playerId;
				Debug.Log(Time.time);
			}
			GCGame._currentGameData.playerOneScore = 0;
			GCGame._currentGameData.playerTwoScore = 0;
			GCGame._currentGameData.VelX = 0.0f;
			GCGame._currentGameData.VelY = 0.0f;
			GCGame._currentGameData.VelZ = 0.0f;
			GCGame._currentGameData.PosX = assets.PuckCenter.x;
			GCGame._currentGameData.PosY = assets.PuckCenter.y;
			GCGame._currentGameData.PosZ = assets.PuckCenter.z;
		}else{ //HANDLE EXISTING GAME SETUP via just changing out the current game
			byte[] data = bytes;
			object d = NetworkHelper.DeserializeGamePkt<GCDataPacket>(data);
			GCGame._currentGameData = (GCDataPacket)d;
		}
		GameCenterTurnBasedBinding.saveCurrentTurnWithMatchData(NetworkHelper.SerializeGamePkt<GCDataPacket>(GCGame._currentGameData));
		dataLoaded = true;
		//Set the application to enter the Online Game Mode
		if(matchLoaded && dataLoaded)
			assets.AppManager.ApplicationState = ApplicationStates.OnlineMatch;
	}
	
	void loadMatchDataFailedEvent( string error )
	{
		Debug.Log( "loadMatchDataFailedEvent: " + error );
	}
	#endregion
	
#endif
}