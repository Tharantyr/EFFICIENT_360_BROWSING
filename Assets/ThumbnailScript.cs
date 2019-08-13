using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;

public class ThumbnailScript : MonoBehaviour, IGvrPointerHoverHandler
{
    GameObject fade, thumbBall, thumbTimeline, black;
    float t = 0;
    int ms = 0;
    bool started = false;
    float cursorPosition = 0;
    float stepSize = 0.05f;
    int stepCounter = 0;
    bool DPressed = false;
    Vector3 originalOKPosition;
    bool written = false;
    string data = "";
    float fraction;
    int currentTime;
    bool notyetloaded = false;

    [HideInInspector] public MediaPlayerCtrl forwardThumb, fastForwardThumb, backThumb, forwardScene, fastForwardScene, backScene;
    [HideInInspector] public float ffTimer = 0;
    [HideInInspector] public bool switched = false;
    [HideInInspector] public bool ffMode = false;
    [HideInInspector] public bool enable = true;
    [HideInInspector] public bool thumbnailVisible = true;
    [HideInInspector] public float thumbScale = 1;
    [HideInInspector] public bool forward = true;
    [HideInInspector] public int forwardFactor = 0;
    [HideInInspector] public bool checkContent = false;
    [HideInInspector] public bool contentDone = false;
    [HideInInspector] public ContentList[] contentListArray;
    [HideInInspector] public bool contentFailed = false;
    [HideInInspector] public bool overviewCompleted = false;
    [HideInInspector] public int contentCounter = 0;
    [HideInInspector] public int videoCounter;
    [HideInInspector] public bool playbackReset = false;
    [HideInInspector] public Video[] videoOrder;
    [HideInInspector] public List<Video[]> videoOrderList;
    [HideInInspector] public TimeManipulation[] techniqueOrder;
    [HideInInspector] public List<TimeManipulation[]> techniqueOrderList;
    [HideInInspector] public int transitionCounter = 2;
    [HideInInspector] public bool paused = false;
    [HideInInspector] public bool normalSpeed = true;
    [HideInInspector] public bool fastForward = false;
    [HideInInspector] public bool rewind = false;
    [HideInInspector] public int previousSpeed = 0;
    [HideInInspector] public bool ffgazetut1 = false;
    [HideInInspector] public bool ffgazetut2 = false;
    [HideInInspector] public bool ffgazetut3 = false;
    [HideInInspector] public bool tlgazetut = false;
    [HideInInspector] public bool tlgazetutdone = false;
    [HideInInspector] public bool ffpadtut1 = false;
    [HideInInspector] public bool ffpadtut2 = false;
    [HideInInspector] public bool ffpadtut3 = false;
    [HideInInspector] public bool ffpadtut4 = false;
    [HideInInspector] public bool tlpadtut1 = false;
    [HideInInspector] public bool tlpadtut2 = false;
    [HideInInspector] public bool tlpadtut3 = false;
    [HideInInspector] public bool tlpadtut4 = false;
    public string participantName;
    public int participant = 0;
    public int taskTime = 60000;
    public int taskTimer;

    public enum TimeManipulation { FastForwardDeviceThumbnail, FastForwardDevice, TimelineDeviceThumbnail, TimelineDevice, FastForwardGazeThumbnail, TimelineGazeThumbnail };
    public TimeManipulation timeManipulation = TimeManipulation.FastForwardDeviceThumbnail;
    public enum Video { Rome, London, Bangkok, Dubrovnik, Underwater, FreeSolo, Tutorial };
    public Video video = Video.London;
    public bool overviewTask = false;
    public bool overviewTaskStarted = false;
    public bool contentTask = false;
    public bool contentTaskStarted = false;
    public bool transition = false;
    public bool overrideExperiment = false;
    public bool tutorialPhase1 = false;
    public bool tutorialPhase2 = false;

    [System.Serializable]
    public struct ContentList
    {
        [SerializeField] public Content[] contentArray;

        public ContentList(Content[] p)
        {
            contentArray = p;
        }
    }

    [System.Serializable]
    public struct Content
    {
        [SerializeField] public string name;
        [SerializeField] public int startFrame;
        [SerializeField] public int endFrame;
        [SerializeField] public string description;

        public Content(string n, int s, int e, string d)
        {
            name = n;
            startFrame = s;
            endFrame = e;
            description = d;
        }
    }

