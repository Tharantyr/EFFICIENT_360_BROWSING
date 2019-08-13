using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class TimelineGaze : MonoBehaviour, IGvrPointerHoverHandler
{
    GvrReticlePointer reticlePointer;
    GameObject timeline, preview, zoom;
    Timeline timelineScript;
    Color zoomColor;

    // Use this for initialization
    void Start()
    {
        reticlePointer = GameObject.Find("GvrReticlePointer").GetComponent<GvrReticlePointer>();
        timeline = GameObject.Find("timeline");
        preview = GameObject.Find("preview");
        timelineScript = timeline.GetComponent<Timeline>();
        zoomColor = GameObject.Find("zoom").GetComponent<MeshRenderer>().material.color;
        Material[] mats = GetComponent<Renderer>().materials;
        mats[1].color = new Color(1, 1, 1, 0);
        GetComponent<Renderer>().materials = mats;
    }

    public void OnGvrPointerHover(PointerEventData data)
    {
        timelineScript = timeline.GetComponent<Timeline>();

        if (timelineScript.currentSolution == Timeline.Solution.Exploration)
        {
            if (!timelineScript.fading)
            {
                if (this.name == "zoom")
                {
                    Material[] mats = GetComponent<Renderer>().materials;
                    if (mats[1].color.a == 1)
                    {
                        timeline.GetComponent<Timeline>().fading = true;
                        timeline.GetComponent<Timeline>().segmentRestarted = true;
                        timelineScript.hlt = 0;
                        timelineScript.t = 0;
                    }
                    else
                    {
                        timelineScript.hlt += Time.deltaTime;
                        mats[0].color = new Color(1, 1, 1, 1 - Mathf.Clamp01(timelineScript.hlt));
                        mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(timelineScript.hlt));
                        GetComponent<Renderer>().materials = mats;
                    }
                }

                else if (this.name == "next")
                {
                    Material[] mats = GetComponent<Renderer>().materials;
                    if (mats[1].color.a == 1)
                    {
                        timelineScript.hlt = 0;

                        if (timelineScript.previewState < 4)
                        {
                            preview.transform.position = preview.transform.position + preview.transform.right * 0.625f;
                            timelineScript.previewState++;
                        }
                        else
                        {
                            preview.transform.position = preview.transform.position - preview.transform.right * 2.5f;
                            timelineScript.previewState = 0;
                        }
                    }
                    else
                    {
                        timelineScript.hlt += Time.deltaTime;
                        mats[0].color = new Color(1, 1, 1, 1 - Mathf.Clamp01(timelineScript.hlt));
                        mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(timelineScript.hlt));
                        GetComponent<Renderer>().materials = mats;
                    }
                }

                else if (this.name == "previous")
                {
                    Material[] mats = GetComponent<Renderer>().materials;
                    if (mats[1].color.a == 1)
                    {
                        timelineScript.hlt = 0;

                        if (timelineScript.previewState > 0)
                        {
                            preview.transform.position = preview.transform.position - preview.transform.right * 0.625f;
                            timelineScript.previewState--;
                        }
                        else
                        {
                            preview.transform.position = preview.transform.position + preview.transform.right * 2.5f;
                            timelineScript.previewState = 4;
                        }
                    }
                    else
                    {
                        timelineScript.hlt += Time.deltaTime;
                        mats[0].color = new Color(1, 1, 1, 1 - Mathf.Clamp01(timelineScript.hlt));
                        mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(timelineScript.hlt));
                        GetComponent<Renderer>().materials = mats;
                    }
                }

                else
                {
                    timelineScript.hlt = 0;
                    Material[] mats = GameObject.Find("zoom").GetComponent<Renderer>().materials;
                    mats[0].color = new Color(1, 1, 1, 1);
                    mats[1].color = new Color(1, 1, 1, 0);
                    GameObject.Find("zoom").GetComponent<Renderer>().materials = mats;

                    mats = GameObject.Find("previous").GetComponent<Renderer>().materials;
                    mats[0].color = new Color(1, 1, 1, 1);
                    mats[1].color = new Color(1, 1, 1, 0);
                    GameObject.Find("previous").GetComponent<Renderer>().materials = mats;

                    mats = GameObject.Find("next").GetComponent<Renderer>().materials;
                    mats[0].color = new Color(1, 1, 1, 1);
                    mats[1].color = new Color(1, 1, 1, 0);
                    GameObject.Find("next").GetComponent<Renderer>().materials = mats;
                }
            }
            else
            {
                if (this.name == "back")
                {
                    Material[] mats = GetComponent<Renderer>().materials;
                    if (mats[1].color.a == 1)
                    {
                        timelineScript.hlt = 0;
                        timelineScript.t = 0;
                        timelineScript.fading = false;
                        timelineScript.goBack = true;
                    }
                    else
                    {
                        timelineScript.hlt += Time.deltaTime;
                        mats[0].color = new Color(1, 1, 1, 1 - Mathf.Clamp01(timelineScript.hlt));
                        mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(timelineScript.hlt));
                        GetComponent<Renderer>().materials = mats;
                    }
                }
                else
                {
                    Material[] mats = GameObject.Find("back").GetComponent<Renderer>().materials;
                    mats = GameObject.Find("back").GetComponent<Renderer>().materials;
                    mats[0].color = new Color(1, 1, 1, 1 - timelineScript.GetComponent<Timeline>().faded);
                    mats[1].color = new Color(1, 1, 1, 0);
                    GameObject.Find("back").GetComponent<Renderer>().materials = mats;
                }
            }
        }
    }

    void OnGvrPointerExit(GameObject previousObject)
    {
        if (timelineScript.currentSolution == Timeline.Solution.Exploration)
        {
            timelineScript.hlt = 0;
            Material[] mats = GameObject.Find("zoom").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, 1);
            mats[1].color = new Color(1, 1, 1, 0);
            GameObject.Find("zoom").GetComponent<Renderer>().materials = mats;

            mats = GameObject.Find("previous").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, 1);
            mats[1].color = new Color(1, 1, 1, 0);
            GameObject.Find("previous").GetComponent<Renderer>().materials = mats;

            mats = GameObject.Find("next").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, 1);
            mats[1].color = new Color(1, 1, 1, 0);
            GameObject.Find("next").GetComponent<Renderer>().materials = mats;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timelineScript.currentSolution == Timeline.Solution.Exploration)
        {
            float reticleDistance = 0;

            if (timelineScript.previewState == 0)
            {
                reticleDistance = 1 - (Vector3.Distance(GameObject.Find("zoom").transform.position, reticlePointer.PointerTransform.forward * reticlePointer.maxReticleDistance) - 1.5f);
                reticleDistance = Mathf.Clamp(reticleDistance, 0, 0.25f) * 4;
            }
            else if (timelineScript.previewState == 1)
            {
                reticleDistance = 1 - (Vector3.Distance(GameObject.Find("zoom").transform.position, reticlePointer.PointerTransform.forward * reticlePointer.maxReticleDistance) - 1.9f);
                reticleDistance = Mathf.Clamp(reticleDistance, 0, 0.25f) * 4;
            }
            else if (timelineScript.previewState == 2)
            {
                reticleDistance = 1 - (Vector3.Distance(GameObject.Find("zoom").transform.position, reticlePointer.PointerTransform.forward * reticlePointer.maxReticleDistance) - 2.1f);
                reticleDistance = Mathf.Clamp(reticleDistance, 0, 0.25f) * 4;
            }
            else if (timelineScript.previewState == 3)
            {
                reticleDistance = 1 - (Vector3.Distance(GameObject.Find("zoom").transform.position, reticlePointer.PointerTransform.forward * reticlePointer.maxReticleDistance) - 2.1f);
                reticleDistance = Mathf.Clamp(reticleDistance, 0, 0.25f) * 4;
            }
            else if (timelineScript.previewState == 4)
            {
                reticleDistance = 1 - (Vector3.Distance(GameObject.Find("zoom").transform.position, reticlePointer.PointerTransform.forward * reticlePointer.maxReticleDistance) - 1.9f);
                reticleDistance = Mathf.Clamp(reticleDistance, 0, 0.25f) * 4;
            }

            GameObject.Find("zoom").GetComponent<MeshRenderer>().material.color = new Color(GameObject.Find("zoom").GetComponent<MeshRenderer>().material.color.r, GameObject.Find("zoom").GetComponent<MeshRenderer>().material.color.g, GameObject.Find("zoom").GetComponent<MeshRenderer>().material.color.b, timelineScript.faded * reticleDistance);
        }
    }
}