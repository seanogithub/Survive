using UnityEngine;
using System.Collections;

public class MeleeWeapon : Holdable {

	public int DamagePerHit = 50;
	public GameObject TriggerCollisionObject;
	public int BaddieID = 0;
	public AudioSource AudioWhoosh;

	public AudioClip[] ImpactSounds;
	public AudioClip[] WhooshSounds;

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
		AudioWhoosh = gameObject.AddComponent<AudioSource>();
		AudioWhoosh.playOnAwake = false;
		AudioWhoosh.spatialBlend = 1.0f;
		AudioWhoosh.maxDistance = 50f;
	}

	// Use this for initialization
	override public void Start () {
		Velocity = Vector3.zero;
	}


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

			// play whoosh sound
			float WhooshDist = Vector3.Distance(PreviousPosition, transform.position);
			if(WhooshDist> 0.04f)
			{
				if(AudioWhoosh.isPlaying == false) AudioWhoosh.PlayOneShot(WhooshSounds[(Random.Range(0,(WhooshSounds.Length-1)))]);
			}
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

			// play whoosh sound
			float WhooshDist = Vector3.Distance(PreviousPosition, transform.position);
			if(WhooshDist> 0.04f)
			{
				if(AudioWhoosh.isPlaying == false) AudioWhoosh.PlayOneShot(WhooshSounds[(Random.Range(0,(WhooshSounds.Length-1)))]);
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

	void OnTriggerEnter (Collider other)
	{
		TriggerCollisionObject = other.gameObject;

		if(InHandLeft || InHandRight)
		{
			//print("Hit with melee weapon !");
			if(TriggerCollisionObject != null) // just in case the object is null?
			{
				if(TriggerCollisionObject.transform.root.tag == "Baddie")
				{
					if(BaddieID != TriggerCollisionObject.transform.root.GetComponent<Baddie>().ID)
					{
						//Debug.Log("hit baddie with melee weapon");
						//Debug.Log("Enter: " + TriggerCollisionObject.name);
						BaddieID = TriggerCollisionObject.transform.root.GetComponent<Baddie>().ID;
						if(Audio.isPlaying == false) Audio.PlayOneShot(ImpactSounds[(Random.Range(0,(ImpactSounds.Length-1)))]);
						Player.GetComponent<Player>().StatsMeleeHits += 1;
						TriggerCollisionObject.transform.root.GetComponent<Baddie>().SendMessage("TakeDamage", DamagePerHit);
					}
				}
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if(other.transform.root.tag == "Baddie")
		{
			if(BaddieID == other.transform.root.GetComponent<Baddie>().ID)
			{
				BaddieID = 0;
				//Debug.Log("Exit: " + other.name);
			}
		}
		TriggerCollisionObject = null;
	}
}
