using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	
	private Camera mainCamera;
	private Plane[] planes;
	
	private GameObject leftCollider;
	private GameObject rightCollider;
	private GameObject topCollider;
	private GameObject bottomCollider;
	
	private Vector3 lastGoodPosition;
	
	private float currentX;
	private float currentY;
	
	// ---------------------------------------------
	// Public
	// ---------------------------------------------
	public float Sensitivity = 0.0001f;
	public float BankingFactor = 10.0f;
	public float EdgeBuffer = 1.0f;
		
	// Use this for initialization
	void Start ()
	{
		// Get rid of the cursor and lock it
		Screen.lockCursor = true;
		Screen.showCursor = false;
		
		this.mainCamera = Camera.main;
		this.planes = GeometryUtility.CalculateFrustumPlanes(this.mainCamera);
		this.leftCollider = this.GetColliderPlane(this.planes[0]);
		this.rightCollider = this.GetColliderPlane(this.planes[1]);
		this.topCollider = this.GetColliderPlane(this.planes[2]);
		this.bottomCollider = this.GetColliderPlane(this.planes[3]);
	}	
	
	void OnCollisionEnter(Collision collision)
	{
		print("Colliding...");
		foreach (ContactPoint point in collision.contacts)
		{
			Debug.DrawRay(point.point, point.normal, Color.white);
		}		
	}
	
	// Update is called once per frame
	void Update ()
	{
		float newX = Input.GetAxis("Mouse X");
		float newY = Input.GetAxis("Mouse Y");
		
		if(newX == 0.0f && newY == 0.0f)
		{
			return;
		}
		
		float xMovement = newX * this.Sensitivity;
		float yMovement = newY * this.Sensitivity;
		
		print ("[0] = Stat: "+xMovement+"x"+yMovement+" | "+this.currentX+"x"+this.currentY+" | "+newX+"x"+newY);
		
		this.CheckBounds(ref xMovement, ref yMovement);
		
		print ("[1] = Stat: "+xMovement+"x"+yMovement+" | "+this.currentX+"x"+this.currentY+" | "+newX+"x"+newY);
		
		this.currentX += xMovement;
		this.currentY += yMovement;
		
		//transform.Rotate(Vector3.forward, newX * this.BankingFactor);
		//transform.Rotate(Vector3.left, newY * this.BankingFactor);
			
		transform.Translate(this.currentX, this.currentY, 0, Space.World);
	}
	
	private void CheckBounds(ref float xMovement, ref float yMovement)
	{
		if(xMovement < 0.0f)
		{
		Vector3 closestPoint = this.leftCollider.collider.ClosestPointOnBounds(transform.position);
		Vector3 relativePoint = transform.InverseTransformPoint(this.leftCollider.transform.position);		
		float distance = Vector3.Distance(this.leftCollider.transform.position, transform.position);
		print ("DistX: " + distance + " rel: "+relativePoint.x + " mov: "+xMovement);
		if((relativePoint.x > 0.0f || distance < this.EdgeBuffer) && xMovement < 0.0f)
		{
			print ("clipping!");
			xMovement = 0.0f;
		}
		}
		
		if(xMovement > 0.0f)
		{
		Vector3 closestPoint = this.rightCollider.collider.ClosestPointOnBounds(transform.position);
		Vector3 relativePoint = transform.InverseTransformPoint(this.rightCollider.transform.position);		
		float distance = Vector3.Distance(this.rightCollider.transform.position, transform.position);
		print ("-DistX: " + distance + " rel: "+relativePoint.x + " mov: "+xMovement);
		if((relativePoint.x < 0.0f || distance < this.EdgeBuffer) && xMovement > 0.0f)
		{
			print ("clipping!");
			xMovement = 0.0f;
		}
		}
		
		if(yMovement < 0.0f)
		{
		Vector3 closestPoint = this.topCollider.collider.ClosestPointOnBounds(transform.position);
		Vector3 relativePoint = transform.InverseTransformPoint(this.topCollider.transform.position);		
		float distance = Vector3.Distance(this.topCollider.transform.position, transform.position);
		print ("DistY: " + distance + " rel: "+relativePoint.y + " mov: "+yMovement);
		if((relativePoint.y > 0.0f || distance < this.EdgeBuffer) && yMovement < 0.0f)
		{
			print ("clipping!");
			yMovement = 0.0f;
		}
		}
		
		if(yMovement > 0.0f)
		{
		Vector3 closestPoint = this.topCollider.collider.ClosestPointOnBounds(transform.position);
		Vector3 relativePoint = transform.InverseTransformPoint(this.topCollider.transform.position);		
		float distance = Vector3.Distance(this.topCollider.transform.position, transform.position);
		print ("-DistY: " + distance + " rel: "+relativePoint.y + " mov: "+yMovement);
		if((relativePoint.y < 0.0f || distance < this.EdgeBuffer) && yMovement > 0.0f)
		{
			print ("clipping!");
			yMovement = 0.0f;
		}
		}
	}
	
	// ---------------------------------------------
	// Private
	// ---------------------------------------------
	private GameObject GetColliderPlane(Plane plane)
	{
		GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
  		p.transform.position = -plane.normal * plane.distance;
  		p.transform.rotation = Quaternion.FromToRotation(Vector3.up, plane.normal);
  		p.transform.localScale = Vector3.one * 20.0f;
		return p;
	}
}
