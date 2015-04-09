using UnityEngine;
using System.Collections;

public static class AI {


	public static Vector3 CheckStraightShot(Vector3 puckPos, Vector3 targetPos, float radius){

		Vector3 rayTargetPos = targetPos;
		if(puckPos.y >= 1024/2)
			rayTargetPos.y -= radius;
		else
			rayTargetPos.y += radius;

		RaycastHit _hit;
		Vector3 _puckPos = puckPos;
		_puckPos.y -= radius;
		if(Physics.Raycast( _puckPos, (rayTargetPos - _puckPos), out _hit)){
			if(_hit.collider.gameObject.name == "AIGoalTarget"){
				_puckPos = puckPos;
				if(puckPos.y >= 1024/2)
					rayTargetPos.y += radius;
				else
					rayTargetPos.y -= radius;
				_puckPos.y += radius;
				if(Physics.Raycast( _puckPos, (rayTargetPos - _puckPos), out _hit)){
					if(_hit.collider.gameObject.name == "AIGoalTarget"){
						Vector3 _dir = (targetPos - puckPos).normalized;
						float _dist = Vector3.Distance(puckPos, targetPos) * 2.0f;
						Vector3 _vel = _dir * _dist;
						return _vel;
					}
					return Vector3.zero;
				}
				return Vector3.zero;
			}
			return Vector3.zero;
		}
		return Vector3.zero;

	}


	public static Vector3 CheckBlockShot(Vector3 puckPos, Vector3 targetPos){

		if(puckPos.x <= 140.0f){
			Vector3 _dir = (targetPos - puckPos).normalized;
			float _dist = Vector3.Distance(puckPos, targetPos) * 3.0f;
			Vector3 _vel = _dir * _dist;
			return _vel;
		}

		return Vector3.zero;
	}


	public static Vector3 CheckCornerShot(Vector3 puckPos, Vector3 goalPos, Vector3 cornerTopPos, Vector3 cornerBottomPos){
		if(puckPos.y > 668){
			Vector3 _dir = (cornerTopPos - puckPos).normalized;
			float _dist = ( Vector3.Distance(puckPos, cornerTopPos) +  Vector3.Distance(cornerTopPos, goalPos )) * 2.5f;
			Vector3 _vel = _dir * _dist;
			return _vel;
		}
		else if (puckPos.y < 100){
			Vector3 _dir = (cornerBottomPos - puckPos).normalized;
			float _dist = ( Vector3.Distance(puckPos, cornerBottomPos) +  Vector3.Distance(cornerBottomPos, goalPos )) * 2.5f;
			Vector3 _vel = _dir * _dist;
			return _vel;
		}

		return Vector3.zero;
	}


	public static Vector3 CheckTopWallShot(Vector3 puckPos, Vector3 goalPos, Vector3 targetPos, float radius){
		RaycastHit _hit;
		if(Physics.Raycast( puckPos, ( targetPos - puckPos ), out _hit)){
			if(_hit.collider.name == "ColliderTop"){
				Vector3 _targetPos = _hit.point;
				if(puckPos.x >= 512)
					_targetPos.y -= radius * 2;
				else
					_targetPos.y -= radius * 1;
				Vector3 _dir = (_targetPos - puckPos).normalized;
				float _dist = (Vector3.Distance(puckPos, _targetPos) + Vector3.Distance(_targetPos, goalPos)) * 2.0f;
				Vector3 _vel = _dir * _dist;
				return _vel;
			}
			return Vector3.zero;
		}
		return Vector3.zero;
	}


	public static Vector3 CheckBottomWallShot(Vector3 puckPos, Vector3 goalPos, Vector3 targetPos, float radius){
		RaycastHit _hit;
		if(Physics.Raycast( puckPos, ( targetPos - puckPos ), out _hit)){
			if(_hit.collider.name == "ColliderBottom"){
				Vector3 _targetPos = _hit.point;
				if(puckPos.x >= 512)
					_targetPos.y += radius * 2;
				else
					_targetPos.y += radius * 1;
				Vector3 _dir = (_targetPos - puckPos).normalized;
				float _dist = (Vector3.Distance(puckPos, _targetPos) + Vector3.Distance(_targetPos, goalPos)) * 2.0f;
				Vector3 _vel = _dir * _dist;
				return _vel;
			}
			return Vector3.zero;
		}
		return Vector3.zero;
	}

}