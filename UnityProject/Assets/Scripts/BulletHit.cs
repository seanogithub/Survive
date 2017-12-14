using UnityEngine;
using System.Collections;

public class BulletHit : MonoBehaviour {

	public float LifeTime = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		LifeTime -= Time.deltaTime;

		if(LifeTime < 0)
		{
			Destroy(this.gameObject);
		}
	}
}
