using UnityEngine;
using System.Collections;

public class Weapon 
{
	public string Name;
	
	public float Cooldown;
	
	public bool AlternateFireMode;
	
	public void Fire(Vector3 origin)
	{
		if(this.AlternateFireMode)
		{
			this.VulcanSpread(origin);
		} else
		{
			this.FireSpread(origin);
		}
	}
	
	private void VulcanSpread(Vector3 origin)
	{
		var shot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		shot.transform.localScale = new Vector3(0.1f, 5f, 0.1f);
		shot.AddComponent<Shot>();
		shot.AddComponent<BoxCollider>();
		shot.GetComponent<BoxCollider>().isTrigger = true;
		shot.GetComponent<Shot>().CollisionDamage = 50.0f;
		shot.GetComponent<Shot>().CollisionInterval = 0.1f;
		shot.GetComponent<Shot>().Initialize(Vector3.up, origin, 1.0f, 50f);
		shot.name = "Shot";
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
	}
	
	private void FireSpread(Vector3 origin)
	{
		var shot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		shot.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
		shot.AddComponent<Shot>();
		shot.AddComponent<BoxCollider>();
		shot.GetComponent<BoxCollider>().isTrigger = true;
		shot.GetComponent<Shot>().CollisionDamage = 10.0f;
		shot.GetComponent<Shot>().CollisionInterval = 0.1f;
		shot.GetComponent<Shot>().Initialize(Vector3.up, origin, 5.0f, 7f);
		shot.name = "Shot";
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
		
		shot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		shot.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
		shot.AddComponent<Shot>();
		shot.AddComponent<BoxCollider>();
		shot.GetComponent<BoxCollider>().isTrigger = true;
		shot.GetComponent<Shot>().CollisionDamage = 20.0f;
		shot.GetComponent<Shot>().CollisionInterval = 0.1f;
		shot.GetComponent<Shot>().Initialize(new Vector3(-0.2f, 0.5f, 0).normalized, origin, 5.0f, 7f);
		shot.name = "Shot";
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
		
		shot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		shot.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
		shot.AddComponent<Shot>();
		shot.AddComponent<BoxCollider>();
		shot.GetComponent<BoxCollider>().isTrigger = true;
		shot.GetComponent<Shot>().CollisionDamage = 20.0f;
		shot.GetComponent<Shot>().CollisionInterval = 0.1f;
		shot.GetComponent<Shot>().Initialize(new Vector3(0.2f, 0.5f, 0).normalized, origin, 5.0f, 7f);
		shot.name = "Shot";		
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
	}
}
