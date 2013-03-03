using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponGravity : Weapon 
{
	private GameObject resource;
	private GameObject resourceGrabbed;
	
	private GameObject radiusIncreaseIndicator;
	
	private bool isEnabled;
	
	private IDictionary<GameObject, float> pendingForGrab = new Dictionary<GameObject, float>();
	private IDictionary<GameObject, Vector3> grabLockVectors = new Dictionary<GameObject, Vector3>();
	private IList<GameObject> grabbedShots = new List<GameObject>();
	
	private float grabTime = 0.2f;
	private float angle = 0.0f;
	private float shotMoveSpeed = 2.5f;
	private int maxGrab = 60;
	
	private float scaleGrabTimeAdd = 0f;
	private float massGrabTimeAdd = 0f;
	
	private float scaleGrabAccelleration = 0f;
	
	private float grabLockSpeed = 170.0f;
	private float grabLockRadius = 2.0f;
	
	private bool delayRadiusIncrease = false;
	private float radiusIncreaseInterval = 0.5f;
	private float radiusIncreaseState = 0;
	private float radiusIncrease = 2.5f;
	private float radiusIncreaseDelay = 0.5f;
	private float radiusIncreaseDelayState = 0;
		
	private float colliderRadius;
	private float colliderRadiusMin = 22.0f;
	private float colliderRadiusMax = 90.0f;
	
	private Vector3 rotationOffset = new Vector3(1.5f, 0, 0);
	
	void Start()
	{
		var wellResource = Resources.Load("GravityWell") as GameObject;
		this.resource = Instantiate(wellResource) as GameObject;
		this.resource.transform.parent = this.transform;
		this.resource.transform.localPosition = Vector3.zero;
		this.resource.renderer.enabled = false;
		
		this.radiusIncreaseIndicator = Instantiate(Resources.Load("GravityWellIndicator") as GameObject) as GameObject;
		this.radiusIncreaseIndicator.transform.parent = this.transform;
		this.radiusIncreaseIndicator.transform.localPosition = Vector3.zero;
		this.radiusIncreaseIndicator.renderer.enabled = false;
				
		this.resourceGrabbed = Resources.Load("DumbFireFriendly") as GameObject;		
	}
	
	public override void Fire()
	{
		if(this.isEnabled)
		{
			return;
		}
		
		print("Enabling Gravity well");
		this.isEnabled = true;
		this.resource.renderer.enabled = true;
		
		float initial = 0;
		if(this.grabbedShots.Count > 0)
		{
			initial = (this.colliderRadiusMax - this.colliderRadiusMin) * ((float)this.grabbedShots.Count / (float)this.maxGrab);
		}
		
		this.ChangeRadius(this.colliderRadiusMin - this.colliderRadius + initial);
	}
	
	public override void AlternateFire()
	{
		if(this.grabbedShots.Count <= 0)
		{
			return;
		}
		
		IList<GameObject> shots = new List<GameObject>(this.grabbedShots);
		this.grabbedShots.Clear();
		foreach(GameObject grabbed in shots)
		{
			Vector3 pos = grabbed.transform.position;
			Vector3 dir = (pos - this.transform.position).normalized;
			
			DestroyObject(grabbed);
			
			var shot = Shot.Create(this.resourceGrabbed, true);
			//shot.transform.localScale = //this.scale * 8;
			shot.GetComponent<Shot>().Initialize(dir, pos, 7.0f, 50f);
			shot.GetComponent<Shot>().SetCollision(100.0f, 0.1f);
			shot.GetComponent<Shot>().KillsShots = true;
			shot.GetComponent<Shot>().Source = this.Source;
			
			shot.name = "Gravity Shot";
			GameObject.Find("GameManager").GetComponent<GameManager>().AddShot(shot);
		}
	}
	
	public override void Disable()
	{
		if(!this.isEnabled)
		{
			return;
		}
		
		print("Disabling Gravity well");
		this.radiusIncreaseState = 0;
		this.delayRadiusIncrease = false;
		this.isEnabled = false;
		this.resource.renderer.enabled = false;		
		this.radiusIncreaseIndicator.renderer.enabled = false;
	}
	
	public static GameObject Create()
	{
		var obj = new GameObject("WeaponGravity");
		obj.AddComponent<WeaponGravity>();
		return obj;
	}
	
	private void ChangeRadius(float change)
	{
		this.resource.transform.localScale += new Vector3(change, change, change);
		this.colliderRadius += change;		
		this.scaleGrabTimeAdd = this.colliderRadius / 40.0f;
		this.scaleGrabAccelleration = this.colliderRadius / 10.0f;
	}
	
	public void Update()
	{
		this.angle += this.shotMoveSpeed;
		if(this.grabbedShots.Count > 0)
		{
			float spacing = 360 / this.grabbedShots.Count;
			IList<GameObject> shots = new List<GameObject>(this.grabbedShots);
			for(int i=0;i<shots.Count;i++)
			{
				Vector3 rotationSlot = this.resource.transform.position + (Quaternion.AngleAxis(this.angle + (i * spacing), Vector3.forward) *
					(this.rotationOffset + new Vector3(this.colliderRadius / 4.0f, 0, 0)));
				if(this.grabLockVectors.ContainsKey(shots[i]))
				{
					Vector3 dir = rotationSlot - this.grabLockVectors[shots[i]];
					if(dir.magnitude <= this.grabLockRadius)
					{
						this.grabLockVectors.Remove(shots[i]);
					}
					else
					{
						this.grabLockVectors[shots[i]] += dir.normalized * (this.grabLockSpeed * Time.deltaTime);
						rotationSlot = this.grabLockVectors[shots[i]];
					}
				}
				
				shots[i].transform.position = rotationSlot;
				shots[i].GetComponent<Shot>().IsActive = true;
			}
		}
		
		if(!this.isEnabled)
		{
			return;
		}
		
		if(this.grabbedShots.Count >= this.maxGrab)
		{
			this.Disable();
			return;
		}
		
		if(this.delayRadiusIncrease)
		{
			this.radiusIncreaseDelayState += 1.0f * Time.deltaTime;
			if(this.radiusIncreaseDelayState > this.radiusIncreaseDelay)
			{
				this.delayRadiusIncrease = false;
				this.radiusIncreaseDelayState = 0;
			}
		}
		
		if(!this.delayRadiusIncrease)
		{
			this.radiusIncreaseState += 1.0f * Time.deltaTime;
			if(this.radiusIncreaseState >= this.radiusIncreaseInterval)
			{
				this.ChangeRadius(this.radiusIncrease);
				this.radiusIncreaseState = 0;
				this.radiusIncreaseIndicator.renderer.enabled = false;
				this.delayRadiusIncrease = true;
			} 
			else
			{
				float range = (this.radiusIncreaseState / this.radiusIncreaseInterval);
				this.radiusIncreaseIndicator.renderer.enabled = true;
				this.radiusIncreaseIndicator.transform.localScale = this.resource.transform.localScale * range;
			}
		}
		
		if(this.colliderRadius >= this.colliderRadiusMax)
		{
			return;
		}
		
		var collider = this.resource.GetComponent<SphereCollider>();		
		IList<GameObject> pending = new List<GameObject>(this.pendingForGrab.Keys);
		IList<GameObject> active = GameObject.Find("GameManager").GetComponent<GameManager>().GetShotsWithin(this.resource.transform.position + collider.center, this.colliderRadius);
		foreach(GameObject shot in active)
		{
			if(!this.pendingForGrab.ContainsKey(shot))
			{
				this.pendingForGrab.Add(shot, 0);
				continue;
			}
			else
			{
				pending.Remove(shot);
			}
			
			this.pendingForGrab[shot] += Time.deltaTime;
			shot.GetComponent<Shot>().ChangeTarget(this.resource.transform.position);
			shot.GetComponent<Shot>().ChangeSpeed((0.5f + this.scaleGrabAccelleration) * Time.deltaTime);
			
			if(this.pendingForGrab[shot] >= (this.grabTime + this.massGrabTimeAdd + this.scaleGrabTimeAdd))
			{				
				this.Grab(shot);
				continue;
			}
			
			if(shot.GetComponent<Shot>().LifeTime <= 0)
			{
				this.pendingForGrab.Remove(shot);
			}
		}
		
		foreach(GameObject shot in pending)
		{
			this.pendingForGrab.Remove(shot);
		}
	}
	
	public void Grab(GameObject shot)
	{
		Vector3 origin = shot.transform.position;
		this.pendingForGrab.Remove(shot);
		shot.GetComponent<Shot>().Terminate();
		
		var grabbed = Shot.Create(this.resourceGrabbed);
		shot.name = "Gravity Grabbed Shot";
		shot.GetComponent<Shot>().Initialize(Vector3.up, Vector3.zero, 0, 0);
		shot.GetComponent<Shot>().SetCollision(2.0f, 0.1f);
		shot.GetComponent<Shot>().Source = this.Source;
		this.grabbedShots.Add(grabbed);
		this.grabLockVectors.Add(grabbed, origin);
		
		this.massGrabTimeAdd = this.grabbedShots.Count  / 20.0f;
	}
}
