using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ThumbTLGaze : MonoBehaviour, IGvrPointerHoverHandler
{
    MediaPlayerCtrl forwardThumb, backThumb, forwardScene, backScene;
    GameObject bigThumb;

    // Use this for initialization
    void Start () {
        bigThumb = GameObject.Find("BigThumb");
        forwardThumb = GameObject.Find("ForwardThumb").GetComponent<MediaPlayerCtrl>();
        forwardScene = GameObject.Find("Sphere").GetComponent<MediaPlayerCtrl>();
        backScene = GameObject.Find("BackSphere").GetComponent<MediaPlayerCtrl>();
        backThumb = GameObject.Find("BackThumb").GetComponent<MediaPlayerCtrl>();
    }

    public void OnGvrPointerHover(PointerEventData data)
    {
        /*if (bigThumb.GetComponent<ThumbnailScript>().timeManipulation == ThumbnailScript.TimeManipulation.TimelineGaze)
        {
            RaycastResult ray = reticlePointer.CurrentRaycastResult;
            float timelineDistance = Vector3.Distance(transform.position, ray.worldPosition);

            //Debug.Log(timelineDistance);
            float playPosition = Vector3.Distance(ray.worldPosition, transform.position - transform.right * 1.5f);

            Vector3 pointOnScreen = GameObject.Find("Camera").transform.forward;
            Vector3 startPosition = transform.position;// - transform.forward * 0.42f + Vector3.up * 0.18f - transform.right * 1.05f;
            startPosition = new Vector3(startPosition.x, 0, startPosition.z);
            playPosition = (-1.0f) * Vector3.SignedAngle(new Vector3(pointOnScreen.x, 0, pointOnScreen.z), startPosition, Vector3.up);
            Debug.Log(playPosition);

            forwardThumb.SeekTo((int)(((playPosition + 45) / 90f) * forwardThumb.GetDuration()));
            backThumb.SeekTo((int)(((playPosition + 45) / 90f) * forwardThumb.GetDuration()));
            forwardScene.SeekTo((int)(((playPosition + 45) / 90f) * forwardThumb.GetDuration()));
            backScene.SeekTo((int)(((playPosition + 45) / 90f) * forwardThumb.GetDuration()));
        }*/
    }

    // Update is called once per frame
    void Update ()
    {
        if (bigThumb.GetComponent<ThumbnailScript>().timeManipulation == ThumbnailScript.TimeManipulation.TimelineGazeThumbnail)
        {
            ThumbnailScript thumbnailScript = GameObject.Find("BigThumb").GetComponent<ThumbnailScript>();

            if (thumbnailScript.tutorialPhase1 || thumbnailScript.tutorialPhase2)
            {
                if (!thumbnailScript.tlgazetut)
                {
                    forwardThumb.SeekTo(0);
                    GameObject.Find("tlgaze_tut").GetComponent<MeshRenderer>().enabled = true;
                    thumbnailScript.tlgazetut = true;
                }
            }

            RaycastHit hit;

            if (Time.frameCount % 3 == 0 && Physics.Raycast(GameObject.Find("Camera").transform.position, GameObject.Find("Camera").transform.position + GameObject.Find("Camera").transform.forward * 0.1f, out hit, Mathf.Infinity, 1 << 9))
            {
                if (hit.collider.gameObject.name == "ThumbTimeline")
                {
                    if ((thumbnailScript.tutorialPhase1 || thumbnailScript.tutorialPhase2) && thumbnailScript.tlgazetut)
                    {
                        thumbnailScript.tlgazetutdone = true;
                        GameObject.Find("OK").GetComponent<MeshRenderer>().enabled = true;
                        GameObject.Find("tlgaze_tut").GetComponent<MeshRenderer>().enabled = false;

                        if (thumbnailScript.timeManipulation == thumbnailScript.techniqueOrder[0] || thumbnailScript.timeManipulation == thumbnailScript.techniqueOrder[2])
                            GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "First part of the tutorial complete!\n\nGaze at the \"OK\" button above to continue to the second part.";
                        else
                            GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "Second part of the tutorial complete!\n\nGaze at the \"OK\" button above when you are ready to proceed with the experiment.";
                    }

                    Vector3 thumbPosition = GameObject.Find("ThumbTimeline").transform.position;
                    float playbackTime = Mathf.Min(2.5f, Vector3.Distance(new Vector3(thumbPosition.x, 0, thumbPosition.z) - GameObject.Find("ThumbTimeline").transform.right * 1.25f, new Vector3(hit.point.x, 0, hit.point.z))) / 2.5f;

                    if (playbackTime < 0.25f && Vector3.Distance(new Vector3(thumbPosition.x, 0, thumbPosition.z), new Vector3(hit.point.x, 0, hit.point.z)) > 1.25f)
                        playbackTime = 0;

                    forwardThumb.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                    backThumb.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                    forwardScene.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                    backScene.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                }
            }
        }
    }
}
