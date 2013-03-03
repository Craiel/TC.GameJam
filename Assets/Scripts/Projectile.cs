using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public Transform m_BulletPrefab;
	public Transform m_ExplosionPrefab;
	public float m_ExplosionTime;
	
	private GameObject m_Visual;
	
	void Start()
	{
		var visualTransform = Instantiate( m_BulletPrefab, transform.position, transform.rotation ) as Transform;
		m_Visual = visualTransform.gameObject;
		m_Visual.transform.parent = this.transform;
	}
	
	void Update()
	{
	}
	
	public void Explode()
	{
		DestroyObject( m_Visual );
		var visualTransform = Instantiate( m_ExplosionPrefab, transform.position, transform.rotation ) as Transform;
		m_Visual = visualTransform.gameObject;
		m_Visual.transform.parent = this.transform;
	}

}
