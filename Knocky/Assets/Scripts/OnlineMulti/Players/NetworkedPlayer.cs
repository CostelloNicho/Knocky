using UnityEngine;
using System.Collections;
using System;

public interface NetworkedPlayer {
	
	
	void Construct(tk2dTextMesh nameDisplay, tk2dTextMesh scoreDisplay, uint score, OnlineAssets assets, uint goalNum);
	
	void Shoot(Action CallBack);
	
	string Alias{
		get;
		set;
	}
	uint Score{
		get;
		set;
	}
	
	uint Goal{
		get;
	}
	
	
	
}
