using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehavior : MonoBehaviour {



	// Use this for initialization
	void Start () {
        Material[] mats = GetComponent<Renderer>().materials;
        mats[0].color = new Color(0, 0, 0, 1);
        GetComponent<Renderer>().materials = mats;
    }
	
	// Update is called once per frame
	void LateUpdate () {
    }
}
