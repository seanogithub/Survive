using UnityEngine;
using System.Collections;

public class Bomb : Holdable {

	public GameObject Light;
	public GameObject FuseModel;
	public GameObject BombModel;
	public GameObject ExplosionFX;
	public int Damage = 200;
	public float DamageInnerRadius = 2;
	public float DamageOutterRadius = 5;
	public float FuseTimer = 3.0f;
	public float ExplosionTimer = 3.0f;
	public string State = "Ready";

	// Use this for initialization
	override public void Start () {
		Velocity = Vector3.zero;
		if(Light != null) Light.SetActive(false);
		if(ExplosionFX != null) ExplosionFX.SetActive(false);
	}

	override public void Update () 
	{
		ButtonBounceTime +=Time.deltaTime;
		switch(State)
		{
		case "Ready":
			{
				// don't do anything
				break;
			}
		case "Triggered":
			{
				if(FuseModel != null) FuseModel.SetActive(false);
				FuseTimer -= Time.deltaTime;
				if(FuseTimer < 0)
				{
					State = "Detonate";
				}
				break;
			}
		case "Detonate":
			{
				if(Light != null) Light.SetActive(true);
				if(BombModel != null) BombModel.SetActive(false);
				if(ExplosionFX != null)
				{
					ExplosionFX.SetActive(true);
					ExplosionFX.SendMessage("StartExplosion");
				}
				DoPlayerDamage();
				DoBaddieDamage();
				State = "Exploding";
				break;
			}
		case "Exploding":
			{
				if(Light != null) Light.GetComponent<Light>().intensity -= ExplosionTimer / 10;
				ExplosionTimer -= Time.deltaTime;
				if(ExplosionTimer < 0)
				{
					State = "Die";
				}
				break;
			}
		case "Die":
			{
				Destroy(this.gameObject);
				break;
			}
		}
	}

	public void DoPlayerDamage()
	{
		if(InHandLeft || InHandRight)
		{
			int PlayerDamage = (int)((float)Damage/2f); // do half the damage if its in the players hand
			Player.GetComponent<Player>().SendMessage("TakeDamage", PlayerDamage);
		}
		else
		{
			float PlayerDistance = Vector3.Distance(Player.transform.position, transform.position);
			float PlayerDamage = DamageOutterRadius - PlayerDistance;
			if(PlayerDamage < 0) // baddie is too far away
			{
				PlayerDamage = 0;
			}
			if(PlayerDamage > 0)
			{
				if(PlayerDistance < DamageInnerRadius) 
				{
					PlayerDamage = Damage; // baddie is inside the inner radius so take the full damage
				}
				else
				{
					PlayerDamage = (1 - ((PlayerDistance - DamageInnerRadius)/(DamageOutterRadius-DamageInnerRadius))) * Damage / 4; // damage is propotional to the distance between the inner and outer radius
				}
				Player.SendMessage("TakeDamage", PlayerDamage);
				//Debug.Log("PlayerDistance = " + PlayerDistance + " - PlayerDamage = " + PlayerDamage);
			}
		}
	}

	public void DoBaddieDamage()
	{
		GameObject[] BaddieList = GameObject.FindGameObjectsWithTag("Baddie");
		//Debug.Log("Explosion Damage");
		for (int i = 0; i < BaddieList.Length; i++)
		{
			float BaddieDistance = Vector3.Distance(BaddieList[i].transform.position, transform.position);

			float BaddieDamage = DamageOutterRadius - BaddieDistance;
			if(BaddieDamage < 0) // baddie is too far away
			{
				BaddieDamage = 0;
			}
			if(BaddieDamage > 0)
			{
				if(BaddieDistance < DamageInnerRadius) 
				{
					BaddieDamage = Damage; // baddie is inside the inner radius so take the full damage
				}
				else
				{
					BaddieDamage = (1 - ((BaddieDistance - DamageInnerRadius)/(DamageOutterRadius-DamageInnerRadius))) * Damage; // damage is propotional to the distance between the inner and outer radius
				}
				BaddieList[i].SendMessage("TakeDamage", BaddieDamage);
				//Debug.Log("BaddieDistance = " + BaddieDistance + " - BaddieDamage = " + BaddieDamage);
			}
		}
	}

	override public void TriggerPressedDown(string HandString)
	{
		if(InHandLeft || InHandRight)
		{
			// dont toggle... just turn it on once and it can't be turned off.
			State = "Triggered";
		}
	}

}
