using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	public float Cooldown;
		
	public Vector3 Target = Vector3.up;
	
	public ShotSource Source;
	
	public virtual void Fire()
	{
	}
	
	public virtual void AlternateFire()
	{
	}
	
	public virtual void Disable()
	{
	}
}
