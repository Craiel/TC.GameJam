using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Subject : MonoBehaviour {
	
	private List<IObserver> m_ObserverList = new List<IObserver>();
	
	public virtual void RegisterObserver(IObserver observer)
	{
		if(!m_ObserverList.Contains(observer))
		{
			m_ObserverList.Add(observer);
		}
	}
	
	public virtual void NotifyObservers(object args = null)
	{
		foreach(IObserver observer in m_ObserverList)
		{
			observer.OnNotify(this, args);
		}
	}
		
}
