using UnityEngine;
using System.Collections;
using System;

public abstract class InputScheme : MonoBehaviour {
	
	protected Action inputComplete;
	protected Transform puck;
	
	public abstract void Enable( Action callback, LineRenderer _puckLine, Transform _puck, Transform _lineIndicator);
}
