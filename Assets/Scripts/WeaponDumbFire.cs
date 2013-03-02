using UnityEngine;
using System.Collections;

public class WeaponDumbFire : Weapon 
{
	public override void Fire ()
	{
		var shot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		shot.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
		shot.AddComponent<Shot>();
		shot.AddComponent<BoxCollider>();
		shot.GetComponent<BoxCollider>().isTrigger = true;
		shot.GetComponent<Shot>().CollisionDamage = 10.0f;
		shot.GetComponent<Shot>().CollisionInterval = 0.1f;
		shot.GetComponent<Shot>().Initialize(Vector3.up, this.Origin, 5.0f, 7f);
		shot.name = "Shot";
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
		
		shot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		shot.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
		shot.AddComponent<Shot>();
		shot.AddComponent<BoxCollider>();
		shot.GetComponent<BoxCollider>().isTrigger = true;
		shot.GetComponent<Shot>().CollisionDamage = 20.0f;
		shot.GetComponent<Shot>().CollisionInterval = 0.1f;
		shot.GetComponent<Shot>().Initialize(new Vector3(-0.2f, 0.5f, 0).normalized, this.Origin, 5.0f, 7f);
		shot.name = "Shot";
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
		
		shot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		shot.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
		shot.AddComponent<Shot>();
		shot.AddComponent<BoxCollider>();
		shot.GetComponent<BoxCollider>().isTrigger = true;
		shot.GetComponent<Shot>().CollisionDamage = 20.0f;
		shot.GetComponent<Shot>().CollisionInterval = 0.1f;
		shot.GetComponent<Shot>().Initialize(new Vector3(0.2f, 0.5f, 0).normalized, this.Origin, 5.0f, 7f);
		shot.name = "Shot";		
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
	}
}
