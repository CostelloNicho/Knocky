using UnityEngine;
using System.Collections;
using System;

public class LineDragInput : InputScheme {

	//********Member Variables********
	private Transform indicator;
	
	//************************************CONSTRUCTOR *******************************************
	//Summary: Initializes variables
	//			Sets up listeners
	//*******************************************************************************************
	public override void Enable( Action callback, LineRenderer _puckLine, Transform _puck, Transform _lineIndicator ){
		inputComplete = callback;
		EasyTouch.On_Drag += OnTouchDrag;
		EasyTouch.On_DragEnd += OnDragRelease;
		puck = _puck;
		indicator = _lineIndicator;
		indicator.position = puck.position;
	}
	
	void OnDestroy(){
		EasyTouch.On_Drag -= OnTouchDrag;
		EasyTouch.On_DragEnd -= OnDragRelease;
	}
	
	#region Event Listeners
	/// <summary>
	/// Raises the touch drag event.
	/// </summary>
	/// <param name='gesture'>
	/// Gesture.
	/// </param>
	void OnTouchDrag(Gesture gesture){
		if(gesture.pickObject == puck.gameObject){
			Vector3 _temp = puck.position;
			_temp.y = PuckAlgorithms.CalculateInputAxisY();
			iTween.MoveUpdate(puck.gameObject, 
				iTween.Hash("position", _temp, "time", 0.1f));
			//set Indicator
			indicator.position = puck.position;
		}
	}
	
	/// <summary>
	/// Releases the puck from line drag mode 
	//  Removes listeners
	/// </summary>
	void OnDragRelease(Gesture gesture){
		if(gesture.pickObject == puck.gameObject ){
			EasyTouch.On_Drag -= OnTouchDrag;
			EasyTouch.On_DragEnd -= OnDragRelease;
			indicator.position = new Vector3(0,0,Layers.hiddenLayer);
			inputComplete();
		}
	}
	#endregion
}
