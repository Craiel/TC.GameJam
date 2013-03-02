using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour, IObserver {
	
	public GameObject m_LevelRootNode = null;
	
	public List<LevelModule> m_LevelModules = null;
	public int m_NumVisibleModules = 3; 
	
	private List<LevelModule> m_SpawnedLevelModules = new List<LevelModule>();	
	
	// Use this for initialization
	void Start () 
	{	
		SpawnModules();
	}
	
	public void OnNotify(Subject subject, object args)
	{
		if(subject is LevelModule)
		{
			m_SpawnedLevelModules.Remove(subject as LevelModule);
			Destroy(subject.gameObject);
			SpawnModules();
		}
	}	
			
	private void SpawnModules()
	{
		if(m_SpawnedLevelModules.Count == m_NumVisibleModules)
		{
			return;
		}
		else
		{
			while(m_SpawnedLevelModules.Count < m_NumVisibleModules)
			{
				LevelModule newModule = Instantiate(m_LevelModules[Random.Range(0, m_LevelModules.Count)]) as LevelModule;
				newModule.transform.parent = m_LevelRootNode.transform;
				newModule.transform.localRotation = Quaternion.identity;
				if(m_SpawnedLevelModules.Count > 0)
				{
					newModule.transform.localPosition = new Vector3(0,0, m_SpawnedLevelModules[m_SpawnedLevelModules.Count-1].transform.localPosition.z + (m_SpawnedLevelModules[m_SpawnedLevelModules.Count-1].m_PieceLength + newModule.m_PieceLength)/2.0f);
				}
				else
				{
					newModule.transform.localPosition = Vector3.zero;
				}
				m_SpawnedLevelModules.Add(newModule);
				newModule.RegisterObserver(this);
			}
			
		}
	}
}
