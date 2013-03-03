using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponGravity : Weapon 
{
	private GameObject resource;
	private GameObject resourceGrabbed;
	
	private bool isEnabled;
	
	private IDictionary<GameObject, float> pendingForGrab = new Dictionary<GameObject, float>();
	private IList<GameObject> grabbedShots = new List<GameObject>();
	
	private float grabTime = 0.2f;
	private float angle = 0.0f;
	private float shotMoveSpeed = 1.0f;
	private float rotationSpace = 8.0f;
	private int maxGrab = 45;
	
	//private Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
	private Vector3 scale = new Vector3(1f, 1f, 1f);
	
	private float colliderRadius;
	
	private Vector3 rotationOffset = new Vector3(1.5f, 0, 0);
	
	void Start()
	{
		var wellResource = Resources.Load("GravityWell") as GameObject;
		this.resource = Instantiate(wellResource) as GameObject;
		this.resource.transform.parent = this.transform;
		this.resource.transform.localPosition = Vector3.zero;
		this.resource.renderer.enabled = false;
				
		this.resourceGrabbed = Resources.Load("DumbFireFriendly") as GameObject;
		this.colliderRadius = this.resource.GetComponent<SphereCollider>().radius;
	}
	
	public override void Fire()
	{
		if(this.isEnabled)
		{
			return;
		}
		
		print("Enabling Gravity well");
		this.ReleaseLeftOvers();
		this.isEnabled = true;
		this.resource.renderer.enabled = true;
	}
	
	private void ReleaseLeftOvers()
	{
		IList<GameObject> shots = new List<GameObject>(this.grabbedShots);
		this.grabbedShots.Clear();
		print ("Releasing shots: "+shots.Count);
		foreach(GameObject grabbed in shots)
		{
			Vector3 pos = grabbed.transform.position;
			Vector3 dir = (pos - this.transform.position).normalized;
			
			DestroyObject(grabbed);
			
			print (dir + " -- "+pos);
			var shot = Shot.Create(this.resourceGrabbed, true);
			//shot.transform.localScale = //this.scale * 8;
			shot.GetComponent<Shot>().Initialize(dir, pos, 7.0f, 30f);
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
		this.isEnabled = false;
		this.resource.renderer.enabled = false;		
	}
	
	public static GameObject Create()
	{
		var obj = new GameObject("WeaponGravity");
		obj.AddComponent<WeaponGravity>();
		return obj;
	}
	
	public void ChangeRadius(float change)
	{
		this.resource.transform.localScale += new Vector3(change, change, change);
		this.colliderRadius += change;
		if(this.colliderRadius < 0.0f)
		{
			this.resource.transform.localScale -= new Vector3(change, change, change);
			this.colliderRadius = 0;
		}
		
		this.grabTime = 0.2f + (this.colliderRadius / 40.0f);
	}
	
	public void Update()
	{
		this.angle += this.shotMoveSpeed;
		if(this.grabbedShots.Count > 0)
		{
			IList<GameObject> shots = new List<GameObject>(this.grabbedShots);
			for(int i=0;i<shots.Count;i++)
			{
				shots[i].transform.position = this.resource.transform.position;
				shots[i].transform.Translate(this.rotationOffset + new Vector3(this.colliderRadius / 4.0f, 0, 0));
				shots[i].transform.rotation = Quaternion.AngleAxis(this.angle + (i * this.rotationSpace), Vector3.forward);
				shots[i].GetComponent<Shot>().IsActive = true;
			}
		}
		
		if(!this.isEnabled)
		{
			return;
		}
		
		if(this.grabbedShots.Count >= this.maxGrab)
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
			shot.GetComponent<Shot>().ChangeSpeed(0.5f * Time.deltaTime);
			
			if(this.pendingForGrab[shot] >= this.grabTime)
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
		this.pendingForGrab.Remove(shot);
		shot.GetComponent<Shot>().Terminate();
		
		var grabbed = Shot.Create(this.resourceGrabbed);
		shot.name = "Gravity Grabbed Shot";
		shot.GetComponent<Shot>().Initialize(Vector3.up, Vector3.zero, 0, 0);
		shot.GetComponent<Shot>().SetCollision(1.0f, 0.1f);
		shot.GetComponent<Shot>().Source = this.Source;
		this.grabbedShots.Add(grabbed);
	}
}
