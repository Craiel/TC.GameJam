using UnityEngine;
using System.Collections;

public class Shot : ActiveEntity 
{
	private Vector3 direction;
	private Vector3? startPos;
	
	private float speed;
	private float lifeTime;
	
	private ShotSource source;
	
	public enum ShotSource
	{
		Friend,
		Foe
	}
		
	public float LifeTime
	{
		get
		{
			return this.lifeTime;
		}
	}
	
	public float Speed
	{
		get
		{
			return this.speed;
		}
	}
	
	public ShotSource Source
	{
		get
		{
			return this.source;
		}
	}
	
	void Update()
	{		
		if(this.startPos != null)
		{
			this.transform.position = (Vector3)this.startPos;
			this.startPos = null;
		}
		
		this.lifeTime -= Time.deltaTime;
		this.transform.Translate(this.direction * this.speed * Time.deltaTime);
	}
	
	public void Initialize(Vector3 direction, Vector3 position, float lifeTime, float speed)
	{
		this.direction = direction;
		this.startPos = position;
		this.lifeTime = lifeTime;
		this.speed = speed;
	}
	
	void OnTriggerExit(Collider collider)
	{
		ActiveEntity component = collider.gameObject.GetComponent(typeof(ActiveEntity)) as ActiveEntity;
		if(component != null && component.GetType() != this.GetType() && component.GetType() != typeof(Player))
		{
			this.lifeTime = 0;
		}
	}
}
