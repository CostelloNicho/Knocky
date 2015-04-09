using UnityEngine;
using System.Collections;

public class PuckSFX : MonoBehaviour {
	
	GameObject puckSFX;

	void OnCollisionEnter( Collision collision ){
				
		switch( collision.collider.tag ){
		case "Boundry":
			PuckWallCollisionEffect( collision );
			break;
		case "Corner":
			PuckWallCollisionEffect( collision );
			break;
		case "Block":
			PuckWallCollisionEffect( collision );
			break;
		}
	}
	
	void Start(){
		puckSFX = Resources.Load("PuckHitBoundrySFX")as GameObject;
	}
		
	private void PuckWallCollisionEffect( Collision collision ){
		ContactPoint contact = collision.contacts[0];
		Instantiate(puckSFX , contact.point, Quaternion.LookRotation(-contact.normal));
		
	}
}
