using UnityEngine;
using System.Collections;

public class OnlineAssets : MonoBehaviour {
	
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
	public GameObject PlayerOne;
	public GameObject PlayerTwo;
	public tk2dTextMesh PlayerOneScore;
	public tk2dTextMesh PlayerOneName;
	public tk2dTextMesh PlayerTwoScore;
	public tk2dTextMesh PlayerTwoName;
    #endregion
	
	#region Vectors
	public int lineLeft;
	public int lineRight;
	public Vector3 PuckCenter;
	#endregion
	
	void Awake(){
		Radius = Puck.GetComponent<SphereCollider>().radius;
	}
}
