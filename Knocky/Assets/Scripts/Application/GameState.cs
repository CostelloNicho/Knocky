using UnityEngine;
using System.Collections;

/// <summary>
/// The states of the overall application
/// </summary>
public enum ApplicationStates{
		MainMenu,
		Options,
		OnePlayerGame,
		TwoPlayerGame,
		GameCenter,
		OnlineMatch
};

public enum OnlineGameState{
	NewGame,
	ExistingNoPlayer2,
	Existing
};

public enum Difficulty{
	veryEasy,
	easy,
	medium,
	Hard
}