using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitivePlacer : MonoBehaviour
{
	public GameObject playerCamera;
	public float placeOffset = 2.0f;//distance from player position that the primitive will be placed.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
        	//place cube
        	GameObject new_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        	new_cube.transform.position = gameObject.transform.position  + playerCamera.transform.forward * placeOffset;
       
        	new_cube.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);



        }

        if (Input.GetKeyDown(KeyCode.O))
        {
        	//place sphere
        	GameObject new_sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        	new_sphere.transform.position = gameObject.transform.position  + playerCamera.transform.forward * placeOffset;
       
        	new_sphere.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
        	//place plane
        	GameObject new_plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        	new_plane.transform.position = gameObject.transform.position  + playerCamera.transform.forward * placeOffset;
       
        	new_plane.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }
}
