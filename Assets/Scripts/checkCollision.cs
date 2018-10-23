using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkCollision : MonoBehaviour {

	void OnTriggerEnter (Collider col)
	    {
	        if(col.gameObject.tag == "text")
	        {
	            Debug.Log("game over");
	        }
	}
}
