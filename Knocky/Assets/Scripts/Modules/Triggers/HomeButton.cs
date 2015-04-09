using UnityEngine;
using System.Collections;

public class HomeButton : MonoBehaviour {

	void Start(){
		EasyTouch.On_TouchUp += HomeButtonPressed;
	}
	
	void HomeButtonPressed(Gesture gesture){
		if(gesture.pickObject == this.gameObject){
			Messenger.Broadcast<bool>("HomeButtonPressed", true);
		}
	}
	
	void OnDestroy(){
		EasyTouch.On_TouchUp -= HomeButtonPressed;
	}
}
