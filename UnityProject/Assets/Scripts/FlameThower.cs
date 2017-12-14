using UnityEngine;
using System.Collections;

public class FlameThower : Holdable {

	//public GameObject GunRigidBody;
	public float Ammo = 60; // seconds
	public int DamagePerFrame = 5;
	public GameObject LightFire;
	public GameObject Flames;
	public bool FlameOn = false;
	public AudioClip FlameSound;
	public GameObject TriggerCollisionObject;

	override public void Start () {
		Velocity = Vector3.zero;
		Flames.SetActive(false);
		LightFire.GetComponent<Light>().enabled = false;
		Audio = GetComponent<AudioSource>();
		Audio.clip = FlameSound;
	}

	override public void FixedUpdate()
	{
		if(FlameOn)
		{
			// does the gun have any ammo?
			if(Ammo > 0)
			{
				Ammo-= Time.deltaTime;
				if(!Audio.isPlaying)
				{
					Audio.Play();
				}
				LightFire.GetComponent<Light>().enabled = true;
				Flames.SetActive(true);
				//Debug.Log("Flame On !");
				if(TriggerCollisionObject != null)
				{
					if(TriggerCollisionObject.transform.root.tag == "Baddie")
					{
						//Debug.Log("hit baddie");
						TriggerCollisionObject.transform.root.GetComponent<Baddie>().SendMessage("TakeDamage", DamagePerFrame);
					}
				}
			}
			else
			{
				Audio.Pause();
				FlameOn = false;
				LightFire.GetComponent<Light>().enabled = false;
				Flames.SetActive(false);
			}
		}
		else
		{
			Audio.Pause();
			LightFire.GetComponent<Light>().enabled = false;
			Flames.SetActive(false);
		}

		if (InHandLeft && InHandRight)
		{
			if(LeftHandModel != null) LeftHandModel.SetActive(true);
			if(RightHandModel != null) RightHandModel.SetActive(true);
			this.GetComponent<Rigidbody>().isKinematic = true;
			//GunRigidBody.GetComponent<Rigidbody>().isKinematic = true;
			//GunRigidBody.transform.localPosition = Vector3.zero;
			//GunRigidBody.transform.localRotation = Quaternion.identity;
			this.gameObject.layer = 9; // change layer to InHand for physics collisions
			// slave to controller
			transform.position = HandControllerLeft.transform.position; 

			//transform.rotation = HandControllerLeft.transform.rotation;
			Vector3 targetDir = HandControllerLeft.transform.position - HandControllerRight.transform.position;
			transform.rotation = Quaternion.LookRotation(targetDir);

			HandControllerRight.GetComponent<RobustController>().HoldableObject = this.gameObject;
			HandControllerLeft.GetComponent<RobustController>().HoldableObject = this.gameObject;
		}

		if(!InHandLeft && !InHandRight)
		{
			this.gameObject.layer = 0; // change layer to Default for physics collisions
			if(LeftHandModel != null) LeftHandModel.SetActive(false);
			if(RightHandModel != null) RightHandModel.SetActive(false);
			if(Dropped)
			{
				this.GetComponent<Rigidbody>().isKinematic = false;
				//GunRigidBody.GetComponent<Rigidbody>().isKinematic = false;
				if(UseMomentum)
				{
					transform.root.GetComponent<Rigidbody>().velocity = Velocity;
					transform.root.GetComponent<Rigidbody>().angularVelocity = AngularVelocity;
				}
				Dropped = false;	
			}
		}

		if(InHandLeft && !InHandRight)
		{
			if(LeftHandModel != null) LeftHandModel.SetActive(true);
			if(RightHandModel != null) RightHandModel.SetActive(false);
			this.GetComponent<Rigidbody>().isKinematic = true;
			//GunRigidBody.GetComponent<Rigidbody>().isKinematic = false;
			this.gameObject.layer = 9; // change layer to InHand for physics collisions
			// slave to controller
			transform.position = HandControllerLeft.transform.position; 
			transform.rotation = HandControllerLeft.transform.rotation;
			HandControllerLeft.GetComponent<RobustController>().HoldableObject = this.gameObject;
		}

		if(InHandRight && !InHandLeft)
		{
			if(LeftHandModel != null) LeftHandModel.SetActive(false);
			if(RightHandModel != null) RightHandModel.SetActive(true);
			this.GetComponent<Rigidbody>().isKinematic = true;
			//GunRigidBody.GetComponent<Rigidbody>().isKinematic = false;
			this.gameObject.layer = 9; // change layer to InHand for physics collisions
			// slave to controller
			transform.position = HandControllerRight.transform.position; 
			transform.rotation = HandControllerRight.transform.rotation;
			HandControllerRight.GetComponent<RobustController>().HoldableObject = this.gameObject;
		}

		Velocity = (transform.position - PreviousPosition) * (1f/Time.deltaTime);
		AngularVelocity = ((transform.rotation * Vector3.forward) - PreviousRotation) * (1f/Time.deltaTime);
		PreviousPosition = transform.position;
		PreviousRotation = (transform.rotation * Vector3.forward);

	}


