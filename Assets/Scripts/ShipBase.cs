using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBase : ActiveEntity 
{
	public delegate void NotifyDelegate();
	
	private readonly IDictionary<Collider, float> collisionTrigger = new Dictionary<Collider, float>();	
	protected readonly IDictionary<GameObject, float> weapons = new Dictionary<GameObject, float>();
	
	private float health;
	private float maxHealth;
	
	private bool isDead = false;
	
	// ---------------------------------------------
	// Public
	// ---------------------------------------------
	public event NotifyDelegate OnDying;
	public event NotifyDelegate OnTakenDamage;
	
	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
	}
	
	public float MaxHealth
	{
		get
		{
			return this.maxHealth;
		}
	}
	
	public float Health
	{
		get
		{
			return this.health;
		}
		
		set
		{
			this.health = value;
			this.maxHealth = value;
			this.isDead = this.health <= 0.0f;
		}
	}
	
	public void AddWeapon(GameObject weapon)
	{
		weapon.transform.parent = this.transform;
		this.weapons.Add(weapon, 0);
	}
	
	public void Fire(bool respectCooldown = true, Vector3? target = null)
	{
		if(this.weapons.Count == 0 || this.isDead)
		{
			return;
		}
		
		IList<GameObject> weapons = new List<GameObject>(this.weapons.Keys);
		foreach(GameObject weapon in weapons)
		{
			if(this.weapons[weapon] > 0 && respectCooldown)
			{
				continue;
			}
			
			if(target != null)
			{
				weapon.GetComponent<Weapon>().Target = (Vector3)target;
			}
			
			weapon.GetComponent<Weapon>().Fire();
			this.weapons[weapon] = weapon.GetComponent<Weapon>().Cooldown;
		}
	}
	
	public virtual void Disable()
	{
		if(this.weapons.Count == 0 || this.isDead)
		{
			return;
		}
		
		IList<GameObject> weapons = new List<GameObject>(this.weapons.Keys);
		foreach(GameObject weapon in weapons)
		{
			weapon.GetComponent<Weapon>().Disable();
		}
	}
	
	public void TakeDamage(float damage, GameObject source = null)
	{
		if(this.isDead)
		{
			return;
		}
				
		// Todo: Add fancy damage calculations here
		this.health -= damage;
		if(this.health <= 0)
		{
			this.isDead = true;
			if(this.OnDying != null)
			{
				this.OnDying();
			}
			
			return;
		}
		
		if(this.OnTakenDamage != null)
		{
			this.OnTakenDamage();
		}
	}
		
	// Update is called once per frame
	public virtual void Update () 
	{
		IList<Collider> colliderList = new List<Collider>(this.collisionTrigger.Keys);
		foreach(Collider collider in colliderList)
		{
			this.collisionTrigger[collider] -= Time.deltaTime;
		}
		
		IList<GameObject> weapons = new List<GameObject>(this.weapons.Keys);
		foreach(GameObject weapon in weapons)
		{
			this.weapons[weapon] -= Time.deltaTime;			
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		this.ProcessCollision(collider);
	}
	
	void OnTriggerStay(Collider collider)
	{
		this.ProcessCollision(collider);
	}
	
	// ---------------------------------------------
	// Private
	// ---------------------------------------------
	private void ProcessCollision(Collider collider)
	{
		ActiveEntity component = collider.gameObject.GetComponent(typeof(ActiveEntity)) as ActiveEntity;
		if(component != null && component.GetType() != this.GetType())
		{
			if(!component.CollisionEnabled)
			{
				return;
			}
			
			if(!this.collisionTrigger.ContainsKey(collider))
			{
				this.collisionTrigger.Add(collider, component.CollisionInterval);
			}
			
			if(this.collisionTrigger[collider] > 0)
			{
				return;
			}
			
			if(component.GetType() == typeof(Shot))
			{
				if(!this.AcceptShotSource(((Shot)component).Source))
				{
					return;
				} 
				else
				{
					((Shot)component).Terminate();
				}
			}
			
			this.collisionTrigger[collider] = component.CollisionInterval;
			this.TakeDamage(component.CollisionDamage, collider.gameObject);
		}
	}
	
	protected virtual bool AcceptShotSource(ShotSource source)
	{
		return true;
	}
}
