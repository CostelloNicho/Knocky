using UnityEngine;
using System.Collections;

/// <summary>
/// Puck algorithms.
/// </summary>
public static class PuckAlgorithms {
	
	//********Member Variables********
	public const int PUCK_VEL_MULTIPIER = 10;
	public const int PUCK_DIST_MAX = 3200;
	public const int PUCK_DIST_MIN = 20;
	

	/// <summary>
	/// converts the release position of the mouse (touch) to a vector.
	/// in 3D space at the same z (layer) as the puck.
	/// </summary>
	/// <returns>
	/// The position.
	/// </returns>
	/// <param name='currentPos'>
	/// Current position.
	/// </param>
	public static Vector3 ConvertPosition( Vector3 currentPos ){
		Vector3 _temp = Input.mousePosition;
		_temp = Camera.main.ScreenToWorldPoint(_temp);
		_temp.z = currentPos.z;
		return _temp;
	}
	
	/// <summary>
	/// Calculates the vector to shoot in
	/// </summary>
	/// <returns>
	/// The shot vector.
	/// </returns>
	/// <param name='puckTouchVector'>
	/// Puck touch vector.
	/// </param>
	/// <param name='puckReleaseVector'>
	/// Puck release vector.
	/// </param>
	public static Vector3 CalculateShotVector( Vector3 puckTouchVector, Vector3 puckReleaseVector ){
		
		float _dist = Vector3.Distance(puckTouchVector, puckReleaseVector) * PUCK_VEL_MULTIPIER;
		_dist = Mathf.Clamp(_dist, PUCK_DIST_MIN, PUCK_DIST_MAX);
		Vector3 _dir = (puckTouchVector - puckReleaseVector).normalized;
		Vector3 _vel = _dir * _dist;
		return _vel;
				
	}
	
	/// <summary>
	/// Calculates the line vector.
	/// </summary>
	/// <returns>
	/// The line vector.
	/// </returns>
	public static Vector3 CalculateLineVector(Vector3 puckPos){
		Vector3 _dir = (puckPos - ConvertPosition(puckPos)).normalized;
		float _dist = Vector3.Distance(puckPos, ConvertPosition(puckPos) );
		return ( puckPos + (_dir * _dist ) );
	}
	
	/// <summary>
	/// Calculates the input axis y.
	/// </summary>
	/// <returns>
	/// The input axis y.
	/// </returns>
	public static float CalculateInputAxisY(){
		Vector3 _temp = Input.mousePosition;
		_temp = Camera.main.ScreenToWorldPoint(_temp);
		return _temp.y;
	}
	
	/// <summary>
	/// Checks the puck position.
	/// </summary>
	/// <returns>
	/// The puck position.
	/// </returns>
	public static bool CheckPuckPosition(Vector3 puckPos, int line1, int line2, float radius){
		if( Vector3.Distance(puckPos,new Vector3(line1, puckPos.y, puckPos.z)) <= radius)
			return true;
		if( Vector3.Distance(puckPos,new Vector3(line2, puckPos.y, puckPos.z)) <= radius)
			return true;
		return false;
	}
}