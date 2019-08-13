using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelBehavior : MonoBehaviour, IGvrPointerHoverHandler {

    GameObject player;
    GvrReticlePointer reticlePointer;
    CameraControl cameraControl;
    public float rotationSpeed;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        cameraControl = player.GetComponent<CameraControl>();
        reticlePointer = GameObject.Find("GvrReticlePointer").GetComponent<GvrReticlePointer>();
    }

    public void OnGvrPointerHover(PointerEventData data)
    {
        if (cameraControl.solution == CameraControl.Solution.Panels)
        {
            RaycastResult ray = reticlePointer.CurrentRaycastResult;
            Vector3 panelNoY = new Vector3(transform.position.x, 0, transform.position.z); // Disregard y-axis when calculating distance between ray and panel center
            Vector3 rayNoY = new Vector3(ray.worldPosition.x, 0, ray.worldPosition.z);

            if (cameraControl.scalingRotationSpeed)
                rotationSpeed = Vector3.Distance(panelNoY + transform.right * transform.lossyScale.x * cameraControl.rotationSpeed, rayNoY);
            else
                rotationSpeed = cameraControl.rotationSpeed;

            if (this.name == "RotateRight") // Map ray hit to 0.0-5.0 depending on how far left/right it is on the panel and use this to scale the rotation speed
                player.transform.Rotate(Vector3.up * Time.deltaTime * 50 * rotationSpeed, Space.World);
            else
                player.transform.Rotate(Vector3.up * Time.deltaTime * -50 * rotationSpeed, Space.World);
        }
        else
        {
        }
    }

	// Update is called once per frame
	void Update () {
    }
}
