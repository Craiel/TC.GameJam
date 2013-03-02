using UnityEngine;
using System.Collections;

public class LevelModule : Subject {
	
	public float m_PieceLength = 0.0f;
		
	// Update is called once per frame
	void Update () 
	{	
		if(this.transform.position.y < -m_PieceLength)
		{
			NotifyObservers("OutOfScreen");
		}
	}
	
	
}
