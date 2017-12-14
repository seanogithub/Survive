using UnityEngine;
using System.Collections;

public class MenuController : Holdable {

	public GameObject LaserPointer;
	public GameObject LaserDot;
	public float ButtonScaleAmount = 1.1f;
	public Collider PreviousCollider;

	public AudioClip SoundButtonHover;
	public AudioClip SoundButtonClick;

	override public void FixedUpdate()
	{
		if(InHandLeft)
		{
			if(LeftHandModel != null) LeftHandModel.SetActive(true);
			if(RightHandModel != null) RightHandModel.SetActive(false);
			this.GetComponent<Rigidbody>().isKinematic = true;
			this.gameObject.layer = 9; // change layer to InHand for physics collisions
			// slave to controller
			transform.position = HandControllerLeft.transform.position; 
			transform.rotation = HandControllerLeft.transform.rotation;
			HandControllerLeft.GetComponent<RobustController>().HoldableObject = this.gameObject;
		}
		if(InHandRight)
		{
			if(LeftHandModel != null) LeftHandModel.SetActive(false);
			if(RightHandModel != null) RightHandModel.SetActive(true);
			this.GetComponent<Rigidbody>().isKinematic = true;
			this.gameObject.layer = 9; // change layer to InHand for physics collisions
			// slave to controller
			transform.position = HandControllerRight.transform.position; 
			transform.rotation = HandControllerRight.transform.rotation;
			HandControllerRight.GetComponent<RobustController>().HoldableObject = this.gameObject;
		}

		if(!InHandLeft && !InHandRight)
		{
			this.gameObject.layer = 0; // change layer to Default for physics collisions
			if(LeftHandModel != null) LeftHandModel.SetActive(false);
			if(RightHandModel != null) RightHandModel.SetActive(false);
			if(Dropped)
			{
				this.GetComponent<Rigidbody>().isKinematic = false;
				if(UseMomentum)
				{
					transform.root.GetComponent<Rigidbody>().velocity = Velocity;
					transform.root.GetComponent<Rigidbody>().angularVelocity = AngularVelocity;
				}
				Dropped = false;	
			}
		}

		Velocity = (transform.position - PreviousPosition) * (1f/Time.deltaTime);
		AngularVelocity = ((transform.rotation * Vector3.forward) - PreviousRotation) * (1f/Time.deltaTime);
		PreviousPosition = transform.position;
		PreviousRotation = (transform.rotation * Vector3.forward);

		// cast ray to intersect objects
		RaycastHit hit;
		Vector3 dir = transform.rotation * Vector3.forward;
		Ray ClickRay = new Ray(transform.position, dir);
		if(Physics.Raycast(ClickRay, out hit))
		{
			//Debug.Log (hit.collider.name + " - " + hit.point);
			Vector3 targetDir = (hit.point - transform.position);
			//Debug.Log (targetDir.magnitude);
			Vector3 newScale = new Vector3( 1, targetDir.magnitude, 1);
			LaserPointer.transform.localScale = newScale;
			LaserDot.SetActive(true);
			LaserDot.transform.localScale = newScale;

			// change size of the button
			if(hit.collider != PreviousCollider && PreviousCollider != null)
			{
				PreviousCollider.transform.localScale = Vector3.one;
				PreviousCollider = null;
			}

			if(hit.collider.tag == "UIButton")
			{
				//Debug.Log(hit.collider);
				if(PreviousCollider != hit.collider)
				{
					Audio.PlayOneShot(SoundButtonHover);
				}
				hit.collider.transform.localScale = new Vector3 (ButtonScaleAmount, ButtonScaleAmount, ButtonScaleAmount);
				PreviousCollider = hit.collider;
			}
		}
		else
		{
			LaserPointer.transform.localScale = new Vector3( 1, 30, 1);
			LaserDot.SetActive(false);
		}


	}

	/*
	override public void Update () 
	{
		// draw ray for debugging
		//Vector3 dir = transform.rotation * Vector3.forward;
		//Debug.DrawRay(transform.position, dir, Color.red);
	}
	*/

	// can't drop these
	override public void TouchPadPressedDown(string HandString)
	{
		Debug.Log("TouchPadPressedDown");
	}


	override public void TriggerPressedDown(string HandString)
	{
		if(InHandRight || InHandLeft)
		{
			//print("Click Ray");
			RaycastHit hit;
			Vector3 dir = transform.rotation * Vector3.forward;
			Ray ClickRay = new Ray(transform.position, dir);
			if(Physics.Raycast(ClickRay, out hit))
			{
				//Debug.Log("hit object");
				//Debug.Log(hit.collider);
				if(hit.collider.GetComponent<MenuButton>() != null)
				{
					Audio.PlayOneShot(SoundButtonClick);
					hit.collider.GetComponent<MenuButton>().SendMessage("Clicked");
				}
				//hit.collider.GetComponent<TestBaddie>().SendMessage("TakeDamage", 25);
				//hit.collider.transform.root.GetComponent<Baddie>().SendMessage("TakeDamage", DamagePerShot);
			}
		}
	}
}
