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
	
	public static GameObject Create(GameObject resource)
	{
		var shot = new GameObject("Shot");
		shot.AddComponent<Shot>();
		shot.AddComponent<BoxCollider>();
		shot.GetComponent<BoxCollider>().isTrigger = true;
		shot.GetComponent<Shot>().CollisionEnabled = false;
		shot.name = "Shot";
		
		GameObject resourceInstance = Instantiate(resource) as GameObject;
		resourceInstance.transform.parent = shot.transform;
		
		return shot;
	}
	
	public void Terminate()
	{
		this.lifeTime = 0;
	}
}
