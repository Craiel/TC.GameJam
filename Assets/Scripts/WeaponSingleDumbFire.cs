using UnityEngine;
using System.Collections;

public class WeaponSingleDumbFire : Weapon 
{
	private Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);
	
	private GameObject resource;
	
	void Start()
	{
		this.resource = Resources.Load("Bullet") as GameObject;
	}
	
	public override void Fire ()
	{		
		var shot = Shot.Create(this.resource);
		shot.transform.localScale = this.scale;
		shot.GetComponent<Shot>().Initialize(this.Target, this.transform.position, 7.0f, 3f);
		shot.GetComponent<Shot>().SetCollision(1.0f, 0.1f);
		shot.GetComponent<Shot>().Source = this.Source;
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
	}
	
	public static GameObject Create()
	{
		var obj = new GameObject("WeaponSingleDumbFire");
		obj.AddComponent<WeaponSingleDumbFire>();
		return obj;
	}
}
