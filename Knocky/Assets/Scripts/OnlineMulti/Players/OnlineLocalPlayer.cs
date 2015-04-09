using UnityEngine;
using System.Collections;
using System;

public class OnlineLocalPlayer : MonoBehaviour, NetworkedPlayer {
	#if UNITY_IPHONE
	private uint _score;
	private string _alias;
	private uint _goalNum;
	private tk2dTextMesh nameDisplay;
	private tk2dTextMesh scoreDisplay;
	private InputScheme currentInput;
	private OnlineAssets assets;
	
	private Action turnComplete;
	private Action lineDragCallback;
	private Action puckShotCallback;
	
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
		this.assets = assets;
		this.nameDisplay = nameDisplay;
		this.scoreDisplay = scoreDisplay;
		Score = score;
		_goalNum = goalNum;
		
		Alias = GameCenterBinding.playerAlias();
		
		//set Callbacks
		lineDragCallback = OnDragFinished;
		puckShotCallback = OnPuckShot;
		if(currentInput)
			Destroy(currentInput);
	}
	
	public void Shoot(Action Callback){
		turnComplete = Callback;
		if(PuckAlgorithms.CheckPuckPosition(assets.Puck.position, assets.lineLeft ,
			assets.lineRight, assets.Radius )){
			currentInput = gameObject.AddComponent("LineDragInput") as InputScheme;
			currentInput.Enable(lineDragCallback, assets.PuckAimLine, assets.Puck, assets.LineIndicators);
		}else{
			Debug.Log("shotInputAdded");
			currentInput = gameObject.AddComponent("ShotInput") as InputScheme;
			currentInput.Enable(puckShotCallback, assets.PuckAimLine, assets.Puck, assets.LineIndicators);
		}
	}
	
	
		#region Actions
	/// <summary>
	/// Raises the puck shot event, ending the players turn
	/// </summary>
	void OnPuckShot(){
		Debug.Log("Shot");
		Destroy(currentInput);
		Vector3 _puckRelease = PuckAlgorithms.ConvertPosition( assets.Puck.position );
		Vector3 _shotVelocity = PuckAlgorithms.CalculateShotVector( assets.Puck.position, _puckRelease );
		assets.Puck.rigidbody.velocity = _shotVelocity;
		
		GCGame._currentGameData.VelX = _shotVelocity.x;
		GCGame._currentGameData.VelY = _shotVelocity.y;
		GCGame._currentGameData.VelZ = _shotVelocity.z;
		GCGame._currentGameData.PosX = assets.Puck.position.x;
		GCGame._currentGameData.PosY = assets.Puck.position.y;
		GCGame._currentGameData.PosZ = Layers.gameLayer;
		
		turnComplete();
	}
	
	/// <summary>
	/// Raises the drag finished event, enabling the player to switch
	/// over to shooting mode.
	/// </summary>
	void OnDragFinished(){
		currentInput = gameObject.AddComponent("ShotInput") as InputScheme;
		currentInput.Enable(puckShotCallback, assets.PuckAimLine, assets.Puck, assets.LineIndicators);
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
			
			//work around
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
