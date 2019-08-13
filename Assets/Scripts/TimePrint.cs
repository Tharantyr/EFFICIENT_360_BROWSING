using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePrint : MonoBehaviour {
	private float time = 0;
	// Use this for initialization
   
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (Mathf.Floor(time) != Mathf.Floor(time - Time.deltaTime)) {
			print(Mathf.Floor(time));
		}
	}
}
