using UnityEngine;
using System.Collections;
using System;

public interface IPlayerEntity {
	
	/// <summary>
	/// Raises the current players turn event.
	/// </summary>
	/// <param name='callback'>
	/// Callback.
	/// </param>
	void OnTurn(Action callback);
	
	uint Score{
		get;
		set;
	}
}
