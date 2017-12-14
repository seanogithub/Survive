using UnityEngine;
using System.Collections;

public class Baddie : MonoBehaviour {


	public GameObject PlayerCamera;
	public GameObject Player;
	public GameObject[] FoodItems;
	public GameObject ClosestTarget;
	public int ID = 1;
	public int HitPoints = 100;
	public float MoveSpeed = 1.0f;
	public float TurnSpeed = 5.0f;
	public float AttackDistance = 1.5f;
	public float EatDistance = 0.75f;
	public int AttackDamage = 10;
	public Animator BaddieAnimationController;
	public string CurrentState = "Taunt";
	public string PreviousState = "Idle";

	public AudioSource Audio;
	public AudioClip[] TauntSounds;
	public AudioClip[] MoveSounds;
	public AudioClip[] AttackSounds;
	public AudioClip[] EatSounds;
	public AudioClip[] DieSounds;
	public AudioClip CurrentSound;

	public float WaitTimer = 1.0f;
	public float WaitOnInitTime = 7.0f;
	public float MoveTimer = 1.0f;
	public float DieTimer = 3.0f;

	// Use this for initialization
	void Start () 
	{
		// need to find the player and camera game objects
		Player = GameObject.FindGameObjectWithTag("Player");
		PlayerCamera = GameObject.FindGameObjectWithTag("MainCamera");
		FoodItems = GameObject.FindGameObjectsWithTag("Food");
		Audio = GetComponent<AudioSource>();
		CurrentState = "Taunt";
		PreviousState = "Idle";
		WaitTimer = Random.Range(0f, WaitOnInitTime); // WaitTimer for to start
		MoveTimer = Random.Range(2.0f, 9.0f); // random range for move timer
		RagDoll(false);
	}

