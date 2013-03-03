using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControlTest : MonoBehaviour 
{
	private const int TestEnemyCount = 10;
	private const int MaxShots = 500;
	
	private GameObject[] testEnemies = new GameObject[TestEnemyCount];
	private IList<GameObject> shots = new List<GameObject>();
		
	public GameObject Player;
	public GameObject Gui;
	
	public GameObject ShotHolder;
	public GameObject EnemyHolder;
	
	private int score;
	
	public void AddShot(GameObject newShot)
	{
		newShot.transform.parent = this.ShotHolder.transform;
		if(this.shots.Count > MaxShots)
		{
			var oldShot = this.shots[0];
			this.shots.RemoveAt(0);
			DestroyObject(oldShot);
		}
		
		newShot.GetComponent<Shot>().IsActive = true;
		this.shots.Add(newShot);
	}
	
	public IList<GameObject> GetShotsWithin(Vector3 center, float radius)
	{
		IList<GameObject> result = new List<GameObject>();
		IList<GameObject> list = new List<GameObject>(this.shots);
		for(int i=0;i<list.Count;i++)
		{
			if(list[i].GetComponent<Shot>().LifeTime <= 0 || list[i].GetComponent<Shot>().Source == ShotSource.Friend)
			{
				continue;
			}
		
			if((list[i].collider.bounds.center - center).magnitude < radius)
			{
				result.Add(list[i]);
			}
			
			/*if(bounds.Contains(list[i].collider.bounds.center) || bounds.Intersects(list[i].collider.bounds))
			{
				result.Add(list[i]);
			}*/
		}
		
		return result;
	}
		
	// Use this for initialization
	void Start () 
	{			
		for(int i=0;i<TestEnemyCount;i++)
		{
			this.testEnemies[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.testEnemies[i].transform.parent = this.EnemyHolder.transform;
			this.testEnemies[i].AddComponent<Enemy>();
			this.testEnemies[i].AddComponent<BoxCollider>();
			this.testEnemies[i].GetComponent<BoxCollider>().isTrigger = true;
			this.testEnemies[i].AddComponent<Rigidbody>();
			this.testEnemies[i].GetComponent<Rigidbody>().useGravity = false;
			this.testEnemies[i].GetComponent<Rigidbody>().isKinematic = true;
			this.testEnemies[i].GetComponent<Enemy>().CollisionDamage = Random.value * 5;
			this.testEnemies[i].GetComponent<Enemy>().CollisionInterval = 0.1f;			
			this.testEnemies[i].name = "Enemy "+i;
			
			var weapon = WeaponSingleDumbFire.Create();
			weapon.GetComponent<Weapon>().Cooldown = 2f;
			weapon.GetComponent<Weapon>().Source = ShotSource.Foe;
			this.testEnemies[i].GetComponent<Enemy>().AddWeapon(weapon);
			
			this.ResetEnemy(i);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i=0;i<TestEnemyCount;i++)
		{
			Enemy current = this.testEnemies[i].GetComponent<Enemy>();
			if(current.IsDead)
			{
				this.score += (int)current.MaxHealth;
			}
			
			if(current.LifeTime <= 0 || current.IsDead)
			{
				this.ResetEnemy(i);
				continue;
			}
			
			if(Random.value < 0.2f)
			{
				current.Fire(true, (this.Player.transform.position - current.transform.position).normalized);
			}
		}
		
		IList<GameObject> activeShots = new List<GameObject>(this.shots);
		foreach(GameObject obj in activeShots)
		{			
			Shot current = obj.GetComponent<Shot>();
			if(current.LifeTime <= 0)
			{				
				this.shots.Remove(obj);
				DestroyObject(obj);
			}
		}
		
		//print(this.Player.GetComponent<Player>().Health);
		this.Gui.GetComponent<GUIText>().text = string.Format("Health: {0}, Score: {1}", this.Player.GetComponent<Player>().Health, this.score);
	}
	
	private void ResetEnemy(int slot)
	{
		float pos = (Random.value -0.5f) * 24;
		this.testEnemies[slot].GetComponent<Enemy>().Initialize(new Vector3(pos, 10, 0), Random.value * 15.0f, 3.0f + Random.value * 4.0f);
		this.testEnemies[slot].GetComponent<Enemy>().SetCollision(5.0f, 0.2f);
		this.testEnemies[slot].GetComponent<Enemy>().Health = 1.0f + Random.value * 10.0f;
	}
}
