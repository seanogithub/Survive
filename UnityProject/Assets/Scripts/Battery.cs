using UnityEngine;
using System.Collections;

public class Battery : Holdable {

	public int Power = 60;

	void OnTriggerEnter (Collider other)
	{
		if(InHandLeft || InHandRight)
		{
			string HoldableObjectTag = other.tag;
			//print (HoldableObjectTag);
			if(HoldableObjectTag == "FlashLight" )
			{
				//print("reload");
				other.GetComponent<FlashLight>().Power = Power;
				if(InHandLeft)
				{
					HandControllerLeft.GetComponent<RobustController>().HoldableObject = null;
					HandControllerLeft.GetComponent<RobustController>().HoldingObject = false;
				}
				if(InHandRight)
				{
					HandControllerRight.GetComponent<RobustController>().HoldableObject = null;
					HandControllerRight.GetComponent<RobustController>().HoldingObject = false;
				}
				Destroy(this.gameObject);
			}
			if(HoldableObjectTag == "GunWithLight" )
			{
				//print("reload");
				other.GetComponent<GunWithLight>().Power = Power;
				if(InHandLeft)
				{
					HandControllerLeft.GetComponent<RobustController>().HoldableObject = null;
					HandControllerLeft.GetComponent<RobustController>().HoldingObject = false;
				}
				if(InHandRight)
				{
					HandControllerRight.GetComponent<RobustController>().HoldableObject = null;
					HandControllerRight.GetComponent<RobustController>().HoldingObject = false;
				}
				Destroy(this.gameObject);
			}
			if(HoldableObjectTag == "LightSword" )
			{
				//print("reload");
				other.GetComponent<LightSword>().Power = Power;
				if(InHandLeft)
				{
					HandControllerLeft.GetComponent<RobustController>().HoldableObject = null;
					HandControllerLeft.GetComponent<RobustController>().HoldingObject = false;
				}
				if(InHandRight)
				{
					HandControllerRight.GetComponent<RobustController>().HoldableObject = null;
					HandControllerRight.GetComponent<RobustController>().HoldingObject = false;
				}
				Destroy(this.gameObject);
			}

		}
	}

}
