using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timeline : MonoBehaviour {
    public enum Solution {  None, Exploration, Autopilot };
    public Solution currentSolution = Solution.Exploration;
    int previousState = -1;
    GameObject preview, previewText, fade, ball, back;
    UnityEngine.Video.VideoPlayer previewPlayer, scenePlayer;
    ulong segment;
    float autoCountdown = 6;
    bool intermission1 = false; bool intermission2 = false; bool intermission3 = false; bool intermission4 = false; bool intermission5 = false;
    bool auto1 = false; bool auto2 = false; bool auto3 = false; bool auto4 = false; bool auto5 = false;

    [HideInInspector] public int previewState = 0;
    [HideInInspector] public float t = 0;
    [HideInInspector] public float hlt = 0;
    [HideInInspector] public bool fading;
    [HideInInspector] public bool segmentRestarted = false;
    [HideInInspector] public bool goBack = false;
    [HideInInspector] public float faded = 1;

	// Use this for initialization
	void Start () {
        preview = GameObject.Find("previewScreen");
        fade = GameObject.Find("Fade");
        previewPlayer = preview.GetComponent<UnityEngine.Video.VideoPlayer>();
        scenePlayer = GameObject.Find("Sphere").GetComponent<UnityEngine.Video.VideoPlayer>();
        previewText = GameObject.Find("previewText");
        segment = previewPlayer.frameCount / 5;
        GameObject.Find("end").GetComponent<TextMeshPro>().text = framesToMinutes((int)previewPlayer.frameCount);
        ball = GameObject.Find("ball");

        // Make back button invisible
        back = GameObject.Find("back");
        Material[] mats = back.GetComponent<Renderer>().materials;
        mats[0].color = new Color(1, 1, 1, 0);
        mats[1].color = new Color(1, 1, 1, 0);
        back.GetComponent<Renderer>().materials = mats;

        if (currentSolution == Solution.Autopilot)
        {
            mats = GameObject.Find("previous").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("previous").GetComponent<Renderer>().materials = mats;

            mats = GameObject.Find("next").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("next").GetComponent<Renderer>().materials = mats;

            mats = GameObject.Find("zoom").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("zoom").GetComponent<Renderer>().materials = mats;

            mats = GameObject.Find("frame2").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("frame2").GetComponent<Renderer>().materials = mats;
            mats = GameObject.Find("frame3").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("frame3").GetComponent<Renderer>().materials = mats;
            mats = GameObject.Find("frame4").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("frame4").GetComponent<Renderer>().materials = mats;
            mats = GameObject.Find("frame5").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("frame5").GetComponent<Renderer>().materials = mats;

            mats = GameObject.Find("timeline").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("timeline").GetComponent<Renderer>().materials = mats;

            mats = GameObject.Find("ball").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("ball").GetComponent<Renderer>().materials = mats;

            mats = GameObject.Find("start").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("start").GetComponent<Renderer>().materials = mats;

            mats = GameObject.Find("end").GetComponent<Renderer>().materials;
            mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(0));
            GameObject.Find("end").GetComponent<Renderer>().materials = mats;

            GameObject.Find("start").GetComponent<TextMeshPro>().alpha = 0;
            GameObject.Find("end").GetComponent<TextMeshPro>().alpha = 0;

            GameObject.Find("preview").transform.position = GameObject.Find("preview").transform.position + GameObject.Find("preview").transform.right * 1.5f;

            intermission1 = true;
        }
        else if (currentSolution == Solution.Exploration)
        {
            GameObject.Find("autocountdown").GetComponent<TextMeshPro>().alpha = 0;
            GameObject.Find("autonumbers").GetComponent<TextMeshPro>().alpha = 0;
        }
        else
        {
            mats = fade.GetComponent<Renderer>().materials;
            mats[0].color = new Color(0, 0, 0, 0);
            fade.GetComponent<Renderer>().materials = mats;

            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (currentSolution == Solution.Exploration)
        {
            if (fading)
            {
                t += Time.deltaTime / 2;

                // Fade in scene
                if (segmentRestarted)
                {
                    previewPlayer.frame = (int)segment * previewState;
                    scenePlayer.frame = (int)segment * previewState;
                    segmentRestarted = false;
                }
                Material[] mats = fade.GetComponent<Renderer>().materials;
                mats[0].color = new Color(0, 0, 0, 1 - t);
                fade.GetComponent<Renderer>().materials = mats;

                // Fade out GUI
                preview.GetComponent<Renderer>().material.SetFloat("_Alpha", 1 - t);
                preview.transform.localScale = new Vector3(Mathf.Min(preview.transform.localScale.x * 1 + (t * 0.3f), 15), Mathf.Min(preview.transform.localScale.y * 1 + (t * 0.3f), 15), preview.transform.localScale.z);

                mats = GameObject.Find("previous").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                GameObject.Find("previous").GetComponent<Renderer>().materials = mats;

                mats = GameObject.Find("next").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                GameObject.Find("next").GetComponent<Renderer>().materials = mats;

                mats = GameObject.Find("zoom").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                GameObject.Find("zoom").GetComponent<Renderer>().materials = mats;

                mats = GameObject.Find("frame2").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                GameObject.Find("frame2").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("frame3").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                GameObject.Find("frame3").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("frame4").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                GameObject.Find("frame4").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("frame5").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                GameObject.Find("frame5").GetComponent<Renderer>().materials = mats;

                GameObject.Find("end").GetComponent<TextMeshPro>().text = "0:10";

                previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);

                faded = Mathf.Clamp01(1 - t);

                // Fade in back button
                mats = back.GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(t));
                back.GetComponent<Renderer>().materials = mats;

                // After fading
                if (previewPlayer.frame > 0)
                {
                    float frameCounter = Vector3.Distance(GameObject.Find("start").GetComponent<TextMeshPro>().rectTransform.position, GameObject.Find("end").GetComponent<TextMeshPro>().rectTransform.position);
                    ball.transform.position = ball.transform.position + ball.transform.right * frameCounter * (1f / 825f);

                    if (previewPlayer.frame - (int)segment * previewState == 300)
                        ball.transform.position = GameObject.Find("timeline").transform.position + GameObject.Find("timeline").transform.forward * -0.76f - GameObject.Find("timeline").transform.right * 2.18f + GameObject.Find("timeline").transform.up * 0.38f;
                }
            }

            if (goBack)
            {
                Debug.Log("LOL");
                t += Time.deltaTime / 2;

                Material[] mats = fade.GetComponent<Renderer>().materials;
                mats[0].color = new Color(0, 0, 0, t);
                fade.GetComponent<Renderer>().materials = mats;

                // Fade out GUI
                preview.GetComponent<Renderer>().material.SetFloat("_Alpha", t);
                preview.transform.localScale = new Vector3(1, 1, 1);

                mats = GameObject.Find("previous").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(t));
                GameObject.Find("previous").GetComponent<Renderer>().materials = mats;

                mats = GameObject.Find("next").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(t));
                GameObject.Find("next").GetComponent<Renderer>().materials = mats;

                mats = GameObject.Find("zoom").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(t));
                GameObject.Find("zoom").GetComponent<Renderer>().materials = mats;

                mats = GameObject.Find("back").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                mats[1].color = new Color(1, 1, 1, Mathf.Clamp01(1 - t));
                GameObject.Find("back").GetComponent<Renderer>().materials = mats;

                mats = GameObject.Find("frame2").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(t));
                GameObject.Find("frame2").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("frame3").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(t));
                GameObject.Find("frame3").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("frame4").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(t));
                GameObject.Find("frame4").GetComponent<Renderer>().materials = mats;
                mats = GameObject.Find("frame5").GetComponent<Renderer>().materials;
                mats[0].color = new Color(1, 1, 1, Mathf.Clamp01(t));
                GameObject.Find("frame5").GetComponent<Renderer>().materials = mats;

                GameObject.Find("end").GetComponent<TextMeshPro>().text = framesToMinutes((int)previewPlayer.frameCount);

                previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(t);

                faded = Mathf.Clamp01(t);

                if (mats[0].color.a == 1)
                {
                    ball.transform.position = GameObject.Find("timeline").transform.position + GameObject.Find("timeline").transform.forward * -0.76f - GameObject.Find("timeline").transform.right * 2.18f + GameObject.Find("timeline").transform.up * 0.38f;

                    goBack = false;
                }
            }

            if (previewState != previousState)
            {
                previewPlayer.frame = (int)segment * previewState;
                scenePlayer.frame = (int)segment * previewState;
                previewText.GetComponent<TextMeshPro>().text = "Preview " + (previewState + 1) + " [" + framesToMinutes((int)segment * previewState) + "/" + framesToMinutes((int)previewPlayer.frameCount) + "]";
            }

            if (previewPlayer.frame > (int)segment * previewState + 300 || previewPlayer.frame == (long)previewPlayer.frameCount)
                previewPlayer.frame = (int)segment * previewState;

            if (scenePlayer.frame > (int)segment * previewState + 300 || scenePlayer.frame == (long)scenePlayer.frameCount)
                scenePlayer.frame = (int)segment * previewState;
        }

        // Autopilot
        else if (currentSolution == Solution.Autopilot)
        {
            GameObject startingIn = GameObject.Find("autocountdown");
            GameObject autoNumbers = GameObject.Find("autonumbers");

            if (previewPlayer.frame > (int)segment * previewState + 300 || previewPlayer.frame == (long)previewPlayer.frameCount)
            {
                Debug.Log("TRIGGER YOU FUCK");
                previewPlayer.frame = (int)segment * previewState;
            }

            if (scenePlayer.frame > (int)segment * previewState + 300 || scenePlayer.frame == (long)scenePlayer.frameCount)
            {
                Debug.Log("TRIGGER YOU FUCK");
                scenePlayer.frame = (int)segment * previewState;
            }

            if (intermission1)
            {
                autoCountdown -= Time.deltaTime;

                GameObject.Find("autonumbers").GetComponent<TextMeshPro>().text = ((int)Mathf.Max(0, autoCountdown)).ToString();

                if (autoCountdown <= 2)
                {
                    if (autoCountdown > 1.98f)
                    {
                        scenePlayer.frame = (int)segment * previewState;
                    }

                    t += Time.deltaTime / 2;

                    startingIn.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    autoNumbers.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, 1 - t);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(1 - t));
                    preview.transform.localScale = new Vector3(Mathf.Min(preview.transform.localScale.x * 1 + (t * 0.3f), 15), Mathf.Min(preview.transform.localScale.y * 1 + (t * 0.3f), 15), preview.transform.localScale.z);
                }

                if (autoCountdown <= 0)
                {
                    t = 0;
                    auto1 = true;
                    intermission1 = false;
                }
            }

            if (auto1)
            {
                t += Time.deltaTime / 2;

                GameObject player = GameObject.Find("Player");
                if (t * 100 < 360)
                {
                    player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, t * 100, player.transform.eulerAngles.z);

                    previewState = 1;
                }
                else
                {
                    if (t < 3.62f)
                    {
                        previewPlayer.frame = (int)segment * previewState;
                    }

                    previewText.GetComponent<TextMeshPro>().text = "Preview " + (previewState + 1) + " [" + framesToMinutes((int)segment * previewState) + "/" + framesToMinutes((int)previewPlayer.frameCount) + "]";
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(t - 3.6f);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, t - 3.6f);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(t - 3.6f));
                    preview.transform.localScale = new Vector3(1, 1, 1);

                    if (t >= 5.6f)
                    {
                        startingIn.GetComponent<TextMeshPro>().alpha = 1;
                        autoNumbers.GetComponent<TextMeshPro>().alpha = 1;

                        t = 0;
                        autoCountdown = 6;
                        auto1 = false;
                        intermission2 = true;
                    }
                }
            }

            if (intermission2)
            {
                autoCountdown -= Time.deltaTime;

                GameObject.Find("autonumbers").GetComponent<TextMeshPro>().text = ((int)Mathf.Max(0, autoCountdown)).ToString();

                if (autoCountdown <= 2)
                {
                    if (autoCountdown > 1.98f)
                    {
                        scenePlayer.frame = (int)segment * previewState;
                    }

                    t += Time.deltaTime / 2;

                    startingIn.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    autoNumbers.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, 1 - t);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(1 - t));
                    preview.transform.localScale = new Vector3(Mathf.Min(preview.transform.localScale.x * 1 + (t * 0.3f), 15), Mathf.Min(preview.transform.localScale.y * 1 + (t * 0.3f), 15), preview.transform.localScale.z);
                }

                if (autoCountdown <= 0)
                {
                    t = 0;
                    auto2 = true;
                    intermission2 = false;
                }
            }

            if (auto2)
            {
                t += Time.deltaTime / 2;

                GameObject player = GameObject.Find("Player");
                if (t * 100 < 360)
                {
                    player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, t * 100, player.transform.eulerAngles.z);

                    previewState = 2;
                }
                else
                {
                    if (t < 3.62f)
                    {
                        previewPlayer.frame = (int)segment * previewState;
                    }

                    previewText.GetComponent<TextMeshPro>().text = "Preview " + (previewState + 1) + " [" + framesToMinutes((int)segment * previewState) + "/" + framesToMinutes((int)previewPlayer.frameCount) + "]";
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(t - 3.6f);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, t - 3.6f);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(t - 3.6f));
                    preview.transform.localScale = new Vector3(1, 1, 1);

                    if (t >= 5.6f)
                    {
                        startingIn.GetComponent<TextMeshPro>().alpha = 1;
                        autoNumbers.GetComponent<TextMeshPro>().alpha = 1;

                        t = 0;
                        autoCountdown = 6;
                        auto2 = false;
                        intermission3 = true;
                    }
                }
            }

            if (intermission3)
            {
                autoCountdown -= Time.deltaTime;

                GameObject.Find("autonumbers").GetComponent<TextMeshPro>().text = ((int)Mathf.Max(0, autoCountdown)).ToString();

                if (autoCountdown <= 2)
                {
                    if (autoCountdown > 1.98f)
                    {
                        scenePlayer.frame = (int)segment * previewState;
                    }

                    t += Time.deltaTime / 2;

                    startingIn.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    autoNumbers.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, 1 - t);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(1 - t));
                    preview.transform.localScale = new Vector3(Mathf.Min(preview.transform.localScale.x * 1 + (t * 0.3f), 15), Mathf.Min(preview.transform.localScale.y * 1 + (t * 0.3f), 15), preview.transform.localScale.z);
                }

                if (autoCountdown <= 0)
                {
                    t = 0;
                    auto3 = true;
                    intermission3 = false;
                }
            }

            if (auto3)
            {
                t += Time.deltaTime / 2;

                GameObject player = GameObject.Find("Player");
                if (t * 100 < 360)
                {
                    player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, t * 100, player.transform.eulerAngles.z);

                    previewState = 3;
                }
                else
                {
                    if (t < 3.62f)
                    {
                        previewPlayer.frame = (int)segment * previewState;
                    }

                    previewText.GetComponent<TextMeshPro>().text = "Preview " + (previewState + 1) + " [" + framesToMinutes((int)segment * previewState) + "/" + framesToMinutes((int)previewPlayer.frameCount) + "]";
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(t - 3.6f);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, t - 3.6f);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(t - 3.6f));
                    preview.transform.localScale = new Vector3(1, 1, 1);

                    if (t >= 5.6f)
                    {
                        startingIn.GetComponent<TextMeshPro>().alpha = 1;
                        autoNumbers.GetComponent<TextMeshPro>().alpha = 1;

                        t = 0;
                        autoCountdown = 6;
                        auto3 = false;
                        intermission4 = true;
                    }
                }
            }

            if (intermission4)
            {
                autoCountdown -= Time.deltaTime;

                GameObject.Find("autonumbers").GetComponent<TextMeshPro>().text = ((int)Mathf.Max(0, autoCountdown)).ToString();

                if (autoCountdown <= 2)
                {
                    if (autoCountdown > 1.98f)
                    {
                        scenePlayer.frame = (int)segment * previewState;
                    }

                    t += Time.deltaTime / 2;

                    startingIn.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    autoNumbers.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, 1 - t);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(1 - t));
                    preview.transform.localScale = new Vector3(Mathf.Min(preview.transform.localScale.x * 1 + (t * 0.3f), 15), Mathf.Min(preview.transform.localScale.y * 1 + (t * 0.3f), 15), preview.transform.localScale.z);
                }

                if (autoCountdown <= 0)
                {
                    t = 0;
                    auto4 = true;
                    intermission4 = false;
                }
            }

            if (auto4)
            {
                t += Time.deltaTime / 2;

                GameObject player = GameObject.Find("Player");
                if (t * 100 < 360)
                {
                    player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, t * 100, player.transform.eulerAngles.z);

                    previewState = 4;
                }
                else
                {
                    if (t < 3.62f)
                    {
                        previewPlayer.frame = (int)segment * previewState;
                    }

                    previewText.GetComponent<TextMeshPro>().text = "Preview " + (previewState + 1) + " [" + framesToMinutes((int)segment * previewState) + "/" + framesToMinutes((int)previewPlayer.frameCount) + "]";
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(t - 3.6f);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, t - 3.6f);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(t - 3.6f));
                    preview.transform.localScale = new Vector3(1, 1, 1);

                    if (t >= 5.6f)
                    {
                        startingIn.GetComponent<TextMeshPro>().alpha = 1;
                        autoNumbers.GetComponent<TextMeshPro>().alpha = 1;

                        t = 0;
                        autoCountdown = 6;
                        auto4 = false;
                        intermission5 = true;
                    }
                }
            }

            if (intermission5)
            {
                autoCountdown -= Time.deltaTime;

                GameObject.Find("autonumbers").GetComponent<TextMeshPro>().text = ((int)Mathf.Max(0, autoCountdown)).ToString();

                if (autoCountdown <= 2)
                {
                    if (autoCountdown > 1.98f)
                    {
                        scenePlayer.frame = (int)segment * previewState;
                    }

                    t += Time.deltaTime / 2;

                    startingIn.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    autoNumbers.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(1 - t);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, 1 - t);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(1 - t));
                    preview.transform.localScale = new Vector3(Mathf.Min(preview.transform.localScale.x * 1 + (t * 0.3f), 15), Mathf.Min(preview.transform.localScale.y * 1 + (t * 0.3f), 15), preview.transform.localScale.z);
                }

                if (autoCountdown <= 0)
                {
                    t = 0;
                    auto5 = true;
                    intermission5 = false;
                }
            }

            if (auto5)
            {
                t += Time.deltaTime / 2;

                GameObject player = GameObject.Find("Player");
                if (t * 100 < 360)
                {
                    player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, t * 100, player.transform.eulerAngles.z);

                    previewState = 4;
                }
                else
                {
                    if (t < 3.62f)
                    {
                        previewPlayer.frame = (int)segment * previewState;
                    }

                    previewText.GetComponent<TextMeshPro>().text = "Preview " + (previewState + 1) + " [" + framesToMinutes((int)segment * previewState) + "/" + framesToMinutes((int)previewPlayer.frameCount) + "]";
                    previewText.GetComponent<TextMeshPro>().alpha = Mathf.Clamp01(t - 3.6f);

                    Material[] mats = fade.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, t - 3.6f);
                    fade.GetComponent<Renderer>().materials = mats;

                    preview.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Clamp01(t - 3.6f));
                    preview.transform.localScale = new Vector3(1, 1, 1);

                    if (t >= 5.6f)
                    {
                        startingIn.GetComponent<TextMeshPro>().alpha = 1;
                        autoNumbers.GetComponent<TextMeshPro>().alpha = 1;

                        t = 0;
                        autoCountdown = 6;
                        auto5 = false;
                        //intermission4 = true;
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (currentSolution == Solution.Exploration)
        {
            transform.position = new Vector3(transform.position.x, -1.52f, transform.position.z);
            previousState = previewState;
        }
    }

    string framesToMinutes(int frames)
    {
        int seconds = (int)(frames / previewPlayer.frameRate);
        int minutes = seconds / 60;
        seconds = seconds % 60;

        string add = "";
        if (seconds < 10)
            add = "0";

        return minutes + ":" + add + seconds;
    }
}