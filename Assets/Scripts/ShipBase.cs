using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBase : ActiveEntity 
{
	public delegate void NotifyDelegate();
	
	private readonly IDictionary<Collider, float> collisionTrigger = new Dictionary<Collider, float>();	
	protected readonly IDictionary<Weapon, float> weapons = new Dictionary<Weapon, float>();
	
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
	
	public void AddWeapon(Weapon weapon)
	{
		this.weapons.Add(weapon, 0);
	}
	
	public void Fire(bool respectCooldown = true)
	{
		if(this.weapons.Count == 0 || this.isDead)
		{
			return;
		}
		
		IList<Weapon> weapons = new List<Weapon>(this.weapons.Keys);
		foreach(Weapon weapon in weapons)
		{
			if(this.weapons[weapon] > 0 && respectCooldown)
			{
				continue;
			}
			
			print ("Firing Weapon: "+weapon.Name);
			weapon.Fire(transform.position);
			this.weapons[weapon] = weapon.Cooldown;
		}
	}
	
	public void TakeDamage(float damage, GameObject source = null)
	{
		if(this.isDead)
		{
			return;
		}
		
		print ("Taking "+damage+" from "+source);
		
		// Todo: Add fancy damage calculations here
		this.health -= damage;
		if(this.health <= 0)
		{
			print("Dying..");
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
		
		IList<Weapon> weapons = new List<Weapon>(this.weapons.Keys);
		foreach(Weapon weapon in weapons)
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
			if(!this.collisionTrigger.ContainsKey(collider))
			{
				this.collisionTrigger.Add(collider, component.CollisionInterval);
			}
			
			if(this.collisionTrigger[collider] <= 0)
			{
				this.collisionTrigger[collider] = component.CollisionInterval;				
			} else
			{
				return;
			}
			
			print("Taking collision damage of "+component.CollisionDamage);
			this.TakeDamage(component.CollisionDamage, collider.gameObject);
		}
	}
}
