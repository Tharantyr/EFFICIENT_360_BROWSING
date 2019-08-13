using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAvgLookawayAngle : MonoBehaviour {
	public Text displayText;
	public float timeDelay = 30;

	private float time;

	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > timeDelay) {
			displayText.text = GetComponent<AttentionAttractor>().getAvgLookawayAngle().ToString();
			enabled = false;
		}
	}
}
