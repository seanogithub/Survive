using UnityEngine;
using System.Collections;

public class ControllerUI : MonoBehaviour {

	public bool Init = false;
	public GameObject HandControllerLeft;
	public GameObject HandControllerRight;
	public GameObject DefaultModelLeft;
	public GameObject DefaultModelRight;

	public GameObject ControllerPrefab;
	public GameObject LeftHand;
	public GameObject RightHand;

	virtual public void Awake()
	{
		DefaultModelLeft = HandControllerLeft.GetComponentInChildren<SteamVR_RenderModel>().gameObject;
		DefaultModelRight = HandControllerRight.GetComponentInChildren<SteamVR_RenderModel>().gameObject;
	}

	// Use this for initialization
	void Start () {

	}

	public void Initialize()
	{

		Init = false;
		//Debug.Log("Home Init");

		LeftHand = Instantiate(ControllerPrefab);
		LeftHand.GetComponent<MenuController>().InHandLeft = true;
		HandControllerLeft.GetComponent<RobustController>().HoldingObject = true;
		DefaultModelLeft.SetActive(true);

		RightHand = Instantiate(ControllerPrefab);
		RightHand.GetComponent<MenuController>().InHandRight = true;
		HandControllerRight.GetComponent<RobustController>().HoldingObject = true;
		DefaultModelRight.SetActive(true);
	}

	// Update is called once per frame
	void Update () {
		if(Init) Initialize();

	}

	public void DestroyControllers()
	{
		HandControllerLeft.GetComponent<RobustController>().HoldingObject = false;
		HandControllerRight.GetComponent<RobustController>().HoldingObject = false;
		DefaultModelLeft.SetActive(false);
		DefaultModelRight.SetActive(false);
		Destroy(LeftHand);
		Destroy(RightHand);
	}
}
