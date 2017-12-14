using UnityEngine;
using System.Collections;

public class Knife : Holdable {

	public int DamagePerFrame = 5;

	void OnCollisionStay (Collision other)
	{
		if(InHandLeft || InHandRight)
		{
			string ObjectTag = other.collider.transform.root.tag;
			if(ObjectTag == "Baddie")
			{
				//print("Knife Damage !");
				other.collider.transform.root.GetComponent<Baddie>().SendMessage("TakeDamage", DamagePerFrame);
			}
		}
	}
}
