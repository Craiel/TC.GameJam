using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControlTest : MonoBehaviour 
{
	private const int TestEnemyCount = 30;
	private const int MaxShots = 100;
	
	private GameObject[] testEnemies = new GameObject[TestEnemyCount];
	private IList<GameObject> shots = new List<GameObject>();
	
	public GameObject Player;
	public GameObject Gui;
	
	private int score;
	
	public void AddShot(GameObject newShot)
	{
		if(this.shots.Count > MaxShots)
		{
			var oldShot = this.shots[0];
			this.shots.RemoveAt(0);
			DestroyObject(oldShot);
		}
		
		this.shots.Add(newShot);
	}
		
	// Use this for initialization
	void Start () 
	{	
		for(int i=0;i<TestEnemyCount;i++)
		{
			this.testEnemies[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.testEnemies[i].AddComponent<Enemy>();
			this.testEnemies[i].AddComponent<BoxCollider>();
			this.testEnemies[i].GetComponent<BoxCollider>().isTrigger = true;
			this.testEnemies[i].AddComponent<Rigidbody>();
			this.testEnemies[i].GetComponent<Rigidbody>().useGravity = false;
			this.testEnemies[i].GetComponent<Rigidbody>().isKinematic = true;
			this.testEnemies[i].GetComponent<Enemy>().CollisionDamage = Random.value * 5;
			this.testEnemies[i].GetComponent<Enemy>().CollisionInterval = 0.1f;
			this.testEnemies[i].name = "Enemy "+i;
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
		float pos = (Random.value -0.5f) * 36;
		print ("Spawning new enemy at "+pos);
		this.testEnemies[slot].GetComponent<Enemy>().Initialize(new Vector3(pos, 10, 0), Random.value * 15.0f, 3.0f + Random.value * 4.0f);
		this.testEnemies[slot].GetComponent<Enemy>().Health = 1.0f + Random.value * 10.0f;
	}
}
