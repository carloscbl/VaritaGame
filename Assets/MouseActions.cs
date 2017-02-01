using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

public class MouseActions : MonoBehaviour {

	private TerrainSystem ts;
	// Use this for initialization
	void Start () {
		ts = TerrainSystem.TS;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0))
		{ 
			RaycastHit hit;
			if (Camera.main != null)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, 35f))
				{
                    //Debug.LogWarning(hit.transform.gameObject.name);
                    
                        Chunk tempChunk = ts.findChunk(hit.transform.gameObject.name);
                    if (tempChunk !=null)
                    {
                        tempChunk.updateMesh(hit.point);
                    }
				}
			}
		}
	}
}
