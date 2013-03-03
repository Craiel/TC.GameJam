using UnityEngine;
using System.Collections;

public class WeaponSingleDumbFire : Weapon 
{	
	private GameObject resource;
	private GameObject resourceFriendly;
	
	void Awake()
	{
		this.resource = Resources.Load("DumbFire") as GameObject;
		this.resourceFriendly = Resources.Load("DumbFireFriendly") as GameObject;
	}
	
	void Start()
	{
		
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
		
		shot.GetComponent<Shot>().Initialize(this.Target, this.transform.position, 15.0f, 50f);
		shot.GetComponent<Shot>().SetCollision(1.0f, 0.1f);
		shot.GetComponent<Shot>().Source = this.Source;
		GameObject.Find("GameManager").GetComponent<GameManager>().AddShot(shot);
	}
	
	public static GameObject Create()
	{
		var obj = new GameObject("WeaponSingleDumbFire");
		obj.AddComponent<WeaponSingleDumbFire>();
		return obj;
	}
}
