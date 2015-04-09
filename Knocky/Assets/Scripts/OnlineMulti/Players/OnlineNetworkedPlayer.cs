using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OnlineNetworkedPlayer : MonoBehaviour, NetworkedPlayer {
	#if UNITY_IPHONE
	private uint _score;
	private string _alias;
	private uint _goalNum;
	private tk2dTextMesh nameDisplay;
	private tk2dTextMesh scoreDisplay;
	
	/// <summary>
	/// Construct the specified player
	/// </summary>
	/// <param name='name'>
	/// Name: the name of the participant.
	/// </param>
	/// <param name='score'>
	/// Score, the score of the participant.
	/// </param>
	public void Construct(tk2dTextMesh nameDisplay, tk2dTextMesh scoreDisplay, uint score, OnlineAssets assets, uint goalNum){
		Debug.Log(nameDisplay.name);
		this.nameDisplay = nameDisplay;
		Debug.Log(scoreDisplay.name);
		this.scoreDisplay = scoreDisplay;
		Debug.Log(score);
		Score = score;
		_goalNum = goalNum;
		
		string[] _temp = new string[1];
		
		Debug.Log(GCGame._currentGame.participants);
		foreach(var p in GCGame._currentGame.participants){
			if( p.playerId != GameCenterBinding.playerIdentifier() ){
				Debug.Log(p.playerId);
				_temp[0] = p.playerId;
			}
		}
		
		//load that players data
		GameCenterManager.playerDataLoaded += PlayerDataLoaded;
		GameCenterManager.loadPlayerDataFailed += PlayerDataFailed;
		GameCenterBinding.loadPlayerData(_temp,false, false);
	}
	
	public void Shoot(Action CallBack){
		//PlaceHolder
	}
	
	#region GC Callbacks
	void PlayerDataLoaded(List<GameCenterPlayer> _player){
		foreach( GameCenterPlayer p in _player )
			Alias = p.alias;
		GameCenterManager.playerDataLoaded -= PlayerDataLoaded;
		GameCenterManager.loadPlayerDataFailed -= PlayerDataFailed;
	}
	void PlayerDataFailed(string error){
		Debug.Log(error);
		GameCenterManager.playerDataLoaded -= PlayerDataLoaded;
		GameCenterManager.loadPlayerDataFailed -= PlayerDataFailed;
		Alias = "Networked Player";
	}
	#endregion
	
	
	#region Get-Set
	public uint Score{
		get{
			return _score;
		}
		set{
			_score += value;
			scoreDisplay.text = _score.ToString();
			scoreDisplay.Commit();
			
			//workaround
			if(this.gameObject.name == "PlayerOne")
				GCGame._currentGameData.playerOneScore = _score;
			else 
				GCGame._currentGameData.playerTwoScore = _score;
		}
	}
	public string Alias{
		get{
			return _alias;
		}
		set{
			_alias = value;
			nameDisplay.text = _alias;
			nameDisplay.Commit();
		}
	}
	public uint Goal{
		get{ 
			return _goalNum;
		}
	}
	#endregion
#endif
}
