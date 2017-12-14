using UnityEngine;
using System.Collections;

public class Gun : Holdable {

	public Animator WeaponAnimationController;
	public GameObject MuzzleFlash;
	public GameObject BulletHit;
	public int DamagePerShot = 25;
	public bool Automatic = false;
	public bool FireGunStart = false;
	public bool FireGunDone = true;
	public bool TriggerDown = false;

	public AudioClip[] FireSounds;
	public AudioClip[] FireEmptySounds;

	// Use this for initialization
	override public void Start () {
		Velocity = Vector3.zero;
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
	}

	public void FireGun()
	{
		if(InHandLeft || InHandRight)
		{
			// does the gun have any ammo?
			if(WeaponAnimationController.GetInteger("Ammo") > 0)
			{
				FireGunDone = false;
				Player.GetComponent<Player>().StatsShotsFired += 1;
				Audio.PlayOneShot(FireSounds[(Random.Range(0,(FireSounds.Length-1)))]);
				WeaponAnimationController.SetBool("Fire",true);
				//Debug.Log("Fire Gun");
				RaycastHit hit;
				Vector3 dir = transform.rotation * Vector3.down;
				Ray GunRay = new Ray(transform.position, dir);
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
				Audio.PlayOneShot(FireEmptySounds[(Random.Range(0,(FireEmptySounds.Length-1)))]);
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
}
