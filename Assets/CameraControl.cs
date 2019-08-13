using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public enum Solution { Panels, RotationMapping, None };
    public Solution solution = Solution.Panels;

    [Header("Rotation")]
    public float rotationSpeed = 0.5f;
    public bool scalingRotationSpeed = true;
    public float scalingRotationFactor = 5;
    public int rotationStartingAngle = 70;
    [Space(10)]

    [Header("Panels")]
    public bool visiblePanels = false;
    public bool intrusivePanels = false;
    [Space(10)]

    [Header("Gradients")]
    public bool gradientsOn = false;
    public bool greensOff = false;
    [Space(10)]

    [Header("Rotation Mapping")]
    public int rotationMappingAngle = 120;
    public int orientation = 0;

    GvrReticlePointer reticlePointer;
    CameraControl cameraControl;
    GameObject UnintrusiveLeft, UnintrusiveRight, RotateLeft, RotateRight, LeftMax, RightMax, origin;
    Material leftOpaquePanel, rightOpaquePanel, leftPanel, rightPanel;
    Vector3 prevOrientation, forward;
    int prevOrigin = 0;

    // Use this for initialization
    void Start () {
        cameraControl = GetComponent<CameraControl>();
        UnintrusiveLeft = GameObject.Find("UnintrusiveLeft");
        UnintrusiveRight = GameObject.Find("UnintrusiveRight");
        LeftMax = GameObject.Find("LeftMax");
        RightMax = GameObject.Find("RightMax");
        RotateLeft = GameObject.Find("RotateLeft");
        RotateRight = GameObject.Find("RotateRight");
        reticlePointer = GameObject.Find("GvrReticlePointer").GetComponent<FreshReticlePointer>();
        leftPanel = (Material)Resources.Load("rotateleft", typeof(Material));
        rightPanel = (Material)Resources.Load("rotateleft", typeof(Material));
        leftOpaquePanel = (Material)Resources.Load("rotatelefto", typeof(Material));
        rightOpaquePanel = (Material)Resources.Load("rotatelefto", typeof(Material));

        // Adjust starting angle of rotation panels
        RotateLeft.transform.RotateAround(transform.position, new Vector3(0, 1, 0), 90 - rotationStartingAngle);
        RotateRight.transform.RotateAround(transform.position, new Vector3(0, 1, 0), -90 + rotationStartingAngle);

        origin = new GameObject();
        origin.transform.eulerAngles = new Vector3(transform.eulerAngles.x, orientation, transform.eulerAngles.z);
        forward = origin.transform.forward;
        prevOrientation = origin.transform.forward;
    }
	
	// Update is called once per frame
	void Update () {
        // Set opacities initially to 0
        Color UnintrusiveLeftColor = UnintrusiveLeft.GetComponent<MeshRenderer>().material.color;
        UnintrusiveLeft.GetComponent<MeshRenderer>().material.color = new Color(UnintrusiveLeftColor.r, UnintrusiveLeftColor.g, UnintrusiveLeftColor.b, 0);
        Color LeftMaxColor = LeftMax.GetComponent<MeshRenderer>().material.color;
        LeftMax.GetComponent<MeshRenderer>().material.color = new Color(LeftMaxColor.r, LeftMaxColor.g, LeftMaxColor.b, 0);

        Color UnintrusiveRightColor = UnintrusiveRight.GetComponent<MeshRenderer>().material.color;
        UnintrusiveRight.GetComponent<MeshRenderer>().material.color = new Color(UnintrusiveRightColor.r, UnintrusiveRightColor.g, UnintrusiveRightColor.b, 0);
        Color RightMaxColor = RightMax.GetComponent<MeshRenderer>().material.color;
        RightMax.GetComponent<MeshRenderer>().material.color = new Color(RightMaxColor.r, RightMaxColor.g, RightMaxColor.b, 0);

        Color RotateLeftColor = RotateLeft.GetComponent<MeshRenderer>().material.color;
        RotateLeft.GetComponent<MeshRenderer>().material.color = new Color(RotateLeftColor.r, RotateLeftColor.g, RotateLeftColor.b, 0);

        Color RotateRightColor = RotateRight.GetComponent<MeshRenderer>().material.color;
        RotateRight.GetComponent<MeshRenderer>().material.color = new Color(RotateRightColor.r, RotateRightColor.g, RotateRightColor.b, 0);

        if (solution == Solution.Panels)
        {
            float reticleDistanceLeft = 1 - (Vector3.Distance(RotateLeft.transform.position, reticlePointer.PointerTransform.forward * reticlePointer.maxReticleDistance) - 1.75f);
            float reticleDistanceRight = 1 - (Vector3.Distance(RotateRight.transform.position, reticlePointer.PointerTransform.forward * reticlePointer.maxReticleDistance) - 1.75f);
            reticleDistanceLeft = Mathf.Clamp(reticleDistanceLeft, 0, 0.25f) * 4;
            reticleDistanceRight = Mathf.Clamp(reticleDistanceRight, 0, 0.25f) * 4;

            if (cameraControl.intrusivePanels) // Activate "intrusive" panels
            {
                Material[] leftMats = RotateLeft.GetComponent<Renderer>().materials;
                leftMats[0] = leftOpaquePanel;
                RotateLeft.GetComponent<Renderer>().materials = leftMats;
                RotateLeft.GetComponent<MeshRenderer>().material.color = new Color(RotateLeftColor.r, RotateLeftColor.g, RotateLeftColor.b, 0);

                Material[] rightMats = RotateRight.GetComponent<Renderer>().materials;
                rightMats[0] = rightOpaquePanel;
                RotateRight.GetComponent<Renderer>().materials = rightMats;
                RotateRight.GetComponent<MeshRenderer>().material.color = new Color(RotateRightColor.r, RotateRightColor.g, RotateRightColor.b, 0);
            }
            else
            {
                Material[] leftMats = RotateLeft.GetComponent<Renderer>().materials;
                leftMats[0] = leftPanel;
                RotateLeft.GetComponent<Renderer>().materials = leftMats;
                RotateLeft.GetComponent<MeshRenderer>().material.color = new Color(RotateLeftColor.r, RotateLeftColor.g, RotateLeftColor.b, 0);

                Material[] rightMats = RotateRight.GetComponent<Renderer>().materials;
                rightMats[0] = rightPanel;
                RotateRight.GetComponent<Renderer>().materials = rightMats;
                RotateRight.GetComponent<MeshRenderer>().material.color = new Color(RotateRightColor.r, RotateRightColor.g, RotateRightColor.b, 0);
            }

            if (GetComponent<CameraControl>().gradientsOn)
            {
                // Left side
                float LeftMaxOpacity = Mathf.Clamp(RotateLeft.GetComponent<PanelBehavior>().rotationSpeed, 3, 5) - 3;
                if (!greensOff)
                    UnintrusiveLeft.GetComponent<MeshRenderer>().material.color = new Color(UnintrusiveLeftColor.r, UnintrusiveLeftColor.g, UnintrusiveLeftColor.b, RotateLeft.GetComponent<PanelBehavior>().rotationSpeed * 0.2f);
                LeftMax.GetComponent<MeshRenderer>().material.color = new Color(LeftMaxColor.r, LeftMaxColor.g, LeftMaxColor.b, LeftMaxOpacity);

                // Right side
                float RightMaxOpacity = Mathf.Clamp(RotateRight.GetComponent<PanelBehavior>().rotationSpeed, 3, 5) - 3;
                if (!greensOff)
                    UnintrusiveRight.GetComponent<MeshRenderer>().material.color = new Color(UnintrusiveRightColor.r, UnintrusiveRightColor.g, UnintrusiveRightColor.b, RotateRight.GetComponent<PanelBehavior>().rotationSpeed * 0.2f);
                RightMax.GetComponent<MeshRenderer>().material.color = new Color(RightMaxColor.r, RightMaxColor.g, RightMaxColor.b, RightMaxOpacity);
            }
            if (visiblePanels)
            {
                RotateLeft.GetComponent<MeshRenderer>().material.color = new Color(RotateLeftColor.r, RotateLeftColor.g, RotateLeftColor.b, reticleDistanceLeft);

                RotateRight.GetComponent<MeshRenderer>().material.color = new Color(RotateRightColor.r, RotateRightColor.g, RotateRightColor.b, reticleDistanceRight);
            }
        }
        else if (solution == Solution.RotationMapping)
        {
            if (prevOrigin != orientation)
            {
                origin.transform.eulerAngles = new Vector3(origin.transform.eulerAngles.x, orientation, origin.transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, orientation + Vector3.SignedAngle(transform.forward, origin.transform.forward, transform.up), transform.eulerAngles.z);
                forward = origin.transform.forward;
            }

            //Debug.Log("Head rotation: " + GvrVRHelpers.GetHeadRotation().y);
            Vector3 reticleDirection = Vector3.Normalize(transform.position + reticlePointer.MaxPointerEndPoint);
            float orientationAngle = Vector3.SignedAngle(forward, transform.forward, transform.up);
            float rotationDirection = Vector3.SignedAngle(prevOrientation, reticleDirection, transform.up);
            //Debug.Log("Orientation angle: " + orientationAngle);
            //Debug.Log("Angle: " + angle);
            //Debug.Log(transform.eulerAngles.y);

            if (orientationAngle > -rotationMappingAngle && rotationDirection < 0)
            {
                if (GvrVRHelpers.GetHeadRotation().y * 360 < rotationMappingAngle)
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, orientation + GvrVRHelpers.GetHeadRotation().y * 360, transform.eulerAngles.z);
            }
            else if (orientationAngle < rotationMappingAngle && rotationDirection > 0)
            {
                if (GvrVRHelpers.GetHeadRotation().y * 360 > -rotationMappingAngle)
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, orientation + GvrVRHelpers.GetHeadRotation().y * 360, transform.eulerAngles.z);
            }

            prevOrientation = reticleDirection;
            prevOrigin = orientation;
        }
    }
}
