using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour {

	//******************************
	// this script is just to get the collision object and send it to the parent of this script.
	//******************************

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other)
	{
		//GetComponentInParent<FlameThower>().BaddieOnFire = other.gameObject;
	}

	void OnTriggerExit (Collider other)
	{
		//GetComponentInParent<FlameThower>().BaddieOnFire = null;
	}
}
