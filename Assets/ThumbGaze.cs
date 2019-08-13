using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ThumbGaze : MonoBehaviour, IGvrPointerHoverHandler {

    ThumbnailScript thumbnailScript;
    GvrReticlePointer reticlePointer;
    bool gazingAt = false;

    // Use this for initialization
    void Start () {
        reticlePointer = GameObject.Find("GvrReticlePointer").GetComponent<GvrReticlePointer>();
        thumbnailScript = GameObject.Find("BigThumb").GetComponent<ThumbnailScript>();
    }

    public void TutorialText()
    {
        if (thumbnailScript.timeManipulation == ThumbnailScript.TimeManipulation.FastForwardDeviceThumbnail)
            GameObject.Find("TutorialText").GetComponent<TextMeshPro>().text = "FAST-FORWARDING/REWINDING + GAMEPAD TUTORIAL";

        else if (thumbnailScript.timeManipulation == ThumbnailScript.TimeManipulation.TimelineDeviceThumbnail)
            GameObject.Find("TutorialText").GetComponent<TextMeshPro>().text = "TIMELINE + GAMEPAD TUTORIAL";

        else if (thumbnailScript.timeManipulation == ThumbnailScript.TimeManipulation.TimelineDeviceThumbnail)
            GameObject.Find("TutorialText").GetComponent<TextMeshPro>().text = "TIMELINE + GAZE TUTORIAL";

        else
            GameObject.Find("TutorialText").GetComponent<TextMeshPro>().text = "FAST-FORWARDING/REWINDING + GAZE TUTORIAL";
    }

    public void OnGvrPointerHover(PointerEventData data)
    {
        if (thumbnailScript.timeManipulation == ThumbnailScript.TimeManipulation.FastForwardGazeThumbnail)
        {
            if (thumbnailScript.tutorialPhase1 || thumbnailScript.tutorialPhase2)
            {
                if (thumbnailScript.ffgazetut1 && !thumbnailScript.ffgazetut3 && name == "FastForward")
                {
                    GameObject.Find("ffgaze_tut1").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("ffgaze_tut2").GetComponent<MeshRenderer>().enabled = true;
                    thumbnailScript.ffgazetut2 = true;
                }
                else if (thumbnailScript.ffgazetut2 && !thumbnailScript.ffgazetut3 && name == "Rewind")
                {
                    GameObject.Find("ffgaze_tut2").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("ffgaze_tut3").GetComponent<MeshRenderer>().enabled = true;
                    thumbnailScript.ffgazetut3 = true;
                }
                else if (thumbnailScript.ffgazetut3 && name == "Play")
                {
                    if (thumbnailScript.timeManipulation == thumbnailScript.techniqueOrder[0] || thumbnailScript.timeManipulation == thumbnailScript.techniqueOrder[2])
                        GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "First part of the tutorial complete!\n\nGaze at the \"OK\" button above to continue to the second part.";
                    else
                        GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "Second part of the tutorial complete!\n\nGaze at the \"OK\" button above when you are ready to proceed with the experiment.";

                    GameObject.Find("OK").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("ffgaze_tut3").GetComponent<MeshRenderer>().enabled = false;
                }
                else if (!thumbnailScript.ffgazetut1 && !thumbnailScript.ffgazetut2 && !thumbnailScript.ffgazetut3)
                {
                    thumbnailScript.forwardThumb.SeekTo(thumbnailScript.forwardThumb.GetDuration() / 2);
                    GameObject.Find("ffgaze_tut1").GetComponent<MeshRenderer>().enabled = true;
                    thumbnailScript.ffgazetut1 = true;
                }
            }

            if (name == "Play")
            {
                Material[] mats = GameObject.Find("Play").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 0);
                mats[1].color = new Color(1, 1, 1, 1);
                GameObject.Find("Play").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("FastForward").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 1);
                mats[1].color = new Color(1, 1, 1, 0);
                GameObject.Find("FastForward").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("Rewind").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 1);
                mats[1].color = new Color(1, 1, 1, 0);
                GameObject.Find("Rewind").GetComponent<Renderer>().materials = mats;

                thumbnailScript.normalSpeed = true;
                thumbnailScript.rewind = false;
                thumbnailScript.fastForward = false;
            }

            else if (name == "FastForward")
            {
                Material[] mats = GameObject.Find("Play").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 1);
                mats[1].color = new Color(1, 1, 1, 0);
                GameObject.Find("Play").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("FastForward").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 0);
                mats[1].color = new Color(1, 1, 1, 1);
                GameObject.Find("FastForward").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("Rewind").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 1);
                mats[1].color = new Color(1, 1, 1, 0);
                GameObject.Find("Rewind").GetComponent<Renderer>().materials = mats;

                thumbnailScript.normalSpeed = false;
                thumbnailScript.rewind = false;
                thumbnailScript.fastForward = true;
            }

            else if (name == "Rewind")
            {
                Material[] mats = GameObject.Find("Play").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 1);
                mats[1].color = new Color(1, 1, 1, 0);
                GameObject.Find("Play").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("FastForward").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 1);
                mats[1].color = new Color(1, 1, 1, 0);
                GameObject.Find("FastForward").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("Rewind").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 0);
                mats[1].color = new Color(1, 1, 1, 1);
                GameObject.Find("Rewind").GetComponent<Renderer>().materials = mats;

                thumbnailScript.normalSpeed = false;
                thumbnailScript.rewind = true;
                thumbnailScript.fastForward = false;
            }
        }

        if (name == "OK")
        {
            thumbnailScript.ffTimer += Time.deltaTime;

            if (thumbnailScript.ffTimer < 0.5f)
            {
                Material[] mats = GameObject.Find("OK").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, 1 - Mathf.Clamp01(thumbnailScript.ffTimer * 2));
                mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(thumbnailScript.ffTimer * 2));
                GameObject.Find("OK").GetComponent<Renderer>().materials = mats;
            }

            else
            {
                if (thumbnailScript.transition)
                {
                    if (!gazingAt)
                    {
                        if (!thumbnailScript.tutorialPhase1 && !thumbnailScript.tutorialPhase2)
                        {
                            if (thumbnailScript.videoCounter < thumbnailScript.transitionCounter)
                                thumbnailScript.timeManipulation = thumbnailScript.techniqueOrder[0];
                            else
                                thumbnailScript.timeManipulation = thumbnailScript.techniqueOrder[2];

                            GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "";
                            TutorialText();

                            thumbnailScript.tutorialPhase1 = true;
                            GetComponent<MeshRenderer>().enabled = false;
                        }

                        else if (thumbnailScript.tutorialPhase1 && (thumbnailScript.ffgazetut3 || thumbnailScript.tlgazetutdone || thumbnailScript.ffpadtut4 || thumbnailScript.tlpadtut4))
                        {
                            thumbnailScript.ffgazetut3 = false;
                            thumbnailScript.tlgazetutdone = false;
                            thumbnailScript.ffpadtut4 = false;
                            thumbnailScript.tlpadtut4 = false;

                            if (thumbnailScript.videoCounter < thumbnailScript.transitionCounter)
                                thumbnailScript.timeManipulation = thumbnailScript.techniqueOrder[1];
                            else
                                thumbnailScript.timeManipulation = thumbnailScript.techniqueOrder[3];

                            GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "";
                            TutorialText();

                            thumbnailScript.tutorialPhase1 = false;
                            thumbnailScript.tutorialPhase2 = true;
                            GetComponent<MeshRenderer>().enabled = false;
                        }

                        else if (thumbnailScript.tutorialPhase2 && (thumbnailScript.ffgazetut3 || thumbnailScript.tlgazetutdone || thumbnailScript.ffpadtut4 || thumbnailScript.tlpadtut4))
                        {
                            thumbnailScript.ffgazetut3 = false;
                            thumbnailScript.tlgazetutdone = false;
                            thumbnailScript.ffpadtut4 = false;
                            thumbnailScript.tlpadtut4 = false;

                            thumbnailScript.timeManipulation = thumbnailScript.techniqueOrder[thumbnailScript.videoCounter];

                            thumbnailScript.transition = false;
                            thumbnailScript.tutorialPhase2 = false;
                            thumbnailScript.overviewTask = true;
                            thumbnailScript.taskTimer = thumbnailScript.taskTime;
                            GameObject.Find("TutorialText").GetComponent<TextMeshPro>().text = "";
                            GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().text = "";
                            GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "OVERVIEW TASK " + (thumbnailScript.videoCounter + 1) + "\n\nWe will start with the next task. If the observer has given you permission, please gaze at the \"OK\" button above to begin.\n\nYou have 1 minute to complete the task.";
                            thumbnailScript.video = thumbnailScript.videoOrder[thumbnailScript.videoCounter];
                            thumbnailScript.loadVideo(thumbnailScript.video);
                        }

                        gazingAt = true;
                    }
                }

                else if (!thumbnailScript.overviewTask && !thumbnailScript.contentTask)
                {
                    if (!gazingAt)
                    {
                        thumbnailScript.overviewTask = true;
                        thumbnailScript.video = thumbnailScript.videoOrder[thumbnailScript.videoCounter];
                        thumbnailScript.loadVideo(thumbnailScript.video);
                    }
                    gazingAt = true;
                }

                else if (thumbnailScript.overviewTask)
                {
                    if (!gazingAt)
                    {
                        if (!thumbnailScript.overviewTaskStarted)
                        {
                            thumbnailScript.overviewTaskStarted = true;
                            GameObject.Find("OK").GetComponent<MeshRenderer>().enabled = false;
                            GameObject.Find("TaskText").GetComponent<TextMeshPro>().alpha = 0;
                        }
                        else
                        {
                            if (thumbnailScript.overviewCompleted)
                            {
                                thumbnailScript.overviewTask = false;
                                thumbnailScript.overviewTaskStarted = false;
                                thumbnailScript.overviewCompleted = false;
                                thumbnailScript.contentTask = true;
                                thumbnailScript.taskTimer = thumbnailScript.taskTime;
                                GameObject.Find("TaskText").GetComponent<TextMeshPro>().alpha = 1;
                                GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().alpha = 0;
                            }
                        }
                    }
                    gazingAt = true;
                }

                else if (thumbnailScript.contentTask)
                {
                    if (!thumbnailScript.contentTaskStarted)
                    {
                        if (!gazingAt)
                        {
                            GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().alpha = 1;
                            GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "";
                            thumbnailScript.contentTaskStarted = true;

                            thumbnailScript.forwardThumb.SeekTo(0);
                            thumbnailScript.forwardScene.SeekTo(0);
                            thumbnailScript.backThumb.SeekTo(thumbnailScript.backThumb.GetDuration());
                            thumbnailScript.backScene.SeekTo(thumbnailScript.backScene.GetDuration());
                        }
                        gazingAt = true;
                    }
                    else
                    {
                        if (!gazingAt)
                        {
                            if (!thumbnailScript.checkContent)
                            {
                                Debug.Log(thumbnailScript.forwardThumb.GetSeekPosition());
                                thumbnailScript.checkContent = true;
                            }

                            if (thumbnailScript.contentDone && !GameObject.Find("AllDone").GetComponent<MeshRenderer>().enabled)
                            {
                                //GameObject.Find("AllDone").GetComponent<MeshRenderer>().enabled = true;

                                thumbnailScript.contentTask = false;
                                thumbnailScript.contentTaskStarted = false;
                                thumbnailScript.checkContent = false;
                                thumbnailScript.contentDone = false;

                                thumbnailScript.videoCounter++;

                                if (thumbnailScript.videoCounter < 4)
                                {
                                    if (!thumbnailScript.overrideExperiment)
                                        thumbnailScript.timeManipulation = thumbnailScript.techniqueOrder[thumbnailScript.videoCounter];

                                    if (thumbnailScript.videoCounter == thumbnailScript.transitionCounter)
                                    {
                                        Material[] mats = GameObject.Find("Black").GetComponent<Renderer>().materials;
                                        mats[0].color = new Color(0, 0, 0, 0);
                                        GameObject.Find("Black").GetComponent<Renderer>().materials = mats;

                                        thumbnailScript.paused = false;

                                        thumbnailScript.transition = true;
                                        GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().text = "";
                                        GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "TUTORIAL\n\nThe next experiment will begin soon.\n\nPlease obtain instructions from the observer.";
                                        thumbnailScript.loadVideo(ThumbnailScript.Video.Tutorial);
                                    }
                                    else
                                    {
                                        thumbnailScript.taskTimer = thumbnailScript.taskTime;
                                        GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().text = "";
                                        GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "OVERVIEW TASK " + (thumbnailScript.videoCounter + 1) + "\n\nWe will start with the next task. If the observer has given you permission, please gaze at the \"OK\" button above to begin.\n\nYou have 1 minute to complete the task.";
                                        thumbnailScript.video = thumbnailScript.videoOrder[thumbnailScript.videoCounter];
                                        thumbnailScript.loadVideo(thumbnailScript.video);
                                        thumbnailScript.overviewTask = true;
                                    }
                                }
                                else
                                {
                                    GameObject.Find("AllDone").GetComponent<MeshRenderer>().enabled = true;
                                    GameObject.Find("OK").GetComponent<MeshRenderer>().enabled = false;
                                    GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "";
                                }
                            }

                            gazingAt = true;
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (reticlePointer.CurrentRaycastResult.gameObject != null)
        {
            if (reticlePointer.CurrentRaycastResult.gameObject.name != "OK")
            {
                gazingAt = false;

                if (thumbnailScript.ffTimer < 0.5f)
                {
                    thumbnailScript.ffTimer = 0;
                    Material[] mats = GameObject.Find("OK").GetComponent<Renderer>().materials;
                    mats[0].color = new Color(1, 1, 1, 1);
                    mats[1].color = new Color(1, 1, 1, 0);
                    GameObject.Find("OK").GetComponent<Renderer>().materials = mats;
                }

                if (thumbnailScript.ffTimer >= 0.5f)
                {
                    thumbnailScript.ffTimer = 0;
                    Material[] mats = GameObject.Find("OK").GetComponent<Renderer>().materials;
                    mats[0].color = new Color(1, 1, 1, 1);
                    mats[1].color = new Color(1, 1, 1, 0);
                    GameObject.Find("OK").GetComponent<Renderer>().materials = mats;
                }
            }
        }
	}
}
