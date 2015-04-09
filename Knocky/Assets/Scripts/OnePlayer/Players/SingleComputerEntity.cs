using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SingleComputerEntity : MonoBehaviour, IPlayerEntity {
	
	public SinglePlayerAssets assets;
	private uint _score;
	private Action turnComplete;
	private List<ComputerShot> allShotVectors;
	
	/// <summary>
	/// CONSTRUTCTOR
	/// </summary>
	void Start () {
		assets.PlayerTwoName.text = "Computer";
		assets.PlayerTwoName.Commit();
		_score = 0;
		Score = _score;
		allShotVectors = new List<ComputerShot>();
	}
	
	/// <summary>
	/// DESTRUCTOR
	/// </summary>
	void OnDestroy(){
		allShotVectors.Clear();
	}
	
	#region GetSet
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
			assets.PlayerTwoScore.text = _score.ToString();
			assets.PlayerTwoScore.Commit();
		}
	}
	#endregion
	
	/// <summary>
	/// Raises the turn event for the current instance of the player.
	/// </summary>
	/// <param name='callback'>
	/// Callback: function to call when the shot has been completed.
	/// </param>
	public void OnTurn(Action callback){
		turnComplete = callback;
		
		GatherShotVectors();
	}
	
	private void GatherShotVectors(){
		Vector3 _vel;
		ComputerShot _shot;
		
		//Check for Straight shot, and add it to the list of shot vectors
		_vel = AI.CheckStraightShot(assets.Puck.position, assets.goalTarget.position, assets.Radius);
		if(_vel != Vector3.zero){
			_shot.velocity = _vel;
			_shot.weight = 1;
			_shot.shotType = "Straight";
			allShotVectors.Add(_shot);
		}
		
		//check for block shot and add it to the list of all shot vectors
		_vel = AI.CheckBlockShot(assets.Puck.position, assets.blockTarget.position);
		if(_vel != Vector3.zero){
			_shot.velocity = _vel;
			_shot.weight = 2;
			_shot.shotType = "Block";
			allShotVectors.Add(_shot);
		}
		
		//check corner shot and add it to the list of all shot vectors
		_vel = AI.CheckCornerShot(assets.Puck.position, assets.goalTarget.position,
			assets.leftCornerTop.position, assets.leftCornerBottom.position);
		if(_vel != Vector3.zero){
			_shot.velocity = _vel;
			_shot.weight = 3;
			_shot.shotType = "Corner";
			allShotVectors.Add(_shot);
		}
		
		//check top and bottom shots, add them to all shot vectors
		if(assets.Puck.position.x <= (1024 / 2)){
			//Check for Top-Wall Shot
			_vel = AI.CheckTopWallShot(assets.Puck.position, assets.goalTarget.position, 
				assets.goalReflectionTop.position, assets.Radius);
			if(_vel != Vector3.zero){
				_shot.velocity = _vel;
				if(assets.Puck.position.y > (768/2))
					_shot.weight = 4;
				else
					_shot.weight = 5;
				_shot.shotType = "UpperWall";
				allShotVectors.Add(_shot);
			}


			//Check for Bottom-Wall Shot
			_vel = AI.CheckBottomWallShot(assets.Puck.position, assets.goalTarget.position, 
				assets.goalReflectionBottom.position, assets.Radius);
			if(_vel != Vector3.zero){
				_shot.velocity = _vel;
				if(assets.Puck.position.y <= (768/2))
					_shot.weight = 4;
				else
					_shot.weight = 5;
				_shot.shotType = "lower Wall Shot";
				allShotVectors.Add(_shot);
			}
		}else{
			//Check for Top-Wall Shot
			_vel = AI.CheckTopWallShot(assets.Puck.position, assets.goalTarget.position, 
				assets.goalReflectionTop.position, assets.Radius);
			if(_vel != Vector3.zero){
				_shot.velocity = _vel;
				if(assets.Puck.position.y > (768/2))
					_shot.weight = 5;
				else
					_shot.weight = 4;
				_shot.shotType = "UpperWall";
				allShotVectors.Add(_shot);
			}


			//Check for Bottom-Wall Shot
			_vel = AI.CheckBottomWallShot(assets.Puck.position, assets.goalTarget.position, 
				assets.goalReflectionBottom.position, assets.Radius);
			if(_vel != Vector3.zero){
				_shot.velocity = _vel;
				if(assets.Puck.position.y <= (768/2))
					_shot.weight = 5;
				else
					_shot.weight = 4;
				_shot.shotType = "lower Wall Shot";
				allShotVectors.Add(_shot);
			}
		}
		
		//Apply randomness to choices based on the level of 
		System.Random rand = new System.Random();
		switch(assets.currentDifficulty){
		case Difficulty.veryEasy:
			for(int index = 0; index < allShotVectors.Count; ++index){
				ComputerShot _temp = allShotVectors[index];
				_temp.weight += rand.Next(0, assets.very);
				allShotVectors[index] = _temp;
			}
			break;
		case Difficulty.easy:
			for(int index = 0; index < allShotVectors.Count; ++index){
				ComputerShot _temp = allShotVectors[index];
				_temp.weight += rand.Next(0, assets.easy);
				allShotVectors[index] = _temp;
			}
			break;
		case Difficulty.medium:
			for(int index = 0; index < allShotVectors.Count; ++index){
				ComputerShot _temp = allShotVectors[index];
				_temp.weight += rand.Next(0, assets.medium);
				allShotVectors[index] = _temp;
			}
			break;
		case Difficulty.Hard:
			Debug.Log(assets.currentDifficulty + assets.hard.ToString());
			for(int index = 0; index < allShotVectors.Count; ++index){
				ComputerShot _temp = allShotVectors[index];
				_temp.weight += rand.Next(0, assets.hard);
				allShotVectors[index] = _temp;
			}
			break;
		}
		
		//LOG DATA
		string _log = "";
		foreach(ComputerShot i in allShotVectors){
			_log += i.shotType + " " + i.weight + " | ";
		}
		//Debug.Log(_log);
		
		
		//FIND the best shot by its weight
		if(allShotVectors.Count != 0){
			_shot = allShotVectors[0];
			foreach( ComputerShot i in allShotVectors )
				if(i.weight < _shot.weight)
					_shot = i;
			}
		else
			_shot.velocity = Vector3.zero;
		
		//shoot the puck
		assets.Puck.rigidbody.velocity = _shot.velocity;
		allShotVectors.Clear();
		_shot = new ComputerShot();
		turnComplete();
	}
	
}
