using UnityEngine;
using System.Collections;

public class Player : ShipBase 
{
	public float StartingHealth = 100.0f;
	
	// Use this for initialization
	void Start () 
	{	
		this.Health = this.StartingHealth;
		
		/*var weapon = WeaponSpreadDumbFire.Create();
		weapon.name = "Test Gun";
		weapon.GetComponent<Weapon>().Source = ShotSource.Friend;
		weapon.GetComponent<Weapon>().Cooldown = 0.2f;*/
		
		var weapon = WeaponGravity.Create();
		weapon.name = "Gravity Gun";
		weapon.GetComponent<Weapon>().Source = ShotSource.Friend;
		
		this.AddWeapon(weapon);
	}
	
	public void Update()
	{
		base.Update();
		
		if(Input.GetMouseButton(0))
		{
			this.Fire();
		}
		else
		{
			this.Disable();
		}
		
		float wheel = Input.GetAxis("Mouse ScrollWheel");
		if(wheel < 0.0f || wheel > 0.0f)
		{
			foreach(GameObject weapon in this.weapons.Keys)
			{
				if(weapon.GetComponent<WeaponGravity>() != null)
				{
					weapon.GetComponent<WeaponGravity>().ChangeRadius(wheel * 75.0f);
				}
			}
		}
	}
	
	protected override bool AcceptShotSource (ShotSource source)
	{
		return source != ShotSource.Friend;	
	}
}
