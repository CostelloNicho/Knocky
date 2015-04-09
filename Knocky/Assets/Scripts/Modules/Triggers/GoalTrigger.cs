using UnityEngine;
using System.Collections;

/// <summary>
/// Goal trigger is fired when there is a goal.
/// </summary>
public class GoalTrigger : MonoBehaviour {
	
	void OnTriggerEnter( Collider trigger ){
		
		//broadcast the goal
		if(trigger.gameObject.name == "goal1" || trigger.gameObject.name == "goal2")
			Messenger.Broadcast< GameObject >( "Goal", trigger.gameObject );
		
	}
	
}
