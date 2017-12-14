using UnityEngine;
using System.Collections;
using UnityEngine.VR;
//using System.Runtime.InteropServices;
using Valve.VR;

public class RobustController : MonoBehaviour {

	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId AButton = Valve.VR.EVRButtonId.k_EButton_A;
	private Valve.VR.EVRButtonId XButton = Valve.VR.EVRButtonId.k_EButton_Dashboard_Back;

	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;
	public GameObject HoldableObject;
	private SteamVR_TrackedController TrackedController;
	private bool TouchPadPressedDown = false;
	public bool HoldingObject = false;
	public GameObject HandModel;

	public CVRSystem hmd { get; private set; }
	public string hmd_TrackingSystemName { get { return GetStringProperty(ETrackedDeviceProperty.Prop_TrackingSystemName_String); } }

	// Use this for initialization
	void Start () {
		trackedObject = GetComponent<SteamVR_TrackedObject>();
		TrackedController = GetComponent<SteamVR_TrackedController>();
		hmd = OpenVR.System;

		// move the camera depending on the HMD hardware that is being used
		if(hmd_TrackingSystemName == "vive")
		{
			Vector3 NewPos = this.transform.parent.position;
			NewPos.y = 0.0f;
			this.transform.parent.position = NewPos;
		}

		if(hmd_TrackingSystemName == "oculus")
		{
			Vector3 NewPos = this.transform.parent.position;
			NewPos.y = 1.5f;
			this.transform.parent.position = NewPos;
		}
	}

	string GetStringProperty(ETrackedDeviceProperty prop)
	{
		var error = ETrackedPropertyError.TrackedProp_Success;
		var capactiy = hmd.GetStringTrackedDeviceProperty(OpenVR.k_unTrackedDeviceIndex_Hmd, prop, null, 0, ref error);
		if (capactiy > 1)
		{
			var result = new System.Text.StringBuilder((int)capactiy);
			hmd.GetStringTrackedDeviceProperty(OpenVR.k_unTrackedDeviceIndex_Hmd, prop, result, capactiy, ref error);
			return result.ToString();
		}
		return (error != ETrackedPropertyError.TrackedProp_Success) ? error.ToString() : "<unknown>";
	}

	public void ClearHoldableObject()
	{
		HoldableObject = null;
	}

	void OnTriggerEnter (Collider other)
	{
		HoldableObject = other.transform.root.gameObject;
	}

	void OnTriggerExit (Collider other)
	{
		HoldableObject = null;
	}

	void Update () 
	{
		HandModel.SetActive(!HoldingObject);

		device = SteamVR_Controller.Input((int)trackedObject.index);

		if(hmd_TrackingSystemName == "lighthouse") // HTC Vive HMD
		{
			if(device.GetPressDown(triggerButton) && HoldableObject != null)
			{
				//print("trigger pressed");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TriggerPressedDown(this.name.ToString());
				}
			}
			if(device.GetPressUp(triggerButton) && HoldableObject != null)
			{
				//print("trigger pressed");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TriggerPressedUp(this.name.ToString());
				}
			}
			if(device.GetPress(triggerButton) && HoldableObject != null)
			{
				//print("trigger pressed");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TriggerPressed(this.name.ToString());
				}
			}
		
			if(device.GetPressDown(gripButton) && HoldableObject != null)
			{
				//print("grip pressed down");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().GripPressedDown(this.name.ToString());
				}
			}
			if(device.GetPressUp(gripButton) && HoldableObject != null)
			{
				//print("grip pressed up");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().GripPressedUp(this.name.ToString());
				}
			}

			if(TrackedController.padPressed && HoldableObject != null)
			{
				//print("pad pressed");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TouchPadPressed();
				}
			}
			else
			{
				TouchPadPressedDown = false;
			}

			if(TrackedController.padPressed && TouchPadPressedDown == false && HoldableObject != null)
			{
				//print("pad pressed down");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TouchPadPressedDown(this.name.ToString());
					TouchPadPressedDown = true;
				}
			}		
		}


		if(hmd_TrackingSystemName == "oculus")
		{
			if(device.GetPressDown(triggerButton) && HoldableObject != null)
			{
				//print("trigger pressed");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TriggerPressedDown(this.name.ToString());
				}
			}
			if(device.GetPressUp(triggerButton) && HoldableObject != null)
			{
				//print("trigger pressed");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TriggerPressedUp(this.name.ToString());
				}
			}
			if(device.GetPress(triggerButton) && HoldableObject != null)
			{
				//print("trigger pressed");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TriggerPressed(this.name.ToString());
				}
			}

			//******************************************************************
			// oculus grip button picks up items
			// vive grip button used for secondary item toggle
			//******************************************************************

			if(device.GetPress(gripButton) && HoldableObject != null)
			{
				//print("grip pressed down");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TouchPadPressed();
				}
			}
			else
			{
				TouchPadPressedDown = false;
			}

			if(device.GetPressDown(gripButton) && HoldableObject != null)
			{
				//print("grip pressed up");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().TouchPadPressedDown(this.name.ToString());
					TouchPadPressedDown = true;
				}
			}

			//******************************************************************
			// oculus A button is for the secondary item toggle
			// vive touch pad
			//******************************************************************


			if(device.GetPressDown(AButton) && HoldableObject != null)
			{
				//print("A Button pressed down");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().GripPressedDown(this.name.ToString());
				}
			}

			if(device.GetPressDown(AButton) && HoldableObject != null)
			{
				//print("A Button pressed up");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().GripPressedUp(this.name.ToString());
				}
			}		


			if(device.GetPress(XButton) && HoldableObject != null)
			{
				//print("X Button pressed down");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().GripPressedDown(this.name.ToString());
				}
			}

			if(device.GetPressDown(XButton) && HoldableObject != null)
			{
				//print("X Button pressed up");
				if(HoldableObject.GetComponent<Holdable>() != null) 
				{
					HoldableObject.GetComponent<Holdable>().GripPressedUp(this.name.ToString());
				}
			}		
		}

	}
}
