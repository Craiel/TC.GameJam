using UnityEngine;
using System.Collections;

public enum ShotSource
{
	Friend,
	Foe
}

public class Shot : ActiveEntity 
{
	private Vector3 direction;
	private Vector3? startPos;
	
	private float speed;
	private float lifeTime;	
		
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
	
	public bool IsActive;
	public bool KillsShots;
	
	public ShotSource Source;
	
	public void ChangeTarget(Vector3 target)
	{
		this.direction = (target - this.transform.position).normalized;
	}
	
	public void ChangeSpeed(float diff)
	{
		this.speed += diff;
	}
	
	void Update()
	{		
		if(!this.IsActive)
		{
			return;
		}
		
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
	
	public static GameObject Create(GameObject resource, bool rigid = false)
	{
		var shot = new GameObject("Shot");
		shot.AddComponent<Shot>();
		shot.AddComponent<BoxCollider>();
		shot.GetComponent<BoxCollider>().isTrigger = true;
		shot.GetComponent<Shot>().CollisionEnabled = false;
		shot.name = "Shot";
		
		if(rigid)
		{
			shot.AddComponent<Rigidbody>();
			shot.GetComponent<Rigidbody>().useGravity = false;
			shot.GetComponent<Rigidbody>().isKinematic = true;
		}
		
		GameObject resourceInstance = Instantiate(resource) as GameObject;
		resourceInstance.transform.parent = shot.transform;
		
		return shot;
	}
	
	public void Terminate()
	{
		this.lifeTime = 0;
	}
	
	private void OnTriggerEnter(Collider collider)
	{
		ActiveEntity component = collider.gameObject.GetComponent(typeof(ActiveEntity)) as ActiveEntity;
		if(component != null)
		{
			if(!component.CollisionEnabled)
			{
				return;
			}
			
			if(component.GetType() == typeof(Shot))
			{
				if(((Shot)component).KillsShots && ((Shot)component).Source != this.Source)
				{
					this.Terminate();
				}
			}
		}
	}
}
