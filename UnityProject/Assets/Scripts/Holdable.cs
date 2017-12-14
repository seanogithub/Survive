using UnityEngine;
using System.Collections;

public class Holdable : MonoBehaviour {

	public GameObject HandControllerLeft;
	public GameObject HandControllerRight;
	public GameObject LeftHandModel;
	public GameObject RightHandModel;
	public GameObject Player;
	public bool InHandLeft = false;
	public bool InHandRight = false;
	public float PickUpRadius = 0.25f;
	public Vector3 PreviousPosition;
	public Vector3 Velocity;
	public Vector3 PreviousRotation;
	public Vector3 AngularVelocity;
	public bool UseMomentum = true;
	public bool Dropped = false;
	public AudioSource Audio;
	public float ButtonBounceTime = 0;

	virtual public void Awake()
	{
		GameObject CameraRig = GameObject.Find("[CameraRig]");
		HandControllerLeft = CameraRig.transform.FindChild("Controller (left)").gameObject;
		HandControllerRight = CameraRig.transform.FindChild("Controller (right)").gameObject;
		Player = GameObject.Find("Player_Prefab");
		Audio = gameObject.AddComponent<AudioSource>();
		Audio.playOnAwake = false;
		Audio.spatialBlend = 1.0f;
		Audio.maxDistance = 50f;
	}

	// Use this for initialization
	virtual public void Start () {
		Velocity = Vector3.zero;
	}

	virtual public void FixedUpdate()
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
	}

	// Update is called once per frame
	virtual public void Update () 
	{
		ButtonBounceTime +=Time.deltaTime;
		// draw ray for debugging
		//Vector3 dir = transform.rotation * Vector3.forward;
		//Debug.DrawRay(transform.position, dir, Color.red);

		if(this.transform.position.y < -10)
		{
			Destroy(this.gameObject);
		}

	}

	virtual public void GripPressedDown(string HandString)
	{
		if(InHandLeft)
		{
		}
		if(InHandRight)
		{
		}
	}

	virtual public void GripPressedUp(string HandString)
	{
		if(InHandLeft)
		{
		}
		if(InHandRight)
		{
		}
	}

	virtual public void TriggerPressed(string HandString)
	{
		if(InHandLeft)
		{
		}
		if(InHandRight)
		{
		}
	}

	virtual public void TriggerPressedDown(string HandString)
	{
		if(InHandLeft)
		{
		}
		if(InHandRight)
		{
		}
	}

	virtual public void TriggerPressedUp(string HandString)
	{
		if(InHandLeft)
		{
		}
		if(InHandRight)
		{
		}
	}

	virtual public void TouchPadPressed()
	{
	}

	virtual public void PickupHoldable(string HandString)
	{
		string Hand = "";
		if(HandString.Contains("left")) Hand = "left";
		if(HandString.Contains("right")) Hand = "right";

		if(!InHandLeft && Hand=="left")
		{
			if(HandControllerLeft.GetComponent<RobustController>().HoldableObject == this.gameObject)
			{
				InHandLeft = true;
				HandControllerLeft.GetComponent<RobustController>().HoldableObject = this.gameObject;
				HandControllerLeft.GetComponent<RobustController>().HoldingObject = true;
				//return;
			}
		}
		if(!InHandRight && Hand=="right")
		{
			if(HandControllerRight.GetComponent<RobustController>().HoldableObject == this.gameObject)
			{
				InHandRight = true;
				HandControllerRight.GetComponent<RobustController>().HoldableObject = this.gameObject;
				HandControllerRight.GetComponent<RobustController>().HoldingObject = true;
				//return;
			}
		}		
	}

	virtual public void DropHoldable(string HandString)
	{
		string Hand = "";
		if(HandString.Contains("left")) Hand = "left";
		if(HandString.Contains("right")) Hand = "right";

		if(InHandLeft && Hand=="left")
		{
			// drop
			InHandLeft = false;
			Dropped = true;
			HandControllerLeft.GetComponent<RobustController>().HoldableObject = null;
			HandControllerLeft.GetComponent<RobustController>().HoldingObject = false;
			//return;
		}
		if(InHandRight && Hand=="right")
		{
			// drop
			InHandRight = false;
			Dropped = true;
			HandControllerRight.GetComponent<RobustController>().HoldableObject = null;
			HandControllerRight.GetComponent<RobustController>().HoldingObject = false;
			//return;
		}
	}

	virtual public void TouchPadPressedDown(string HandString)
	{
		//Debug.Log(ButtonBounceTime);
		if (ButtonBounceTime > 0.1f )
		{
			ButtonBounceTime = 0;
			//Debug.Log("TouchPadPressedDown");
			string Hand = "";
			if(HandString.Contains("left")) Hand = "left";
			if(HandString.Contains("right")) Hand = "right";

			if(InHandLeft && Hand=="left")
			{
				DropHoldable(HandString);
				return;
			}
			if(InHandRight && Hand=="right")
			{
				DropHoldable(HandString);
				return;
			}
			if(!InHandLeft && Hand=="left")
			{
				PickupHoldable(HandString);
				return;
			}
			if(!InHandRight && Hand=="right")
			{
				PickupHoldable(HandString);
				return;
			}
		}
		else
		{
			Debug.Log("button bounce");
		}
		ButtonBounceTime = 0;

	}


}
