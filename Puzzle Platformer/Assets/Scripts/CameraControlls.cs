using UnityEngine;

public class CameraControlls : MonoBehaviour
{
    //Transformation of the Camera and the parent Pivot Object
    private Transform Camera;
    private Transform cameraParent;

    //Stores the rotation of the camera pivot per frame
    private Vector3 LocalRotation;
    //Distance of the camera to pivot
    private float CameraDistance = 10f;

    //Scroll and Zoom Sensitivity
    [Header("Sensitivity")]
    public float mouseSensitivity = 4f;
    public float scrollSensitivity = 3f;
    [Space]

    //Speed of the camera rotation and zooming in and out
    //The bigger the number, the longer it takes for the camera to reach its destination
    [Header("Speed/Dampening")]
    public float orbitSpeed = 10f;
    public float scrollSpeed = 6f;
    [Space]

    //The minimum and maximum range that is allowed for the camera to be scrolled out too.
    [Header("Scroll Ranges")]
    public float minScrollRange = 1.5f;
    public float maxScrollRange = 50f;
    private bool cameraDisable = false;

    void Start()
    {
        //Sets the transform of the objects
        this.Camera = this.transform;
        this.cameraParent = this.transform.parent;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            cameraDisable = !cameraDisable;

        if (!cameraDisable)
        {
            //This checks that the camera is not stationarry
            if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                //This appends the local rotation each frame instead of setting it each frame
                LocalRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                LocalRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;

                //Claps the y rotation to the rozizon and not allowing it to flip over when scrolled directly above the scean
                LocalRotation.y = Mathf.Clamp(LocalRotation.y, 0f, 90f);
            }

            //Scroll input from the Scroll Wheel
            if(Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

                /*
                 * Dampening the vaule of the camera distance to the pivot object, 
                 * allowing to scroll out faster the further the camera is from the pivot
                 * But the closer the camera is to the pivot, the faster the camera scrolls
                 */
                ScrollAmount *= (this.CameraDistance * 0.3f);

                this.CameraDistance += ScrollAmount * -1f;

                //Gives the camera the minimum and maximum range that it can scroll to
                this.CameraDistance = Mathf.Clamp(this.CameraDistance, minScrollRange, maxScrollRange);
            }
        }

        //Setting the camera orientations and zooming (The "pitch" and the "Yaw" if you will)
        Quaternion quaternion = Quaternion.Euler(LocalRotation.y, LocalRotation.x, 0);
        this.cameraParent.rotation = Quaternion.Lerp(this.cameraParent.rotation, quaternion, Time.deltaTime * orbitSpeed);

        //Checks if the position of the camera has not moved recently, so that extra calculations for that frame
        if(this.Camera.localPosition.z != this.CameraDistance * -1f)
        {
            this.Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(Camera.localPosition.z, this.CameraDistance * -1f, Time.deltaTime * scrollSpeed));
        }
    }
}
