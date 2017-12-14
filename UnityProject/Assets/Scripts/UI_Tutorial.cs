using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Tutorial : MonoBehaviour {

	public bool Init = false;
	public GameObject UIManager;
	public GameObject TutorialImage;
	public Sprite[] TutorialImageArray;
	public int CurrentImageIndex = 0;

	void Awake()
	{
		UIManager = GameObject.Find("UI_Manager_Prefab");
	}

	// Use this for initialization
	void Start () {

	}

	public void Initialize()
	{
		Init = false;
		CurrentImageIndex = 0;
		UpdateImage();
	}

	// Update is called once per frame
	void Update () 
	{
		if(Init) Initialize();
	}

	public void UpdateImage()
	{
		if(TutorialImageArray[CurrentImageIndex] != null)
		{
			TutorialImage.GetComponent<Image>().sprite = TutorialImageArray[CurrentImageIndex];
		}
	}

	public void Next()
	{
		CurrentImageIndex++;
		if(CurrentImageIndex >= TutorialImageArray.Length)
		{
			CurrentImageIndex = TutorialImageArray.Length - 1;
			// switch states back to home
			this.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("HomeState");
		}
		UpdateImage();
	}

	public void Back()
	{
		CurrentImageIndex--;
		if(CurrentImageIndex < 0 )
		{
			CurrentImageIndex = 0;
			// switch states back to home
			this.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("HomeState");
		}
		UpdateImage();
	}


}