    // Use this for initialization
    void Start () {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        fade = GameObject.Find("Fade");
        black = GameObject.Find("Black");
        thumbBall = GameObject.Find("ThumbBall");
        thumbTimeline = GameObject.Find("ThumbTimeline");
        forwardThumb = GameObject.Find("ForwardThumb").GetComponent<MediaPlayerCtrl>();
        fastForwardThumb = GameObject.Find("FastForwardThumb").GetComponent<MediaPlayerCtrl>();
        forwardScene = GameObject.Find("Sphere").GetComponent<MediaPlayerCtrl>();
        fastForwardScene = GameObject.Find("FastForwardSphere").GetComponent<MediaPlayerCtrl>();
        backScene = GameObject.Find("BackSphere").GetComponent<MediaPlayerCtrl>();
        backThumb = GameObject.Find("BackThumb").GetComponent<MediaPlayerCtrl>();
        taskTimer = taskTime;

        // Initialize technique order
        techniqueOrderList = new List<TimeManipulation[]>();
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail });

        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail });

        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail });

        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail });

        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail });

        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail });

        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.TimelineGazeThumbnail, TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.FastForwardDeviceThumbnail, TimeManipulation.TimelineDeviceThumbnail });

        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail });
        techniqueOrderList.Add(new TimeManipulation[4] { TimeManipulation.FastForwardGazeThumbnail, TimeManipulation.TimelineGazeThumbnail, TimeManipulation.TimelineDeviceThumbnail, TimeManipulation.FastForwardDeviceThumbnail });

        techniqueOrder = techniqueOrderList[participant];

        if (!overrideExperiment)
            timeManipulation = techniqueOrder[0];

        // Load videos
        videoOrderList = new List<Video[]>();
        videoOrderList.Add(numberToVideo(2, 3, 4, 1));
        videoOrderList.Add(numberToVideo(4, 1, 3, 2));
        videoOrderList.Add(numberToVideo(3, 2, 1, 4));
        videoOrderList.Add(numberToVideo(1, 4, 2, 3));

        videoOrderList.Add(numberToVideo(4, 1, 2, 3));
        videoOrderList.Add(numberToVideo(2, 3, 4, 1));
        videoOrderList.Add(numberToVideo(3, 4, 1, 2));
        videoOrderList.Add(numberToVideo(1, 2, 3, 4));

        videoOrderList.Add(numberToVideo(4, 3, 2, 1));
        videoOrderList.Add(numberToVideo(3, 4, 1, 2));
        videoOrderList.Add(numberToVideo(2, 1, 4, 3));
        videoOrderList.Add(numberToVideo(1, 2, 3, 4));

        videoOrderList.Add(numberToVideo(2, 3, 1, 4));
        videoOrderList.Add(numberToVideo(1, 4, 2, 3));
        videoOrderList.Add(numberToVideo(3, 2, 4, 1));
        videoOrderList.Add(numberToVideo(4, 1, 3, 2));

        videoOrderList.Add(numberToVideo(4, 3, 1, 2));
        videoOrderList.Add(numberToVideo(3, 2, 4, 1));
        videoOrderList.Add(numberToVideo(1, 4, 2, 3));
        videoOrderList.Add(numberToVideo(2, 1, 3, 4));

        videoOrderList.Add(numberToVideo(1, 3, 2, 4));
        videoOrderList.Add(numberToVideo(4, 1, 3, 2));
        videoOrderList.Add(numberToVideo(3, 2, 4, 1));
        videoOrderList.Add(numberToVideo(2, 4, 1, 3));

        videoOrderList.Add(numberToVideo(1, 2, 3, 4));
        videoOrderList.Add(numberToVideo(4, 1, 2, 3));
        videoOrderList.Add(numberToVideo(3, 4, 1, 2));
        videoOrderList.Add(numberToVideo(2, 3, 4, 1));

        videoOrderList.Add(numberToVideo(3, 1, 4, 2));
        videoOrderList.Add(numberToVideo(1, 3, 2, 4));
        videoOrderList.Add(numberToVideo(1, 3, 2, 4));
        videoOrderList.Add(numberToVideo(2, 4, 3, 1));

        videoOrder = videoOrderList[participant];
        video = videoOrder[0];
        videoCounter = 0;

        loadVideo(Video.Tutorial);

        // Initialize content to find for user task (finding specific content)
        contentListArray = new ContentList[4]; // 6 lists of content, one for each video

        Content videoContent;
        Content[] videoContentArray;
        ContentList videoContentList;

        for (int i = 0; i < 4; i++)
        {
            switch(videoOrder[i])
            {
                case Video.Rome:
                    videoContent = new Content("Rome", 150000, 178000, "Find the scene with the green fountain.\n\nPlease gaze at the \"OK\" button to confirm your selection.");
                    videoContentArray = new Content[1];
                    videoContentArray[0] = videoContent;
                    videoContentList = new ContentList(videoContentArray);
                    contentListArray[i] = videoContentList;
                    break;
                case Video.London:
                    videoContent = new Content("London", 90000, 118000, "Find the scene with the guitar player.\n\nPlease gaze at the \"OK\" button to confirm your selection.");
                    videoContentArray = new Content[1];
                    videoContentArray[0] = videoContent;
                    videoContentList = new ContentList(videoContentArray);
                    contentListArray[i] = videoContentList;
                    break;
                case Video.Bangkok:
                    videoContent = new Content("Bangkok", 210000, 332000, "Find the scene with the scantily clad woman.\n\nPlease gaze at the \"OK\" button to confirm your selection.");
                    videoContentArray = new Content[1];
                    videoContentArray[0] = videoContent;
                    videoContentList = new ContentList(videoContentArray);
                    contentListArray[i] = videoContentList;
                    break;
                case Video.Dubrovnik:
                    videoContent = new Content("Dubrovnik", 210000, 332000, "Find the scene with the scantily clad woman.\n\nPlease gaze at the \"OK\" button to confirm your selection.");
                    videoContentArray = new Content[1];
                    videoContentArray[0] = videoContent;
                    videoContentList = new ContentList(videoContentArray);
                    contentListArray[i] = videoContentList;
                    break;
                case Video.FreeSolo:
                    videoContent = new Content("FreeSolo", 190000, 210000, "Find the scene with the glowing mountain.\n\nPlease gaze at the \"OK\" button to confirm your selection.");
                    videoContentArray = new Content[1];
                    videoContentArray[0] = videoContent;
                    videoContentList = new ContentList(videoContentArray);
                    contentListArray[i] = videoContentList;
                    break;
                case Video.Underwater:
                    videoContent = new Content("Underwater", 176000, 200000, "Find the scene where the turtles are walking towards the sea.\n\nPlease gaze at the \"OK\" button to confirm your selection.");
                    videoContentArray = new Content[1];
                    videoContentArray[0] = videoContent;
                    videoContentList = new ContentList(videoContentArray);
                    contentListArray[i] = videoContentList;
                    break;
            }
        }

        if (enable)
            forwardThumb.GetComponent<MeshRenderer>().enabled = true;
        else
            forwardThumb.GetComponent<MeshRenderer>().enabled = false;

        Material[] mats = GameObject.Find("OK").GetComponent<Renderer>().materials;
        mats[0].color = new Color(1, 1, 1, 1);
        mats[1].color = new Color(1, 1, 1, 0);
        GameObject.Find("OK").GetComponent<Renderer>().materials = mats;

        mats = GameObject.Find("Play").GetComponent<Renderer>().materials;
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


        mats = black.GetComponent<Renderer>().materials;
        mats[0].color = new Color(0, 0, 0, 0);
        black.GetComponent<Renderer>().materials = mats;
    }

    public Video[] numberToVideo(int n1, int n2, int n3, int n4)
    {
        int[] nList = new int[4] { n1, n2, n3, n4 };
        Video[] videoList = new Video[4];

        for (int i = 0; i < 4; i++)
        {
            int n = nList[i];

            switch (n)
            {
                case 1:
                    videoList[i] = Video.Rome;
                break;
                case 2:
                    videoList[i] = Video.FreeSolo;
                    break;
                case 3:
                    videoList[i] = Video.London;
                break;
                case 4:
                    videoList[i] = Video.Underwater;
                break;
            }
        }

        return videoList;
    }

    public void OnGvrPointerHover(PointerEventData data)
    {
    }

    public void loadVideo(Video v)
    {
        switch (v)
        {
            case Video.Rome:
                forwardThumb.Load("rome.mp4");
                forwardScene.Load("rome.mp4");
                fastForwardThumb.Load("rome_fastforward.mp4");
                fastForwardScene.Load("rome_fastforward.mp4");
                backThumb.Load("rome_rewind.mp4");
                backScene.Load("rome_rewind.mp4");
                break;
            case Video.London:
                forwardThumb.Load("london.mp4");
                forwardScene.Load("london.mp4");
                fastForwardThumb.Load("london_fastforward.mp4");
                fastForwardScene.Load("london_fastforward.mp4");
                backThumb.Load("london_rewind.mp4");
                backScene.Load("london_rewind.mp4");
                break;
            case Video.Bangkok:
                forwardThumb.Load("bangkok.mp4");
                forwardScene.Load("bangkok.mp4");
                backThumb.Load("bangkok_backwards.mp4");
                backScene.Load("bangkok_backwards.mp4");
                break;
            case Video.Dubrovnik:
                forwardThumb.Load("dubrovnik.mp4");
                forwardScene.Load("dubrovnik.mp4");
                backThumb.Load("dubrovnik_backwards.mp4");
                backScene.Load("dubrovnik_backwards.mp4");
                break;
            case Video.Underwater:
                forwardThumb.Load("underwater.mp4");
                forwardScene.Load("underwater.mp4");
                fastForwardThumb.Load("underwater_fastforward.mp4");
                fastForwardScene.Load("underwater_fastforward.mp4");
                backThumb.Load("underwater_rewind.mp4");
                backScene.Load("underwater_rewind.mp4");
                break;
            case Video.FreeSolo:
                forwardThumb.Load("freesolo.mp4");
                forwardScene.Load("freesolo.mp4");
                fastForwardThumb.Load("freesolo_fastforward.mp4");
                fastForwardScene.Load("freesolo_fastforward.mp4");
                backThumb.Load("freesolo_rewind.mp4");
                backScene.Load("freesolo_rewind.mp4");
                break;
            case Video.Tutorial:
                forwardThumb.Load("tutorial.mp4");
                forwardScene.Load("tutorial.mp4");
                fastForwardThumb.Load("tutorial_fastforward.mp4");
                fastForwardScene.Load("tutorial_fastforward.mp4");
                backThumb.Load("tutorial_rewind.mp4");
                backScene.Load("tutorial_rewind.mp4");
                break;
        }
    }

    public void Gamepad() // Gamepad behavior during different phases of the game
    {
        if (tutorialPhase1 || tutorialPhase2)
        {
            if (!tlpadtut1)
            {
                GameObject.Find("Gamepad").GetComponent<MeshRenderer>().enabled = true;
                GameObject.Find("tlpad_tut1").GetComponent<MeshRenderer>().enabled = true;
                tlpadtut1 = true;
            }

            if (tlpadtut1 && !tlpadtut2)
            {
                if (stepSize > 0.5f)
                {
                    GameObject.Find("tlpad_tut1").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("tlpad_tut2").GetComponent<MeshRenderer>().enabled = true;
                    tlpadtut2 = true;
                }
            }

            if (tlpadtut2 && !tlpadtut3)
            {
                if (stepSize <= 0.5f)
                {
                    GameObject.Find("tlpad_tut2").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("tlpad_tut3").GetComponent<MeshRenderer>().enabled = true;
                    tlpadtut3 = true;
                }
            }

            if (tlpadtut3  && !tlpadtut4)
            {
                if (stepCounter == 3)
                {
                    stepCounter = 0;
                    GameObject.Find("tlpad_tut3").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("tlpad_tut4").GetComponent<MeshRenderer>().enabled = true;
                    tlpadtut4 = true;
                }
            }

            if (tlpadtut4)
            {
                if (stepCounter == 3)
                {
                    GameObject.Find("OK").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("Gamepad").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("tlpad_tut4").GetComponent<MeshRenderer>().enabled = false;

                    if (timeManipulation == techniqueOrder[0] || timeManipulation == techniqueOrder[2])
                        GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "First part of the tutorial complete!\n\nGaze at the \"OK\" button above to continue to the second part.";
                    else
                        GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "Second part of the tutorial complete!\n\nGaze at the \"OK\" button above when you are ready to proceed with the experiment.";
                }
            }
        }

        GameObject ghost = GameObject.Find("ThumbGhost");
        ghost.transform.position = GameObject.Find("ThumbBall").transform.position + ghost.transform.right * stepSize;

        GameObject ghost2 = GameObject.Find("ThumbGhost2");
        ghost2.transform.position = GameObject.Find("ThumbBall").transform.position - ghost2.transform.right * stepSize;

        if (Input.GetAxisRaw("DVertical") == 1)
            stepSize = Mathf.Max(0, stepSize - 0.02f);

        else if (Input.GetAxisRaw("DVertical") == -1)
            stepSize = Mathf.Min(1, stepSize + 0.02f);

        else if (Input.GetAxisRaw("DHorizontal") == -1)
        {
            if (!DPressed)
            {
                if (tlpadtut4 && stepCounter < 3)
                    stepCounter++;

                float value = 0;

                if (Vector3.Distance(thumbTimeline.transform.position - thumbTimeline.transform.forward * 0.42f + Vector3.up * 0.18f - thumbTimeline.transform.right * 1.05f + thumbTimeline.transform.right * 2.1f, ghost2.transform.position) < 2.1f)
                    value = Vector3.Distance(thumbTimeline.transform.position - thumbTimeline.transform.forward * 0.42f + Vector3.up * 0.18f - thumbTimeline.transform.right * 1.05f, ghost2.transform.position);

                cursorPosition = Mathf.Clamp(value, 0, 2.1f);

                float playbackTime = cursorPosition / 2.1f;
                forwardThumb.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                backThumb.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                forwardScene.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                backScene.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));

                DPressed = true;
            }
        }

        else if (Input.GetAxisRaw("DHorizontal") == 1)
        {
            float value = 2.1f;

            if (Vector3.Distance(thumbTimeline.transform.position - thumbTimeline.transform.forward * 0.42f + Vector3.up * 0.18f - thumbTimeline.transform.right * 1.05f, ghost.transform.position) < 2.1f)
                value = Vector3.Distance(thumbTimeline.transform.position - thumbTimeline.transform.forward * 0.42f + Vector3.up * 0.18f - thumbTimeline.transform.right * 1.05f, ghost.transform.position);

            if (!DPressed)
            {
                if (tlpadtut3 && stepCounter < 3)
                    stepCounter++;

                cursorPosition = Mathf.Clamp(value, 0, 2.1f);

                float playbackTime = cursorPosition / 2.1f;
                forwardThumb.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                backThumb.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                forwardScene.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));
                backScene.SeekTo((int)(playbackTime * forwardThumb.GetDuration()));

                DPressed = true;
            }
        }

        else
            DPressed = false;
    }

    public void ExecuteUserTasks() // Implementing each task that the test participant should perform during testing
    {
        GameObject.Find("OK").transform.rotation = GameObject.Find("Camera").transform.rotation;
        GameObject.Find("Correct").transform.rotation = GameObject.Find("Camera").transform.rotation;

        if (overviewTask)
        {
            if (!overviewTaskStarted)
            {
                if (!playbackReset)
                {
                    forwardThumb.SeekTo(0);
                    forwardScene.SeekTo(0);
                    backThumb.SeekTo(backThumb.GetDuration());
                    backScene.SeekTo(backThumb.GetDuration());
                    playbackReset = true;
                }

                forwardThumb.transform.position = new Vector3(forwardThumb.transform.position.x, -200, forwardThumb.transform.position.z);
                backThumb.transform.position = new Vector3(backThumb.transform.position.x, -200, backThumb.transform.position.z);

                paused = true;

                Material[] mats = black.GetComponent<Renderer>().materials;
                mats[0].color = new Color(0, 0, 0, 1);
                black.GetComponent<Renderer>().materials = mats;

                GameObject.Find("TaskText").GetComponent<TextMeshPro>().alpha = 1;
            }
            else
            {
                paused = false;

                if (forwardThumb.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                {
                    forwardThumb.Play();
                    forwardScene.Play();
                    backThumb.Play();
                    backThumb.Play();
                }

                Material[] mats = black.GetComponent<Renderer>().materials;
                mats[0].color = new Color(0, 0, 0, 0);
                black.GetComponent<Renderer>().materials = mats;

                taskTimer -= (int)(Time.deltaTime * 1000);
                taskTimer = (int)Mathf.Max(0, taskTimer);
                GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().text = framesToMinutes(taskTimer);

                if (taskTimer == 0)
                {
                    paused = true;

                    mats = black.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, 1);
                    black.GetComponent<Renderer>().materials = mats;

                    GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().alpha = 0;
                    GameObject.Find("TaskText").GetComponent<TextMeshPro>().alpha = 1;
                    GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "Time is up. Please notify the observer.";
                    GameObject.Find("OK").GetComponent<MeshRenderer>().enabled = true;
                    overviewCompleted = true;

                    thumbBall.transform.position = thumbTimeline.transform.position - thumbTimeline.transform.forward * 0.42f + Vector3.up * 0.18f - thumbTimeline.transform.right * 1.05f;
                }
            }
        }

        if (contentTask)
        {
            if (!contentTaskStarted)
            {
                GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "CONTENT TASK " + (videoCounter + 1) + "\n\nWe will start with the next task. If the observer has given you permission, please gaze at the \"OK\" button above to begin.\n\nYou have max. 1 minute to complete the task.";

                forwardThumb.transform.position = new Vector3(forwardThumb.transform.position.x, -200, forwardThumb.transform.position.z);
                backThumb.transform.position = new Vector3(backThumb.transform.position.x, -200, backThumb.transform.position.z);
            }

            else
            {
                paused = false;

                Material[] mats = black.GetComponent<Renderer>().materials;
                mats[0].color = new Color(0, 0, 0, 0);
                black.GetComponent<Renderer>().materials = mats;

                if (!contentDone)
                {
                    taskTimer -= (int)(Time.deltaTime * 1000);
                    taskTimer = (int)Mathf.Max(0, taskTimer);
                    GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().text = framesToMinutes(taskTimer);
                }

                if (taskTimer == 0)
                {
                    paused = true;

                    mats = black.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, 1);
                    black.GetComponent<Renderer>().materials = mats;

                    if (!contentDone)
                    {
                        data += contentListArray[videoCounter].contentArray[contentCounter].name + ": " + "timed out\n";
                        GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "Time is up. Please notify the observer.";
                        GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().text = "";
                    }

                    contentDone = true;
                }

                if (checkContent)
                {
                    GameObject.Find("TaskTimer").GetComponent<TextMeshPro>().text = "";

                    paused = true;

                    mats = black.GetComponent<Renderer>().materials;
                    mats[0].color = new Color(0, 0, 0, 1);
                    black.GetComponent<Renderer>().materials = mats;

                    if (!contentDone)
                    {
                        if (forwardThumb.GetSeekPosition() > contentListArray[videoCounter].contentArray[contentCounter].startFrame &&
                            forwardThumb.GetSeekPosition() < contentListArray[videoCounter].contentArray[contentCounter].endFrame)
                        {
                            Debug.Log("Correct");
                            data += contentListArray[videoCounter].contentArray[contentCounter].name + ": " + (taskTime - taskTimer) + " ms, correct\n";
                            GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "Task complete. Please notify the observer.";
                        }
                        else
                        {
                            Debug.Log("Incorrect");
                            data += contentListArray[videoCounter].contentArray[contentCounter].name + ": " + (taskTime - taskTimer) + " ms, incorrect\n";
                            GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "Task complete. Please notify the observer.";
                        }
                    }

                    contentDone = true;
                }
            }
        }

        if (GameObject.Find("AllDone").GetComponent<MeshRenderer>().enabled)
        {
            Material[] mats = black.GetComponent<Renderer>().materials;
            mats[0].color = new Color(0, 0, 0, 1);
            black.GetComponent<Renderer>().materials = mats;

            if (!written)
            {
                Debug.Log("OMEGALUL");
                File.WriteAllText(Application.persistentDataPath + "/" + participantName + ".txt", data);
                written = true;
            }
        }
    }

    void Update() // Standard update method
    {
        if (Input.GetButtonDown("Select"))
            taskTimer = 0;

        if (Input.GetButtonDown("Start"))
        {
            if (fastForward)
                currentTime = fastForwardThumb.GetSeekPosition();
            else if (normalSpeed)
                currentTime = forwardThumb.GetSeekPosition();
            else if (rewind)
                currentTime = backThumb.GetSeekPosition();

            loadVideo(video);

            notyetloaded = true;
        }

        if (notyetloaded)
        {
            if (fastForward)
            {
                if (fastForwardThumb.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                {
                    fastForwardThumb.SeekTo(currentTime);
                    notyetloaded = false;
                }
            }

            else if (normalSpeed)
            {
                if (forwardThumb.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                {
                    Debug.Log("HELLO");
                    forwardThumb.SeekTo(currentTime);
                    notyetloaded = false;
                }
            }

            else if (rewind)
            {
                if (backThumb.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                {
                    backThumb.SeekTo(currentTime);
                    notyetloaded = false;
                }
            }
        }

        forwardThumb.SetSpeed(1f);
        backScene.SetSpeed(1f);
        forwardScene.SetSpeed(1f);
        backThumb.SetSpeed(1f);

        if (forwardThumb.GetSeekPosition() > 50 && !started)
        {
            forwardThumb.SetVolume(0);
            fastForwardThumb.SetVolume(0);
            backScene.SetVolume(0);
            forwardScene.SetVolume(0);
            fastForwardScene.SetVolume(0);
            backThumb.SetVolume(0);
            started = true;
        }

        ExecuteUserTasks();

        if (paused)
        {
            GameObject.Find("ThumbTimeline").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("ThumbBall").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("ThumbStamp").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("ThumbStart").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("ThumbEnd").GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GameObject.Find("ThumbTimeline").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("ThumbBall").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("ThumbStamp").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("ThumbStart").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("ThumbEnd").GetComponent<MeshRenderer>().enabled = true;
        }

        forwardThumb.transform.localScale = new Vector3(forwardThumb.transform.localScale.x * thumbScale, forwardThumb.transform.localScale.y * thumbScale, 0);
        backThumb.transform.localScale = new Vector3(backThumb.transform.localScale.x * thumbScale, backThumb.transform.localScale.y * thumbScale, 0);

        float ballPosition;
        fraction = (float)fastForwardThumb.GetDuration() / (float)forwardThumb.GetDuration();

        ms = forwardThumb.GetSeekPosition();
        GameObject.Find("ThumbStart").GetComponent<TextMeshPro>().text = framesToMinutes(ms);
        GameObject.Find("ThumbStamp").GetComponent<TextMeshPro>().text = framesToMinutes(ms) + "/" + framesToMinutes(forwardThumb.GetDuration());
        ballPosition = (float)ms / forwardThumb.GetDuration();

        if (timeManipulation == TimeManipulation.FastForwardDeviceThumbnail || timeManipulation == TimeManipulation.FastForwardGazeThumbnail)
        {
            if (fastForward)
            {
                ms = fastForwardThumb.GetSeekPosition();
                GameObject.Find("ThumbStart").GetComponent<TextMeshPro>().text = framesToMinutes(ms);
                GameObject.Find("ThumbStamp").GetComponent<TextMeshPro>().text = framesToMinutes((int)(ms / fraction)) + "/" + framesToMinutes(forwardThumb.GetDuration());
                ballPosition = (float)ms / fastForwardThumb.GetDuration();
            }
            if (rewind)
            {
                ms = backThumb.GetDuration() - backThumb.GetSeekPosition();
                GameObject.Find("ThumbStart").GetComponent<TextMeshPro>().text = framesToMinutes(ms);
                GameObject.Find("ThumbStamp").GetComponent<TextMeshPro>().text = framesToMinutes((int)(ms / fraction)) + "/" + framesToMinutes(forwardThumb.GetDuration());
                ballPosition = (float)ms / backThumb.GetDuration();
            }
        }

        thumbBall.transform.position = thumbTimeline.transform.position - thumbTimeline.transform.forward * 0.42f + Vector3.up * 0.18f - thumbTimeline.transform.right * 1.05f + thumbTimeline.transform.right * ballPosition * 2.1f;

        if (enable)
        {
            t += Time.deltaTime / 2;

            Material[] mats = fade.GetComponent<Renderer>().materials;

            if (timeManipulation == TimeManipulation.FastForwardDevice || timeManipulation == TimeManipulation.TimelineDevice || (overviewTaskStarted && taskTimer == 0) || checkContent || (contentTaskStarted && taskTimer == 0) || GameObject.Find("AllDone").GetComponent<MeshRenderer>().enabled)
            {
                Transform thumbTransform = GameObject.Find("ForwardThumb").transform;
                thumbTransform.position = new Vector3(thumbTransform.position.x, -200, thumbTransform.position.z);
                thumbTransform = GameObject.Find("BackThumb").transform;
                thumbTransform.position = new Vector3(thumbTransform.position.x, -200, thumbTransform.position.z);

                mats[0].color = new Color(0, 0, 0, Mathf.Min(t, 0f));
                fade.GetComponent<Renderer>().materials = mats;
            }

            if (!(timeManipulation == TimeManipulation.FastForwardDevice || timeManipulation == TimeManipulation.TimelineDevice || (overviewTaskStarted && taskTimer == 0) || checkContent || (contentTaskStarted && taskTimer == 0) || GameObject.Find("AllDone").GetComponent<MeshRenderer>().enabled))
            {
                if (!(overviewTask && !overviewTaskStarted) && !(contentTask && !contentTaskStarted))
                {
                    Transform thumbTransform = GameObject.Find("ForwardThumb").transform;
                    thumbTransform.position = new Vector3(thumbTransform.position.x, 0, thumbTransform.position.z);
                    thumbTransform = GameObject.Find("BackThumb").transform;
                    thumbTransform.position = new Vector3(thumbTransform.position.x, 0, thumbTransform.position.z);
                }

                mats = fade.GetComponent<Renderer>().materials;
                mats[0].color = new Color(0, 0, 0, Mathf.Min(t, 0.5f));
                fade.GetComponent<Renderer>().materials = mats;
            }

            if (((timeManipulation == TimeManipulation.FastForwardDeviceThumbnail || timeManipulation == TimeManipulation.FastForwardGazeThumbnail) && !(contentTask && !contentTaskStarted) && !(overviewTask && !overviewTaskStarted) && taskTimer > 0 && !GameObject.Find("AllDone").GetComponent<MeshRenderer>().enabled) || ((timeManipulation == TimeManipulation.FastForwardDeviceThumbnail || timeManipulation == TimeManipulation.FastForwardGazeThumbnail) && (tutorialPhase1 || tutorialPhase2)))
            {
                GameObject.Find("Play").GetComponent<MeshRenderer>().enabled = true;
                GameObject.Find("FastForward").GetComponent<MeshRenderer>().enabled = true;
                GameObject.Find("Rewind").GetComponent<MeshRenderer>().enabled = true;
                FastForwarding();
            }

            if ((!(timeManipulation == TimeManipulation.FastForwardDeviceThumbnail || timeManipulation == TimeManipulation.FastForwardGazeThumbnail) || (contentTask && !contentTaskStarted) || (overviewTask && !overviewTaskStarted) || taskTimer == 0 || GameObject.Find("AllDone").GetComponent<MeshRenderer>().enabled) && !((timeManipulation == TimeManipulation.FastForwardDeviceThumbnail || timeManipulation == TimeManipulation.FastForwardGazeThumbnail) && (tutorialPhase1 || tutorialPhase2)))
            {
                if (fastForward)
                {
                    forwardThumb.SeekTo((int)(fastForwardThumb.GetSeekPosition() / fraction));
                    forwardScene.SeekTo((int)(fastForwardThumb.GetSeekPosition() / fraction));
                }
                else if (rewind)
                {
                    forwardThumb.SeekTo((int)((backThumb.GetDuration() - backThumb.GetSeekPosition()) / fraction));
                    forwardThumb.SeekTo((int)((backThumb.GetDuration() - backThumb.GetSeekPosition()) / fraction));
                }

                normalSpeed = true;
                fastForward = false;
                rewind = false;

                mats = GameObject.Find("Play").GetComponent<Renderer>().materials;
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

                GameObject.Find("Play").GetComponent<MeshRenderer>().enabled = false;
                GameObject.Find("FastForward").GetComponent<MeshRenderer>().enabled = false;
                GameObject.Find("Rewind").GetComponent<MeshRenderer>().enabled = false;
                forwardThumb.GetComponent<MeshRenderer>().enabled = true;
                forwardScene.GetComponent<MeshRenderer>().enabled = true;
                fastForwardThumb.GetComponent<MeshRenderer>().enabled = false;
                fastForwardScene.GetComponent<MeshRenderer>().enabled = false;
                backThumb.GetComponent<MeshRenderer>().enabled = false;
                backScene.GetComponent<MeshRenderer>().enabled = false;
            }

            if (timeManipulation == TimeManipulation.TimelineDeviceThumbnail || timeManipulation == TimeManipulation.TimelineDevice)
            {
                Gamepad();

                if (!paused)
                {
                    GameObject.Find("ThumbGhost").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("ThumbGhost2").GetComponent<MeshRenderer>().enabled = true;
                }
                else
                {
                    GameObject.Find("ThumbGhost").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("ThumbGhost2").GetComponent<MeshRenderer>().enabled = false;
                }
            }

            if (!(timeManipulation == TimeManipulation.TimelineDeviceThumbnail || timeManipulation == TimeManipulation.TimelineDevice))
            {
                GameObject.Find("ThumbGhost").GetComponent<MeshRenderer>().enabled = false;
                GameObject.Find("ThumbGhost2").GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void FastForwarding() // Implement fast-forwarding/rewinding behavior
    {

        if (timeManipulation == TimeManipulation.FastForwardDeviceThumbnail)
        {
            if (tutorialPhase1 || tutorialPhase2)
            {
                if (!ffpadtut1)
                {
                    forwardThumb.SeekTo(forwardThumb.GetDuration() / 2);
                    GameObject.Find("Gamepad").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("ffpad_tut1").GetComponent<MeshRenderer>().enabled = true;
                    ffpadtut1 = true;
                }
            }

            if (Input.GetButtonDown("R") || Input.GetButtonDown("R2"))
            {
                if (normalSpeed)
                {
                    if ((tutorialPhase1 || tutorialPhase2) && (ffpadtut1 && !ffpadtut3))
                    {
                        GameObject.Find("ffpad_tut1").GetComponent<MeshRenderer>().enabled = false;
                        GameObject.Find("ffpad_tut2").GetComponent<MeshRenderer>().enabled = true;
                        ffpadtut2 = true;
                    }

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

                    normalSpeed = false;
                    fastForward = true;
                }
                else if (rewind)
                {
                    if ((tutorialPhase1 || tutorialPhase2) && ffpadtut4)
                    {
                        GameObject.Find("OK").GetComponent<MeshRenderer>().enabled = true;
                        GameObject.Find("Gamepad").GetComponent<MeshRenderer>().enabled = false;
                        GameObject.Find("ffpad_tut4").GetComponent<MeshRenderer>().enabled = false;

                        if (timeManipulation == techniqueOrder[0] || timeManipulation == techniqueOrder[2])
                        GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "First part of the tutorial complete!\n\nGaze at the \"OK\" button above to continue to the second part.";
                    else
                        GameObject.Find("TaskText").GetComponent<TextMeshPro>().text = "Second part of the tutorial complete!\n\nGaze at the \"OK\" button above when you are ready to proceed with the experiment.";
                    }

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

                    rewind = false;
                    normalSpeed = true;
                }
            }
            else if (Input.GetButtonDown("L") || Input.GetButtonDown("L2"))
            {
                if (normalSpeed)
                {
                    if ((tutorialPhase1 || tutorialPhase2) && (ffpadtut3 && !ffpadtut4))
                    {
                        GameObject.Find("ffpad_tut3").GetComponent<MeshRenderer>().enabled = false;
                        GameObject.Find("ffpad_tut4").GetComponent<MeshRenderer>().enabled = true;
                        ffpadtut4 = true;
                    }

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

                    normalSpeed = false;
                    rewind = true;
                }
                else if (fastForward)
                {
                    if ((tutorialPhase1 || tutorialPhase2) && (ffpadtut2 && !ffpadtut3))
                    {
                        GameObject.Find("ffpad_tut2").GetComponent<MeshRenderer>().enabled = false;
                        GameObject.Find("ffpad_tut3").GetComponent<MeshRenderer>().enabled = true;
                        ffpadtut3 = true;
                    }

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

                    fastForward = false;
                    normalSpeed = true;
                }
            }
        }

        if (normalSpeed)
        {
            if (!forwardThumb.GetComponent<MeshRenderer>().enabled)
            {
                if (previousSpeed == 1) // If previously fast-forwarding
                {
                    fastForwardThumb.GetComponent<MeshRenderer>().enabled = false;
                    fastForwardScene.GetComponent<MeshRenderer>().enabled = false;
                    forwardThumb.SeekTo((int)(fastForwardThumb.GetSeekPosition() / fraction));
                    forwardScene.SeekTo((int)(fastForwardThumb.GetSeekPosition() / fraction));
                }
                else if (previousSpeed == 2) // If previously rewinding
                {
                    backThumb.GetComponent<MeshRenderer>().enabled = false;
                    backScene.GetComponent<MeshRenderer>().enabled = false;
                    forwardThumb.SeekTo((int)((backThumb.GetDuration() - backThumb.GetSeekPosition()) / fraction));
                    forwardScene.SeekTo((int)((backThumb.GetDuration() - backThumb.GetSeekPosition()) / fraction));
                }

                if (forwardThumb.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                    forwardThumb.Play();

                forwardThumb.GetComponent<MeshRenderer>().enabled = true;
                forwardScene.GetComponent<MeshRenderer>().enabled = true;
            }
            else
                previousSpeed = 0;
        }

        else if (fastForward)
        {
            if (!fastForwardThumb.GetComponent<MeshRenderer>().enabled)
            {
                if (previousSpeed == 0) // If previously at normal speed
                {
                    forwardThumb.GetComponent<MeshRenderer>().enabled = false;
                    forwardScene.GetComponent<MeshRenderer>().enabled = false;
                    fastForwardThumb.SeekTo((int)(forwardThumb.GetSeekPosition() * fraction));
                    fastForwardScene.SeekTo((int)(forwardThumb.GetSeekPosition() * fraction));
                }
                else if (previousSpeed == 2) // If previously rewinding
                {
                    backThumb.GetComponent<MeshRenderer>().enabled = false;
                    backScene.GetComponent<MeshRenderer>().enabled = false;
                    fastForwardThumb.SeekTo(backThumb.GetDuration() - backThumb.GetSeekPosition());
                    fastForwardScene.SeekTo(backThumb.GetDuration() - backThumb.GetSeekPosition());
                }

                if (fastForwardThumb.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                    fastForwardThumb.Play();

                fastForwardThumb.GetComponent<MeshRenderer>().enabled = true;
                fastForwardScene.GetComponent<MeshRenderer>().enabled = true;
            }
            else
                previousSpeed = 1;
        }

        else if (rewind)
        {
            if (!backThumb.GetComponent<MeshRenderer>().enabled)
            {
                if (previousSpeed == 0) // If previously at normal speed
                {
                    forwardThumb.GetComponent<MeshRenderer>().enabled = false;
                    forwardScene.GetComponent<MeshRenderer>().enabled = false;
                    Debug.Log("huh? " + fraction);
                    backThumb.SeekTo((int)((forwardThumb.GetDuration() - forwardThumb.GetSeekPosition()) * fraction));
                    backScene.SeekTo((int)((forwardThumb.GetDuration() - forwardThumb.GetSeekPosition()) * fraction));
                }
                else if (previousSpeed == 1) // If previously fast-forwarding
                {
                    fastForwardThumb.GetComponent<MeshRenderer>().enabled = false;
                    fastForwardScene.GetComponent<MeshRenderer>().enabled = false;
                    backThumb.SeekTo(fastForwardThumb.GetDuration() - fastForwardThumb.GetSeekPosition());
                    backScene.SeekTo(fastForwardThumb.GetDuration() - fastForwardThumb.GetSeekPosition());
                }

                if (backThumb.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                    backThumb.Play();

                backThumb.GetComponent<MeshRenderer>().enabled = true;
                backScene.GetComponent<MeshRenderer>().enabled = true;
            }
            else
                previousSpeed = 2;
        }
    }

    string framesToMinutes(int ms) // Transform frames into minutes for displaying purposes
    {
        int seconds = ms / 1000;
        int minutes = seconds / 60;
        seconds = seconds % 60;

        string add = "";
        if (seconds < 10)
            add = "0";

        return minutes + ":" + add + seconds;
    }
}
