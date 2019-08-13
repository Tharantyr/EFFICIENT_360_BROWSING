using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionAttractor : MonoBehaviour {
	public List<InterestPoint> interestPoints;
	private int interestPointIdx = 0;
	private float sumLookawayAngle; 

	protected float time;
	private float attractionTime; 
    
	void Start () {
		interestPoints.Sort((a, b) => a.startTime.CompareTo(b.startTime));
        time = 0;
		attractionTime = 0;
		sumLookawayAngle = 0;
	}

	private void Update()
	{
		time += Time.deltaTime;
		var curPoint = GetCurrentPoint();
        if (curPoint == null)
        {
            return;
        }
		attractionTime += Time.deltaTime;
		Vector3 forwardHor = transform.forward;
        forwardHor.y = 0;
        forwardHor.Normalize();
        Vector3 goalHor = curPoint.transform.position - transform.position;
        goalHor.y = 0;
        goalHor.Normalize();
		sumLookawayAngle += Vector3.Angle(forwardHor, goalHor) * Time.deltaTime;
        if ((int)(time * 5) != (int)((time - Time.deltaTime) * 5))
        {
            //DebugFile.Log("Time: " + time.ToString());
            //DebugFile.Log("Target Angle: " + Vector3.Angle(Vector3.forward, goalHor).ToString());
            //DebugFile.Log("Gaze Angle: " + Vector3.Angle(Vector3.forward, forwardHor).ToString());
            //DebugFile.Log("Difference: " + Vector3.Angle(forwardHor, goalHor).ToString());
            //DebugFile.Log("Avg error: " + getAvgLookawayAngle().ToString());
        }
	}

	public float getAvgLookawayAngle() 
	{
		return sumLookawayAngle / attractionTime;	
	}

	protected InterestPoint GetCurrentPoint()
    {
        while (true)
        {
            if (interestPointIdx >= interestPoints.Count) return null;
            var curPoint = interestPoints[interestPointIdx];
            if (time < curPoint.startTime) return null;
            if (time > curPoint.finishTime)
            {
                ++interestPointIdx;
                continue;
            }
            return curPoint;
        }

    }


}
