﻿using UnityEngine;
using System.Collections;

public class MiniGun : Holdable {

	public Animator WeaponAnimationController;
	public GameObject FiringPostion;
	public GameObject MuzzleFlash;
	public GameObject BulletHit;
	public int Ammo = 300;
	public int DamagePerShot = 25;
	public bool Automatic = false;
	public bool FireGunStart = false;
	public bool FireGunDone = true;
	public bool TriggerDown = false;
	public float FireTime = 0.1f;
	private float FireTimeLeft = 0.1f;

	public AudioSource AudioFireSound;

	public AudioClip[] FireSounds;
	public AudioClip[] FireEmptySounds;
	public AudioClip WindUpSound;
	public AudioClip WindDownSound;
	public AudioClip WindSpinningSound;

	override public void Awake()
	{
		GameObject CameraRig = GameObject.Find("[CameraRig]");
		HandControllerLeft = CameraRig.transform.FindChild("Controller (left)").gameObject;
		HandControllerRight = CameraRig.transform.FindChild("Controller (right)").gameObject;
		Player = GameObject.Find("Player_Prefab");
		Audio = gameObject.AddComponent<AudioSource>();
		Audio.playOnAwake = false;
		Audio.spatialBlend = 1.0f;
		Audio.maxDistance = 50f;
		Audio.volume = 0.30f;

		AudioFireSound = gameObject.AddComponent<AudioSource>();
		AudioFireSound.playOnAwake = false;
		AudioFireSound.spatialBlend = 1.0f;
		AudioFireSound.maxDistance = 50f;
	}

	// Use this for initialization
	override public void Start () {
		Velocity = Vector3.zero;
		//Audio = GetComponent<AudioSource>();
	}

	override public void Update () 
	{
		ButtonBounceTime +=Time.deltaTime;

		if(!FireGunDone)
		{
			FireTimeLeft -= Time.deltaTime;
			if(FireTimeLeft < 0)
			{
				FireGunComplete();
			}
		}
			
		// draw ray for debugging
		//Vector3 dir = transform.rotation * Vector3.forward;
		//Debug.DrawRay(transform.position, dir, Color.red);
	}

	override public void TriggerPressedDown(string HandString)
	{
		if(InHandLeft || HandString == "Left")
		{
			TriggerDown = true;
			if(FireGunDone == true)
				FireGunStart = true;
		}
		if(InHandRight || HandString == "Right")
		{
			TriggerDown = true;
			if(FireGunDone == true)
				FireGunStart = true;
		}
	}

	override public void TriggerPressedUp(string HandString)
	{
		if(InHandLeft || HandString == "Left")
		{
			TriggerDown = false;
		}
		if(InHandRight || HandString == "Right")
		{
			TriggerDown = false;
		}
	}

	public void FireGun()
	{
		if(InHandLeft || InHandRight)
		{
			// does the gun have any ammo?
			if(WeaponAnimationController.GetInteger("Ammo") > 0)
			{
				FireTimeLeft = FireTime;
				FireGunDone = false;
				Player.GetComponent<Player>().StatsShotsFired += 1;
				AudioFireSound.PlayOneShot(FireSounds[(Random.Range(0,(FireSounds.Length-1)))]);
				WeaponAnimationController.SetBool("Fire",true);
				//Debug.Log("Fire Gun");
				RaycastHit hit;
				Vector3 dir = FiringPostion.transform.rotation * Vector3.forward;
				Ray GunRay = new Ray(FiringPostion.transform.position, dir);
				if(Physics.Raycast(GunRay, out hit))
				{
					SpawnBulletHit(hit.point);
					if(hit.collider.transform.root.tag == "Baddie")
					{
						//Debug.Log("hit baddie");
						hit.collider.transform.root.GetComponent<Baddie>().SendMessage("TakeDamage", DamagePerShot);
						Player.GetComponent<Player>().StatsBaddieHits += 1;
					}
				}
			}
			else
			{
				AudioFireSound.PlayOneShot(FireEmptySounds[(Random.Range(0,(FireEmptySounds.Length-1)))]);
			}
		}
		FireGunStart = false;
	}


	public void FireGunComplete()
	{
		if(InHandLeft || InHandRight)
		{
			FireGunDone = true;
			int Ammo = WeaponAnimationController.GetInteger("Ammo") - 1;
			WeaponAnimationController.SetInteger("Ammo", Ammo);
			WeaponAnimationController.SetBool("Fire",false);
			//Debug.Log(Ammo);
			if(Automatic && TriggerDown)
			{
				FireGunStart = true;
			}
		}
	}

	public void SpawnBulletHit(Vector3 HitPosition)
	{
		if(BulletHit != null)
		{
			GameObject Hit = Instantiate(BulletHit);
			Hit.transform.position = HitPosition;
		}
	}

	override public void FixedUpdate()
	{
		MuzzleFlash.SetActive(false);
		if (FireGunStart)
		{
			FireGun();
			if(MuzzleFlash != null && WeaponAnimationController.GetInteger("Ammo") > 0)
			{
				MuzzleFlash.SetActive(true);
			}
		}

		if (InHandLeft || InHandRight)
		{
			if(LeftHandModel != null) LeftHandModel.SetActive(true);
			if(RightHandModel != null) RightHandModel.SetActive(true);
			this.GetComponent<Rigidbody>().isKinematic = true;
			this.gameObject.layer = 9; // change layer to InHand for physics collisions
			// slave to controller
			transform.position = HandControllerLeft.transform.position; 

			//transform.rotation = HandControllerLeft.transform.rotation;
			Vector3 targetDir = HandControllerLeft.transform.position - HandControllerRight.transform.position;
			transform.rotation = Quaternion.LookRotation(targetDir);

			HandControllerRight.GetComponent<RobustController>().HoldableObject = this.gameObject;
			HandControllerLeft.GetComponent<RobustController>().HoldableObject = this.gameObject;

			if(!Audio.isPlaying)
			{
				Audio.PlayOneShot(WindSpinningSound);
			}
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
		WeaponAnimationController.SetBool("Holding",true);
		Audio.PlayOneShot(WindUpSound);
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
		WeaponAnimationController.SetBool("Holding",false);
		Audio.PlayOneShot(WindDownSound);
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

}
