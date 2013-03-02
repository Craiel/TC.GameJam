using UnityEngine;
using System.Collections;

public abstract class Weapon
{
	public string Name;
	
	public float Cooldown;
		
	public Vector3 Origin;
	public Vector3 Target = Vector3.up;
	
	public abstract void Fire();
}
