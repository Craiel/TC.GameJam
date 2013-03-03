using UnityEngine;
using System.Collections;

public class WeaponSingleDumbFire : Weapon 
{
	private Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);
	
	private GameObject resource;
	private GameObject resourceFriendly;
	
	void Start()
	{
		this.resource = Resources.Load("DumbFire") as GameObject;
		this.resourceFriendly = Resources.Load("DumbFireFriendly") as GameObject;
	}
	
	public override void Fire ()
	{		
		GameObject shot =  null;
		switch(this.Source)
		{
			case ShotSource.Friend:
			{
				shot = Shot.Create(this.resourceFriendly);
				break;
			}
			
			case ShotSource.Foe:
			{
				shot = Shot.Create(this.resource);
				break;
			}
		}
		
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
