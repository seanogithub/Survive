using UnityEngine;
using System.Collections;

public class Food : Holdable {

	public int Health = 50;

	override public void TriggerPressedDown(string HandString)
	{
		string Hand = "";
		if(HandString.Contains("left")) Hand = "left";
		if(HandString.Contains("right")) Hand = "right";

		if(InHandLeft && Hand=="left")
		{
			// drop
			InHandLeft = false;
			//Dropped = true;
			HandControllerLeft.GetComponent<RobustController>().HoldableObject = null;
			HandControllerLeft.GetComponent<RobustController>().HoldingObject = false;
			//Eat and give health
			Player.GetComponent<Player>().HitPoints+= Health;
			Destroy(this.gameObject);
			return;
		}
		if(InHandRight && Hand=="right")
		{
			// drop
			InHandRight = false;
			//Dropped = true;
			HandControllerRight.GetComponent<RobustController>().HoldableObject = null;
			HandControllerRight.GetComponent<RobustController>().HoldingObject = false;
			//Eat and give health
			Player.GetComponent<Player>().HitPoints+= Health;
			Destroy(this.gameObject);
			return;
		}
	}

	public void Die()
	{
		Destroy(this.gameObject);
	}
}
