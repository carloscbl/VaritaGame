using UnityEngine;
using System.Collections;

public class ArmProceduralMovement : MonoBehaviour {

    //Public Vars
    public Camera camera;
    public float speed;
    private bool enable = false;

    private Vector3 mousePosition;
    private Vector3 mousePosition2;
    private Vector3 direction;
    private float distanceFromObject;
    // Use this for initialization
    void Start()
    {
        if (this.gameObject.GetComponentInParent<Character>().mainPlayer)
        {
            if (camera = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.Find("Main Camera").gameObject.GetComponent<Camera>())
            {
                enable = true;
            }
        }        
    }
	
	// Update is called once per frame
	void Update () {

        if (enable)
        {
            mousePosition2 = Input.mousePosition;
            //Grab the current mouse position on the screen
            mousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - camera.transform.position.z));
            //Debug.Log("hola");
            //Rotates toward the mouse
            float z = Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg;
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.x, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg);

            //Judge the distance from the object and the mouse
            distanceFromObject = (Input.mousePosition - camera.WorldToScreenPoint(transform.position)).magnitude;
        }
    }
}
