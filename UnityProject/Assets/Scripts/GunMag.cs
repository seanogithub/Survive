using UnityEngine;
using System.Collections;

public class GunMag : Holdable {

	public int Ammo = 10;

	public GameObject MagModel;
	public AudioClip[] ReloadSounds;

	// Use this for initialization
	override public void Start () {
		Velocity = Vector3.zero;
		Audio = GetComponent<AudioSource>();
	}

	void OnTriggerEnter (Collider other)
	{
		if(InHandLeft || InHandRight)
		{
			// make sure that its a hand gun
			string HoldableObjectTag = other.tag;
			//Debug.Log (HoldableObjectTag);
			if(HoldableObjectTag == "HandGun")
			{
				// make sure the gun is empty
				int GunAmmoLeft = other.GetComponent<Gun>().WeaponAnimationController.GetInteger("Ammo");
				if(GunAmmoLeft < 1)
				{
					//print("reload");
					MagModel.SetActive(false);
					other.GetComponent<Gun>().WeaponAnimationController.SetInteger("Ammo", Ammo);
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
					Audio.PlayOneShot(ReloadSounds[(Random.Range(0,(ReloadSounds.Length-1)))]);
					Destroy(this.gameObject, 0f);
				}
			}
			if(HoldableObjectTag == "GunWithLight")
			{
				// make sure the gun is empty
				int GunAmmoLeft = other.GetComponent<GunWithLight>().WeaponAnimationController.GetInteger("Ammo");
				if(GunAmmoLeft < 1)
				{
					//print("reload");
					MagModel.SetActive(false);
					other.GetComponent<GunWithLight>().WeaponAnimationController.SetInteger("Ammo", Ammo);
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
					Audio.PlayOneShot(ReloadSounds[(Random.Range(0,(ReloadSounds.Length-1)))]);
					Destroy(this.gameObject, 0f);
				}
			}
		}
	}

}
