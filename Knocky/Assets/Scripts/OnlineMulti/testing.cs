using UnityEngine;
using System.Collections;

public class testing : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EasyTouch.On_Drag += (gesture) => {
			Debug.Log(gesture.pickObject.name);
		};
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
