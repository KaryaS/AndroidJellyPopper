using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Jiggle : MonoBehaviour {

    float fTimer = 0;
    bool bRotateLeft = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(bRotateLeft)
        {
            gameObject.transform.Rotate(new Vector3(0, 0, -3));
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 3));
        }
        if(gameObject.transform.rotation.z >= 0.5)
        {
            bRotateLeft = true;
        }
        else if (gameObject.transform.rotation.z <= -0.5)
        {
            bRotateLeft = false;
        }
    }
}
