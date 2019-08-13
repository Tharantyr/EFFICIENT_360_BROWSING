using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeGuidance : MonoBehaviour {

    public enum State { On, Off };
    public State gazeGuidance = State.On;
    GameObject GazeGuidanceScreen, OriginalCamera;
    UnityEngine.Video.VideoPlayer scenePlayer;
    public List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>();
    float maxRotation = 50;
    bool atLeastOne = false;

    // Use this for initialization
    void Start() {
        GazeGuidanceScreen = GameObject.Find("GazeGuidanceScreen");
        OriginalCamera = GameObject.Find("Camera");
        scenePlayer = GameObject.Find("Sphere").GetComponent<UnityEngine.Video.VideoPlayer>();

        if (gazeGuidance == State.Off)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (gazeGuidance == State.On)
        {
            foreach (PointOfInterest POI in pointsOfInterest)
            {
                if (scenePlayer.frame >= POI.frameStart && scenePlayer.frame <= POI.frameStart + POI.duration)
                {
                    atLeastOne = true;
                    GazeGuidanceScreen.GetComponent<Renderer>().enabled = true;
                    transform.eulerAngles = new Vector3(0, POI.angle, 0);
                    transform.position = transform.forward * POI.distance;

                    if ((Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)) >= -10) &&
                        (Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)) <= 10))
                    {
                        GazeGuidanceScreen.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Abs((Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)))) * 0.1f); ;
                    }
                    else
                    {
                        Debug.Log("KANKERFLIKKER");
                        GazeGuidanceScreen.GetComponent<Renderer>().material.SetFloat("_Alpha", 1);
                    }

                    Debug.Log(Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)));

                    if (Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)) >= 0)
                    {
                        if (Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)) > maxRotation)
                        {
                            GazeGuidanceScreen.transform.eulerAngles = new Vector3(0, OriginalCamera.transform.eulerAngles.y + maxRotation, 0);
                            GazeGuidanceScreen.transform.localScale = new Vector3(0.18f + (Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)) / 720), GazeGuidanceScreen.transform.localScale.y, GazeGuidanceScreen.transform.localScale.z);
                        }
                        else
                            GazeGuidanceScreen.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }
                    else if (Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)) < 0)
                    {
                        if (Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)) < -maxRotation)
                        {
                            GazeGuidanceScreen.transform.eulerAngles = new Vector3(0, OriginalCamera.transform.eulerAngles.y - maxRotation, 0);
                            GazeGuidanceScreen.transform.localScale = new Vector3(0.18f + (Vector3.SignedAngle(OriginalCamera.transform.forward, transform.forward, new Vector3(0, 1, 0)) / -720), GazeGuidanceScreen.transform.localScale.y, GazeGuidanceScreen.transform.localScale.z);
                        }
                        else
                            GazeGuidanceScreen.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }

                    GazeGuidanceScreen.transform.position = GazeGuidanceScreen.transform.forward * 1.5f;
                }
                else
                {
                    if (!atLeastOne)
                        GazeGuidanceScreen.GetComponent<Renderer>().enabled = false;
                }
            }
            atLeastOne = false;
        }
    }

    [System.Serializable]
    public struct PointOfInterest
    {
        public float angle, distance, frameStart, duration;

        public PointOfInterest(float angle, float distance, float frameStart, float duration)
        {
            this.angle = angle;
            this.distance = distance;
            this.frameStart = frameStart;
            this.duration = duration;
        }
    }
}
