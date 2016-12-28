using UnityEngine;
using System.Collections;

public class CameraScrollMouse : MonoBehaviour {

    public Camera camera;
    private float limitDeep = 1;
    private float limitLong = 10;
    float target;
    // Use this for initialization
    void Start()
    {
        
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        target = camera.transform.localPosition.z;
    }
    
    // Update is called once per frame
    void Update () {
	    
        
        if (camera.transform.localPosition.z > -2.5f) {
            camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, camera.transform.localPosition.y, -2.6f);
            Debug.Log("Close True");
            target = -2.6f;
        }
        else if(camera.transform.localPosition.z < -12.0f)
        {
            camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, camera.transform.localPosition.y, -11.5f);
            Debug.Log("Far True");
            target = -11.5f;
        }
        else
        {
            getTarget();
            //float currentVelocity = 0;
            float z = Mathf.SmoothStep(camera.transform.localPosition.z, target, 0.1f);

            camera.transform.Translate(0, 0, z - camera.transform.localPosition.z);
        }
    }
    void getTarget()
    {
        if (Input.GetAxis("Mouse ScrollWheel") >= 0.1f || Input.GetAxis("Mouse ScrollWheel") <= -0.1f)
        {
            if (!((target + 15 * Input.GetAxis("Mouse ScrollWheel")) > -2.5f) && !((target + 15 * Input.GetAxis("Mouse ScrollWheel")) < -11.5f))
            {
                target += 15 * Input.GetAxis("Mouse ScrollWheel");
            }
        }
    }
}
