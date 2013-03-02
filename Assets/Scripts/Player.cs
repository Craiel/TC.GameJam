using UnityEngine;
using System.Collections;

public class Player : ShipBase 
{
	public float StartingHealth = 100.0f;
	
	// Use this for initialization
	void Start () 
	{	
		this.Health = this.StartingHealth;
		
		this.AddWeapon(new Weapon{Name="Test Gun", Cooldown = 0.2f});
		this.AddWeapon(new Weapon{Name="SlowMoFo", Cooldown = 4.0f});
	}
	
	public void Update()
	{
		base.Update();
		
		if(Input.GetMouseButton(0))
		{
			this.Fire();
		}
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			foreach(Weapon weapon in this.weapons.Keys)
			{
				// Test only for first weapon
				weapon.AlternateFireMode = !weapon.AlternateFireMode;
				break;
			}
		}
	}
}
