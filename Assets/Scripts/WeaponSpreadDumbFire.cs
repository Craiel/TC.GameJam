using UnityEngine;
using System.Collections;

public class WeaponSpreadDumbFire : Weapon 
{
	private Vector3 scale = new Vector3(0.2f, 0.4f, 0.2f);
	
	private GameObject resource;
	
	void Start()
	{
		this.resource = Resources.Load("DumbFireFriendly") as GameObject;
	}
	
	public override void Fire ()
	{		
		var shot = Shot.Create(this.resource);
		shot.transform.localScale = this.scale;
		shot.GetComponent<Shot>().Initialize(Vector3.up, this.transform.position, 5.0f, 7f);
		shot.GetComponent<Shot>().SetCollision(10.0f, 0.1f);
		shot.GetComponent<Shot>().Source = this.Source;
		shot.name = "Shot";
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
		
		shot = Shot.Create(this.resource);
		shot.transform.localScale = this.scale;
		shot.GetComponent<Shot>().Initialize(new Vector3(-0.2f, 0.5f, 0).normalized, this.transform.position, 5.0f, 7f);
		shot.GetComponent<Shot>().SetCollision(20.0f, 0.1f);
		shot.GetComponent<Shot>().Source = this.Source;
		shot.name = "Shot";
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
		
		shot = Shot.Create(this.resource);
		shot.transform.localScale = this.scale;
		shot.GetComponent<Shot>().Initialize(new Vector3(0.2f, 0.5f, 0).normalized, this.transform.position, 5.0f, 7f);
		shot.GetComponent<Shot>().SetCollision(20.0f, 0.1f);
		shot.GetComponent<Shot>().Source = this.Source;
		shot.name = "Shot";		
		Camera.main.GetComponent<GameControlTest>().AddShot(shot);
	}
	
	public static GameObject Create()
	{
		var obj = new GameObject("WeaponSpreadDumbFire");
		obj.AddComponent<WeaponSpreadDumbFire>();
		return obj;
	}
}
