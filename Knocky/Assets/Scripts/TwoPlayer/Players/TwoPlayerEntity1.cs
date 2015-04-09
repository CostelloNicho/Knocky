using UnityEngine;
using System.Collections;
using System;

public class TwoPlayerEntity1 : MonoBehaviour, IPlayerEntity {

	public TwoPlayerAssets assets;
	private uint _score;
	private InputScheme currentInput;
	
	private Action turnComplete;
	private Action lineDragCallback;
	private Action puckShotCallback;
	

	/// <summary>
	/// CONSTRUCOR: Constructs a player instance
	/// </summary>
	void Start () {
		if(GameCenterBinding.isPlayerAuthenticated())
			assets.PlayerOneName.text = GameCenterBinding.playerAlias();
		else
			assets.PlayerOneName.text = "Player One";
		assets.PlayerOneName.Commit();
		_score = 0;
		Score = 0;
		lineDragCallback = OnDragFinished;
		puckShotCallback = OnPuckShot;
		if(currentInput)
			Destroy(currentInput);
	}
	
	/// <summary>
	/// Gets or sets the score.
	/// </summary>
	/// <value>
	/// The score.
	/// </value>
	public uint Score{
		get{
			return _score;
		}
		set{
			_score += value;
			assets.PlayerOneScore.text = _score.ToString();
			assets.PlayerOneScore.Commit();
		}
	}
	
	/// <summary>
	/// Raises the turn event for the current instance of the player.
	/// </summary>
	/// <param name='callback'>
	/// Callback: function to call when the shot has been completed.
	/// </param>
	public void OnTurn(Action callback){
		//set callback
		turnComplete = callback;
		if(PuckAlgorithms.CheckPuckPosition(assets.Puck.position, assets.lineLeft ,
			assets.lineRight, assets.Radius )){
			currentInput = gameObject.AddComponent("LineDragInput") as InputScheme;
			currentInput.Enable(lineDragCallback, assets.PuckAimLine, assets.Puck, assets.LineIndicators);
		}else{
			currentInput = gameObject.AddComponent("ShotInput") as InputScheme;
			currentInput.Enable(puckShotCallback, assets.PuckAimLine, assets.Puck, assets.LineIndicators);
		}
	}
	
	#region Actions
	/// <summary>
	/// Raises the puck shot event, ending the players turn
	/// </summary>
	void OnPuckShot(){
		Destroy(currentInput);
		Vector3 _puckRelease = PuckAlgorithms.ConvertPosition( assets.Puck.position );
		Vector3 _shotVelocity = PuckAlgorithms.CalculateShotVector( assets.Puck.position, _puckRelease );
		assets.Puck.rigidbody.velocity = _shotVelocity;
		turnComplete();
	}
	
	/// <summary>
	/// Raises the drag finished event, enabling the player to switch
	/// over to shooting mode.
	/// </summary>
	void OnDragFinished(){
		Destroy(currentInput);
		currentInput = gameObject.AddComponent("ShotInput") as InputScheme;
		currentInput.Enable(puckShotCallback, assets.PuckAimLine, assets.Puck, assets.LineIndicators);
	}
	#endregion
}