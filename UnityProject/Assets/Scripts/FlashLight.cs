using UnityEngine;
using System.Collections;

public class FlashLight : Holdable {


	public GameObject Light;
	public bool TurnedOn = true;
	public GameObject LensModel;
	public float Power = 60;

	// Update is called once per frame
	override public void Update () 
	{
		ButtonBounceTime +=Time.deltaTime;
		// turn light on/off
		if(TurnedOn && Power > 0)
		{
			Power-= Time.deltaTime;
			Light.GetComponent<Light>().enabled = true;
			LensModel.SetActive(true);
		}
		else
		{
			Light.GetComponent<Light>().enabled = false;
			LensModel.SetActive(false);
		}

		// draw ray for debugging
		Vector3 dir = transform.rotation * Vector3.forward;
		Debug.DrawRay(transform.position, dir, Color.red);

	}

	override public void TriggerPressedDown(string HandString)
	{
		if(InHandLeft || InHandRight)
		{
			TurnedOn = !TurnedOn;
		}
	}

}