	override public void PickupHoldable(string HandString)
	{
		// drop other objects
		if(HandControllerLeft.GetComponent<RobustController>().HoldingObject == true)
		{
			HandControllerLeft.GetComponent<RobustController>().HoldableObject.GetComponent<Holdable>().InHandLeft = false;
			HandControllerLeft.GetComponent<RobustController>().HoldableObject.GetComponent<Holdable>().Dropped = true;
			HandControllerLeft.GetComponent<RobustController>().HoldableObject.GetComponent<Holdable>().SendMessage("DropHoldable" , "left");
			HandControllerLeft.GetComponent<RobustController>().HoldableObject = null;
			HandControllerLeft.GetComponent<RobustController>().HoldingObject = false;
		}
		if(HandControllerRight.GetComponent<RobustController>().HoldingObject == true)
		{
			HandControllerRight.GetComponent<RobustController>().HoldableObject.GetComponent<Holdable>().InHandRight = false;
			HandControllerRight.GetComponent<RobustController>().HoldableObject.GetComponent<Holdable>().Dropped = true;
			HandControllerRight.GetComponent<RobustController>().HoldableObject.GetComponent<Holdable>().SendMessage("DropHoldable" , "right");
			HandControllerRight.GetComponent<RobustController>().HoldableObject = null;
			HandControllerRight.GetComponent<RobustController>().HoldingObject = false;
		}

		// pick up this one
		InHandLeft = true;
		Dropped = false;
		HandControllerLeft.GetComponent<RobustController>().HoldableObject = this.gameObject;
		HandControllerLeft.GetComponent<RobustController>().HoldingObject = true;
		InHandRight = true;
		Dropped = false;
		HandControllerRight.GetComponent<RobustController>().HoldableObject = this.gameObject;
		HandControllerRight.GetComponent<RobustController>().HoldingObject = true;
	}

	override public void DropHoldable(string HandString)
	{
		// drop
		InHandLeft = false;
		Dropped = true;
		HandControllerLeft.GetComponent<RobustController>().HoldableObject = null;
		HandControllerLeft.GetComponent<RobustController>().HoldingObject = false;
		InHandRight = false;
		Dropped = true;
		HandControllerRight.GetComponent<RobustController>().HoldableObject = null;
		HandControllerRight.GetComponent<RobustController>().HoldingObject = false;
	}

	override public void TouchPadPressedDown(string HandString)
	{
		if (ButtonBounceTime > 0.1f )
		{
			ButtonBounceTime = 0;
			//Debug.Log("TouchPadPressedDown");
			if(InHandLeft || InHandRight)
			{
				DropHoldable(HandString);
				return;
			}
			if(!InHandLeft || !InHandRight)
			{
				PickupHoldable(HandString);
				return;
			}
		}
		else
		{
			ButtonBounceTime = 0;
			Debug.Log("Button Bounce");
		}
	}


	void OnTriggerEnter (Collider other)
	{
		TriggerCollisionObject = other.gameObject;
	}

	void OnTriggerExit (Collider other)
	{
		TriggerCollisionObject = null;
	}

	override public void TriggerPressedDown(string HandString)
	{
		FlameOn = true;
	}
	override public void TriggerPressedUp(string HandString)
	{
		FlameOn = false;
	}


}
