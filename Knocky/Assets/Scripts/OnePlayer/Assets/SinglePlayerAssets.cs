using UnityEngine;
using System.Collections;

/// <summary>
/// Single player assets holds all necessary objects for the single player game.
/// </summary>
public class SinglePlayerAssets : MonoBehaviour {
	
	#region additions
    public GameObject endScreen;
    public GameObject replayButton;
    public GameObject menuButton;
    public bool gameOver;
	#endregion

	#region Game Settings
	public int ScoreToWin;
	#endregion
	
	#region Game Assets
	public Transform Puck;
	public Transform LineIndicators;
	public LineRenderer PuckAimLine;
	#endregion
	
	#region Constants
	public int PUCK_SPEED_MIN;
	public int PUCK_SPEED_MAX;
	[HideInInspector]
	public float Radius;
	#endregion
	
	#region Players
	public SinglePlayerEntity PlayerOne;
	public SingleComputerEntity PlayerTwo;
	[HideInInspector]
	public tk2dTextMesh PlayerOneScore;
	public tk2dTextMesh PlayerOneName;
	[HideInInspector]
	public tk2dTextMesh PlayerTwoScore;
	public tk2dTextMesh PlayerTwoName;
    #endregion
	
	#region Vectors
	public int lineLeft;
	public int lineRight;
	public Vector3 PuckCenter;
	#endregion
	
	#region ComputerAI
	public float SHOT_DELAY;
	
	public Transform goalTarget, blockTarget;
	public Transform leftCornerTop, leftCornerBottom;
	public Transform goalReflectionTop, goalReflectionBottom;
	public Transform reflectionLineTop, reflectionLineBottom;
	
	private Difficulty _currentDifficulty;
	
	public Difficulty currentDifficulty{
		get {return _currentDifficulty;}
		set {_currentDifficulty = value;}
	}
	
	public int very = 4;
	public int easy = 3;
	public int medium = 2;
	public int hard = 1;
	#endregion
	
	void Awake(){
		Radius = Puck.GetComponent<SphereCollider>().radius;
	}
	void Start(){
		PlayerOneScore = GameObject.Find("PlayerOneScore").GetComponent<tk2dTextMesh>();
		PlayerTwoScore = GameObject.Find("PlayerTwoScore").GetComponent<tk2dTextMesh>();
		//set reflection points 
		Vector3 _temp;
		_temp = goalTarget.position;
		_temp.y = reflectionLineBottom.position.y +
			(reflectionLineBottom.position.y - goalTarget.position.y);
		goalReflectionBottom.position = _temp;
		_temp = goalTarget.position;
		_temp.y = reflectionLineTop.position.y +
			(reflectionLineTop.position.y - goalTarget.position.y);
		reflectionLineTop.position = _temp;
		
	}
}
