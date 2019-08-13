using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SteerAttentionAttractor : AttentionAttractor {
	public float attractionScaler = 10f;
	public float graceZone = 60.0f;
	public GameObject sphere = null;

	private void LateUpdate () {
		var curPoint = GetCurrentPoint();
		if (curPoint == null) return;
		Vector3 forwardHor = transform.forward;
		forwardHor.y = 0;
		forwardHor.Normalize();
		Vector3 goalHor = curPoint.transform.position - transform.position;
		goalHor.y = 0;
		goalHor.Normalize();
		float angle = Vector3.SignedAngle(forwardHor, goalHor, Vector3.up);
		if (Mathf.Abs(angle) < graceZone) return;
        float speed = Mathf.Abs(angle) * attractionScaler;  
        sphere.transform.Rotate(new Vector3(0, -speed * Time.deltaTime * Mathf.Sign(angle), 0));
	}
    
}
