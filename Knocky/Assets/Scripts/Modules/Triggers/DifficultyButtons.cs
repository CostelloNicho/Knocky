using UnityEngine;
using System.Collections;

public class DifficultyButtons : MonoBehaviour {

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable(){
		EasyTouch.On_TouchStart += OnButtonTouch;
	}
	
	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable(){
		EasyTouch.On_TouchStart -= OnButtonTouch;
	}
	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy(){
		EasyTouch.On_TouchStart -= OnButtonTouch;
	}
	
	/// <summary>
	/// Raises the button touch event.
	/// </summary>
	/// <param name='gesture'>
	/// Gesture.
	/// </param>
	public void OnButtonTouch(Gesture gesture){
		//Send message that item has been pressed
		if(gesture.pickObject == gameObject){
			Messenger.Broadcast< string >( "DifficultyChosen", this.gameObject.tag);
			iTween.ShakeScale(gameObject, iTween.Hash("time", 0.5f, "amount", new Vector3(1.2f,1.2f,0.0f)));
		}
	}	
}
