using UnityEngine;
using System.Collections;

public class LightSword : Holdable {

	public Animator WeaponAnimationController;
	public int DamagePerHit = 100;
	public GameObject TriggerCollisionObject;
	public bool TurnedOn = false;
	public float Power = 60;
	public GameObject SparkFX;
	public AudioSource AudioWhoosh;

	public AudioClip[] ImpactSounds;
	public AudioClip[] WhooshSounds;
	public AudioClip[] OpenSounds;
	public AudioClip[] CloseSounds;
	public AudioClip IdleSound;

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
		Audio = GetComponent<AudioSource>();
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
				//if(AudioWhoosh.isPlaying == false) AudioWhoosh.PlayOneShot(WhooshSounds[(Random.Range(0,(WhooshSounds.Length-1)))]);
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
				//if(AudioWhoosh.isPlaying == false) AudioWhoosh.PlayOneShot(WhooshSounds[(Random.Range(0,(WhooshSounds.Length-1)))]);
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

		if(!Audio.isPlaying && TurnedOn)
		{
			Audio.PlayOneShot(IdleSound);
		}

		Velocity = (transform.position - PreviousPosition) * (1f/Time.deltaTime);
		AngularVelocity = ((transform.rotation * Vector3.forward) - PreviousRotation) * (1f/Time.deltaTime);
		PreviousPosition = transform.position;
		PreviousRotation = (transform.rotation * Vector3.forward);
	}

	override public void Update () 
	{
		ButtonBounceTime +=Time.deltaTime;
		// turn light on/off
		if(TurnedOn && Power > 0)
		{
			Power-= Time.deltaTime;
			WeaponAnimationController.SetBool("On",true);
		}
		else
		{
			WeaponAnimationController.SetBool("On",false);
		}

		// draw ray for debugging
		Vector3 dir = transform.rotation * Vector3.forward;
		Debug.DrawRay(transform.position, dir, Color.red);

	}


	void OnTriggerEnter (Collider other)
	{
		TriggerCollisionObject = other.gameObject;

		if(InHandLeft || InHandRight)
		{
			if(TurnedOn && Power > 0)
			{
				//print("Hit with melee weapon !");
				if(TriggerCollisionObject != null) // just in case the object is null?
				{
					Audio.PlayOneShot(ImpactSounds[(Random.Range(0,(ImpactSounds.Length-1)))]);

					RaycastHit hit;
					Vector3 dir = transform.rotation * Vector3.forward;
					Ray SwordRay = new Ray(transform.position, dir);
					if(Physics.Raycast(SwordRay, out hit, 0.7f, 9, QueryTriggerInteraction.Ignore))
					{
						SpawnSparkFX(hit.point);
					}

					if(TriggerCollisionObject.transform.root.tag == "Baddie")
					{
						//Debug.Log("hit baddie with melee weapon");
						Player.GetComponent<Player>().StatsMeleeHits += 1;
						TriggerCollisionObject.transform.root.GetComponent<Baddie>().SendMessage("TakeDamage", DamagePerHit);
					}
				}
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		TriggerCollisionObject = null;
	}

	override public void TriggerPressedDown(string HandString)
	{
		if(InHandLeft || InHandRight)
		{
			TurnedOn = !TurnedOn;
		}

		// play open and close sounds
		if(Power > 0)
		{
			if(TurnedOn)
			{
				Audio.PlayOneShot(OpenSounds[(Random.Range(0,(OpenSounds.Length-1)))]);
			}
			else
			{
				Audio.PlayOneShot(CloseSounds[(Random.Range(0,(CloseSounds.Length-1)))]);
			}
		}
	}

	public void SpawnSparkFX(Vector3 HitPosition)
	{
		if(SparkFX != null)
		{
			GameObject Hit = Instantiate(SparkFX);
			Hit.transform.position = HitPosition;
		}
	}
}
