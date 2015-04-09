using UnityEngine;
using System.Collections;
using System;

public class ShotInput : InputScheme {
	
	//********Member Variables********
	private LineRenderer puckLine;
	
	
	//************************************CONSTRUCTOR *******************************************
	//Summary: Initializes variables
	//			Sets up listeners
	//*******************************************************************************************
	
	public override void Enable( Action callback, LineRenderer _puckLine, Transform _puck, Transform _lineIndicator){
		inputComplete = callback;
		EasyTouch.On_Drag += OnTouchDrag;
		EasyTouch.On_DragEnd += OnDragRelease;
		puck = _puck;
		puckLine = _puckLine;
	}
	
	void OnDestroy(){
		EasyTouch.On_Drag -= OnTouchDrag;
		EasyTouch.On_DragEnd -= OnDragRelease;
	}
	
	
	//************************************OnTouchDrag *******************************************
	//Summary: When Puck is being dragged, activate the line renderers. 
	//
	//**********************************************************************************************
	
	public void OnTouchDrag(Gesture gesture){
		//Activate Line 
		if(gesture.pickObject == puck.gameObject){
			puckLine.SetPosition(0, puck.position);
			puckLine.SetPosition(1, PuckAlgorithms.CalculateLineVector(puck.transform.position));
		}
	}
	
	//************************************OnDragRelease *******************************************
	//Summary: When Puck is Released shoot.
	//   	   DeActivate Line renderers
	//**********************************************************************************************
	public void OnDragRelease(Gesture gesture){
		if(gesture.pickObject == puck.gameObject){
			EasyTouch.On_Drag -= OnTouchDrag;
			EasyTouch.On_DragEnd -= OnDragRelease;
			//Deactive Line effects
			puckLine.SetPosition(0, Vector3.zero);
			puckLine.SetPosition(1, Vector3.zero);
			inputComplete();
		}
	}
}
