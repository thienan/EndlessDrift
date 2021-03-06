﻿using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour {
	public static event Action<int> OnAddCoinsVisual;

	[HideInInspector] public GameObject ParentObj;
    private Vector3 _targetPos;
    private float _velocity;
    private float _moveAngle;
    private bool _isAnglePlus;
    private float _addAngleCoeff;
    private bool _isShowAnimation;
    private bool _isHideAnimation;
    private bool _isMoveToTarget = false;
    private const float VelocityMax = 0.45f;

	public void Show() {
		_isShowAnimation = true;
	}

	public void Hide(bool isAnimation) {
		_isHideAnimation = isAnimation;

		if (!_isHideAnimation) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!_isMoveToTarget) {
			if (ParentObj) transform.position = ParentObj.transform.position;
		}

		if (_isShowAnimation)
		{
			transform.localScale = new Vector3 (transform.localScale.x - 0.2f, transform.localScale.y - 0.2f, 1f);
			if (transform.localScale.x <= 1f) {
				_isShowAnimation = false;
				transform.localScale = new Vector3 (1f, 1f, 1f);
			}
		} else
			if (_isHideAnimation)
			{
				transform.localScale = new Vector3 (transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, 1f);
				if (transform.localScale.x <= 0f) {
					//GameEvents.Send(OnAddCoinsVisual, 1);
					Destroy (gameObject);
				}
			}else
				if (_isMoveToTarget) {
					if (transform.localScale.x >= 1) transform.localScale = new Vector3(transform.localScale.x - 0.1f,
						transform.localScale.y - 0.1f, 1f);

					Vector3 newPos = Camera.main.ScreenToWorldPoint(DefsGame.Coins.img.transform.position);
					_targetPos = new Vector3(newPos.x, newPos.y, transform.position.z);

					float ang = Mathf.Atan2 (_targetPos.y - transform.position.y, _targetPos.x - transform.position.x);

					if (_isAnglePlus) {
						_moveAngle += _addAngleCoeff* Mathf.Deg2Rad;
						if (_moveAngle >= 180f* Mathf.Deg2Rad) _moveAngle -= 360f* Mathf.Deg2Rad;
					} else {
						_moveAngle -= _addAngleCoeff* Mathf.Deg2Rad;
						if (_moveAngle <= -180f* Mathf.Deg2Rad) _moveAngle += 360f* Mathf.Deg2Rad;
					}

					if (_addAngleCoeff < 35f) _addAngleCoeff += 0.5f;

					if (Mathf.Abs(_moveAngle - ang) < _addAngleCoeff*1.5f* Mathf.Deg2Rad) _moveAngle = ang;

					if (_velocity < VelocityMax) _velocity += 0.008f;
					transform.position = new Vector3(transform.position.x + _velocity * Mathf.Cos(_moveAngle),
						transform.position.y + _velocity * Mathf.Sin(_moveAngle), 1f);

					if (Vector2.Distance(transform.position, _targetPos) <= 0.45f) {
						_isMoveToTarget = false;

						GameEvents.Send(OnAddCoinsVisual, 1);
						Destroy(gameObject);
					    //DefsGame.btnBuyCoinsComponent.addOneCoinVisual();
					    //Defs.soundEngine.playSound(sndTouch);
					}
				}
	}

	public void MoveToEnd()
	{
//	    Vector3 newPos = Camera.main.ScreenToWorldPoint(DefsGame.Coins.img.transform.position);
//	    _targetPos = new Vector3(newPos.x, newPos.y, gameObject.transform.position.z);
		_velocity = 0.05f + Random.value*0.05f;
		if (Random.value < 0.5f) _moveAngle = Random.value * 180f* Mathf.Deg2Rad;
		else _moveAngle = -Random.value * 180f* Mathf.Deg2Rad;

		_isAnglePlus = !(Random.value < 0.5f);

		_addAngleCoeff = 5f;

		_isMoveToTarget = true;

		transform.localScale = new Vector3 (2.5f, 2.5f, 1f);
	} 
}
