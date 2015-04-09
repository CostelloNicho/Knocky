using UnityEngine;
using System.Collections;

/// <summary>
/// Player interface.
/// </summary>
public interface PlayerInterface{
	
	/// <summary>
	/// Gets or sets the score.
	/// </summary>
	/// <value>
	/// The players score.
	/// </value>
	uint Score{
		get;
		set;
	}
	
	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	/// <value>
	/// The players name to be shown on HUD.
	/// </value>
	string Name{
		get;
		set;
	}
	
}
