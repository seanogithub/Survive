using UnityEngine;
using System.Collections;

public class RoadFlare : Holdable {

	public GameObject Light;
	public GameObject CapModel;
	public GameObject FlareFlame;
	public bool TurnedOn = false;
	public float Ammo = 60;
	public bool FlamesAlwaysUp = false;

	// Use this for initialization
	override public void Start () {
		Velocity = Vector3.zero;
		Light.GetComponent<Light>().enabled = false;
		FlareFlame.SetActive(false);
	}

	// Update is called once per frame
	override public void Update () 
	{
		ButtonBounceTime +=Time.deltaTime;
		if(TurnedOn && Ammo > 0)
		{
			Ammo-= Time.deltaTime;
			Light.GetComponent<Light>().enabled = true;
			FlareFlame.SetActive(true);
		}
		else
		{
			Light.GetComponent<Light>().enabled = false;
			FlareFlame.SetActive(false);
		}
		if (Ammo < 0 )
		{
			TurnedOn = false;
		}

		if(FlamesAlwaysUp)
		{
			FlareFlame.transform.rotation = Quaternion.identity;
		}

		// draw ray for debugging
		Vector3 dir = transform.rotation * Vector3.forward;
		Debug.DrawRay(transform.position, dir, Color.red);

	}

	override public void TriggerPressedDown(string HandString)
	{
		if(InHandLeft || InHandRight)
		{
			// dont toggle... just turn it on once and it can't be turned off.
			TurnedOn = true;
			if(CapModel != null) CapModel.SetActive(false);
		}
	}

}
