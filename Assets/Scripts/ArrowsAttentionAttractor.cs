using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsAttentionAttractor : AttentionAttractor {
	public GameObject arrowLeft;
	public GameObject arrowRight;
	public float graceZone = 60.0f;
	public float blinkTime = 0.7f;
    
	private bool isLeftArrowActive = false; 
	private bool isRightArrowActive = false;

	private void LateUpdate () {
		isLeftArrowActive = false;
        isRightArrowActive = false;
		var curPoint = GetCurrentPoint();
		if (curPoint == null)
		{
			UpdateArrows();
			return;
		}
		Vector3 forwardHor = transform.forward;
		forwardHor.y = 0;
		forwardHor.Normalize();
		Vector3 goalHor = curPoint.transform.position - transform.position;
		goalHor.y = 0;
		goalHor.Normalize();
		float angle = Vector3.SignedAngle(forwardHor, goalHor, Vector3.up);
		if (Mathf.Abs(angle) > graceZone) {
			if (angle > 0) isRightArrowActive = true;
			if (angle < 0) isLeftArrowActive = true;
		}
		UpdateArrows();
	}

	private void UpdateArrows() {
		arrowLeft.SetActive(false);
		arrowRight.SetActive(false);
		if ((int)(time / blinkTime) % 2 == 0) {
			if (isLeftArrowActive) arrowLeft.SetActive(true);
			if (isRightArrowActive) arrowRight.SetActive(true);
		}
	}
}