	// Update is called once per frame
	void Update () {

		if(this.transform.position.y < -10)
		{
			CurrentState = "Die";
		}

		// idle timer
		if (WaitTimer > 0)
		{
			WaitTimer -= Time.deltaTime;
			CurrentState = "Idle";
			if(WaitTimer < 0)
			{
				WaitTimer = 0;
				CurrentState = "Move";
			}
		}

		FindClosestTarget();

		// get closer to some targets for eating or attacking
		float Dist = AttackDistance;
		if(ClosestTarget.tag == "Food")
		{
			Dist = EatDistance;			
		}

		// eat or attack or move?
		if(CurrentState != "Taunt" && CurrentState != "Idle" && CurrentState != "Die")
		{
			// if close to player then attack otherwise move to get closer to the player
			float targetDistance = Vector3.Distance(ClosestTarget.transform.position, transform.position);

			if(targetDistance < Dist && HitPoints > 0 )
			{
				if(ClosestTarget.tag == "Food")
				{
					CurrentState = "Eat";
				}
				else
				{
					CurrentState = "Attack";
				}
			}
			else
			{
				// randomly pick a new move state
				if(MoveTimer > 0 )
				{
					MoveTimer -= Time.deltaTime;
					if(MoveTimer < 0)
					{
						MoveTimer = Random.Range(2.0f, 9.0f);
						PickNewMoveState();
					}
				}
			}
		}

		switch (CurrentState)
		{
		case "MoveCharge":
			{
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);

				// position
				float MoveStep = MoveSpeed * 2 * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, ClosestTarget.transform.position, MoveStep);

				// rotation
				float TurnStep = TurnSpeed * Time.deltaTime;
				Vector3 targetDir = ClosestTarget.transform.position - transform.position;
				targetDir.y = 0;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, TurnStep, 0.0F);
				Debug.DrawRay(transform.position, newDir, Color.red);
				transform.rotation = Quaternion.LookRotation(newDir);
				break;
			}
		case	"MoveCircleRight":
			{
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);

				// position
				float MoveStep = MoveSpeed * Time.deltaTime;
				float TurnStep = TurnSpeed * Time.deltaTime;

				Vector3 NewTargetDir = ClosestTarget.transform.position - transform.position;
				Vector3 NewPerpDir = Vector3.Cross(NewTargetDir , Vector3.up);
				Vector3 NewTarget = NewPerpDir.normalized * (NewPerpDir.magnitude * 2);
				transform.position = Vector3.MoveTowards(transform.position, NewTarget, MoveStep * 1.414f);

				// rotation
				Vector3 targetDir = NewTarget - transform.position;
				targetDir.y = 0;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, NewTarget, TurnStep, 0.0F);
				transform.rotation = Quaternion.LookRotation(newDir);

				Debug.DrawLine(transform.position, NewTarget);
				//Debug.DrawRay(transform.position, newDir, Color.red);
				break;
			}
		case	"MoveCircleLeft":
			{
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);

				// position
				float MoveStep = MoveSpeed * Time.deltaTime;
				float TurnStep = TurnSpeed * Time.deltaTime;

				Vector3 NewTargetDir = transform.position - ClosestTarget.transform.position;
				Vector3 NewPerpDir = Vector3.Cross(NewTargetDir , Vector3.up);
				Vector3 NewTarget = NewPerpDir.normalized * (NewPerpDir.magnitude * 2);
				transform.position = Vector3.MoveTowards(transform.position, NewTarget, MoveStep * 1.414f);

				// rotation
				Vector3 targetDir =  transform.position - NewTarget;
				targetDir.y = 0;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, NewTarget, TurnStep, 0.0F);
				transform.rotation = Quaternion.LookRotation(newDir);

				Debug.DrawLine(transform.position, NewTarget);
				//Debug.DrawRay(transform.position, newDir, Color.red);
				break;
			}
		case	"Move":
			{
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);

				// position
				float MoveStep = MoveSpeed * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, ClosestTarget.transform.position, MoveStep);
				//transform.position = new Vector3 (transform.position.x, 0f, transform.position.z);

				// rotation
				float TurnStep = TurnSpeed * Time.deltaTime;
				Vector3 targetDir = ClosestTarget.transform.position - transform.position;
				targetDir.y = 0;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, TurnStep, 0.0F);
				Debug.DrawRay(transform.position, newDir, Color.red);
				transform.rotation = Quaternion.LookRotation(newDir);
				break;

			}
		case "Attack":
			{
				// rotation
				float TurnStep = TurnSpeed * Time.deltaTime;
				Vector3 targetDir = ClosestTarget.transform.position - transform.position;
				targetDir.y = 0;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, TurnStep, 0.0F);
				Debug.DrawRay(transform.position, newDir, Color.red);
				transform.rotation = Quaternion.LookRotation(newDir);
				break;
			}
		case "Die":
			{
				DieTimer -= Time.deltaTime;
				if(DieTimer < 0f)
				{
					Die();
				}
				if(DieTimer < 1.0f)
				{
					BaddieAnimationController.enabled = false;
					Collider[] ColiderArray = this.GetComponentsInChildren<Collider>();
					foreach (Collider RB in ColiderArray)
					{
						RB.enabled = false;
					}
				}

				break;
			}
		}


		// die if hit points are less than 0
		if (HitPoints <= 0)
		{
			CurrentState = "Die";
		}

		if (PreviousState != CurrentState)
		{
			SwitchStates();
		}
	}

	public void PickNewMoveState()
	{
		CurrentState = "Move";
		int NewMoveStateInt = Mathf.RoundToInt(Random.Range (0.0f,3.0f));
		switch (NewMoveStateInt)
		{
		case 0:
			CurrentState = "Move";
			break;
		case 1:
			CurrentState = "MoveCircleRight";
			break;
		case 2:
			CurrentState = "MoveCircleLeft";
			break;
		case 3:
			CurrentState = "MoveCharge";
			break;
		}
	}

	public void SwitchStates()
	{

		switch (CurrentState)
		{
		case	"Idle":
			{
				Audio.Stop();
				CurrentSound = null;
				BaddieAnimationController.SetFloat("Speed",0.0f);
				BaddieAnimationController.SetBool("Attack",false);
				BaddieAnimationController.SetBool("Eat",false);
				BaddieAnimationController.SetBool("Taunt",false);
				break;
			}
		case	"Taunt":
			{
				Audio.Stop();
				CurrentSound = TauntSounds[(Random.Range(0,(TauntSounds.Length-1)))];
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);
				BaddieAnimationController.SetFloat("Speed",0.0f);
				BaddieAnimationController.SetBool("Attack",false);
				BaddieAnimationController.SetBool("Eat",false);
				BaddieAnimationController.SetBool("Taunt",true);
				break;
			}
		case	"MoveCharge":
			{
				Audio.Stop();
				CurrentSound = MoveSounds[(Random.Range(0,(MoveSounds.Length-1)))];
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);
				BaddieAnimationController.SetFloat("Speed",(MoveSpeed * 1.0f));
				BaddieAnimationController.SetBool("Attack",false);
				BaddieAnimationController.SetBool("Eat",false);
				BaddieAnimationController.SetBool("Taunt",false);
				break;
			}
		case	"MoveCircleRight":
			{
				Audio.Stop();
				CurrentSound = MoveSounds[(Random.Range(0,(MoveSounds.Length-1)))];
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);
				BaddieAnimationController.SetFloat("Speed",(MoveSpeed * 0.4f));
				BaddieAnimationController.SetBool("Attack",false);
				BaddieAnimationController.SetBool("Eat",false);
				BaddieAnimationController.SetBool("Taunt",false);
				break;
			}
		case	"MoveCircleLeft":
			{
				Audio.Stop();
				CurrentSound = MoveSounds[(Random.Range(0,(MoveSounds.Length-1)))];
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);
				BaddieAnimationController.SetFloat("Speed",(MoveSpeed * 0.4f));
				BaddieAnimationController.SetBool("Attack",false);
				BaddieAnimationController.SetBool("Eat",false);
				BaddieAnimationController.SetBool("Taunt",false);
				break;
			}
		case	"Move":
			{
				Audio.Stop();
				CurrentSound = MoveSounds[(Random.Range(0,(MoveSounds.Length-1)))];
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);
				BaddieAnimationController.SetFloat("Speed",(MoveSpeed * 0.4f));
				BaddieAnimationController.SetBool("Attack",false);
				BaddieAnimationController.SetBool("Eat",false);
				BaddieAnimationController.SetBool("Taunt",false);
				break;
			}
		case 	"Eat":
			{
				Audio.Stop();
				CurrentSound = EatSounds[(Random.Range(0,(EatSounds.Length-1)))];
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);
				BaddieAnimationController.SetFloat("Speed",0.0f);
				BaddieAnimationController.SetBool("Attack",false);
				BaddieAnimationController.SetBool("Eat",true);
				BaddieAnimationController.SetBool("Taunt",false);
				break;
			}
		case	"Attack":
			{
				Audio.Stop();
				CurrentSound = null;
				BaddieAnimationController.SetFloat("Speed",0.0f);
				BaddieAnimationController.SetBool("Attack",true);
				BaddieAnimationController.SetBool("Eat",false);
				BaddieAnimationController.SetBool("Taunt",false);
				break;
			}
		case	"Die":
			{
				RagDoll(true);
				Player.GetComponent<Player>().StatsBaddiesKilled +=1;
				Audio.Stop();
				CurrentSound = DieSounds[(Random.Range(0,(DieSounds.Length-1)))];
				if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);
				BaddieAnimationController.SetBool("Attack",false);
				BaddieAnimationController.SetBool("Eat",false);
				BaddieAnimationController.SetBool("Die",true);
				BaddieAnimationController.SetBool("Taunt",false);
				break;
			}
		}

		PreviousState = CurrentState;
	}

	public void FindClosestTarget()
	{
		FoodItems = GameObject.FindGameObjectsWithTag("Food");
		ClosestTarget = PlayerCamera; // default to the player
		for (int i = 0; i < FoodItems.Length; i++)
		{
			float ClosestTargetDistance = Vector3.Distance(ClosestTarget.transform.position, transform.position);
			float FoodItemDistance = Vector3.Distance(FoodItems[i].transform.position, transform.position);

			if(FoodItemDistance < ClosestTargetDistance)
			{
				ClosestTarget = FoodItems[i];
//				if(ClosestTarget.GetComponent<Food>().InHandLeft == false && ClosestTarget.GetComponent<Food>().InHandRight == false ) // make sure the food is not being held
//				{
//				}
			}
		}
	}

	// called from an animation event at the end of the taunt animation
	public void EndTaunt()
	{
		CurrentState = "Idle";
	}

	// called from an animation event to eat food
	public void EndEat()
	{
		if(ClosestTarget.GetComponent<Food>() != null)
		{
			ClosestTarget.SendMessage("Die");
		}
		Debug.Log("Eaten");
		WaitTimer = 1f;
		CurrentState = "Idle";
	}


	// called from an animation event to inflict damage on the player.
	public void Attack(int amount)
	{
		CurrentSound = AttackSounds[(Random.Range(0,(AttackSounds.Length-1)))];
		if(!Audio.isPlaying) Audio.PlayOneShot(CurrentSound);

		Player.SendMessage("TakeDamage", AttackDamage);
	}

	// called from the weapon
	public void TakeDamage(int amount)
	{
		HitPoints-= amount;
	}

	public void Die()
	{
		Destroy(this.gameObject);
	}

	public void RagDoll(bool Enable)
	{
		BaddieAnimationController.enabled = !Enable;
		Rigidbody[] RBArray = this.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody RB in RBArray)
		{
			RB.isKinematic = !Enable;
		}
		this.GetComponent<Rigidbody>().isKinematic = Enable;
	}
}
