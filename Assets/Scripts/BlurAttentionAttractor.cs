using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BlurAttentionAttractor : AttentionAttractor {
	public PostprocessBlur blur;
	public float fadeTime = 1.0f;
	public float maxIntensity = 0.05f;

	private void LateUpdate () {
		var curPoint = GetCurrentPoint();
		if (curPoint == null) {
			SetIntensity(0);
            return;
		}
		Vector3 forwardHor = transform.forward;
		forwardHor.y = 0;
		forwardHor.Normalize();
		Vector3 goalHor = curPoint.transform.position - transform.position;
		goalHor.y = 0;
		goalHor.Normalize();
		float intensityScaler = Mathf.Abs(Vector3.Angle(forwardHor, goalHor)) / 180.0f;
		float intensity = maxIntensity;
		if (time < curPoint.startTime + fadeTime) {
			intensity = maxIntensity * (time - curPoint.startTime) / fadeTime;
		} else if (time > curPoint.finishTime - fadeTime) {
			intensity = maxIntensity * (curPoint.finishTime - time) / fadeTime;
		}
		SetIntensity(intensity * intensityScaler);
	}

    private void SetIntensity(float intensity)
	{
		blur.radius = intensity;
	}
}
