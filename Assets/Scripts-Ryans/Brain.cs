using UnityEngine;
using System.Collections;

public class Brain : MonoBehaviour {


	GameObject [] enemies;
	GameObject [] ammo;
	GameObject [] fuel;

	public GameObject target;
	public GameObject close_Fuel;
	public GameObject close_Ammo;

	public int cur_Ammunition;
	public float cur_Fuel;

	float mass;
	float maxSpeed;
	float maxRange;
	float angle;
	float fov;
	float timeShot;
	public int mode;

	bool attack;

	Vector3 velocity = Vector3.zero;
	Vector3 force = Vector3.zero;
	Vector3 acceleration = Vector3.zero;

	public Vector3 patrol;
	public Vector3 pointOne;
	public Vector3 pointTwo;

	void Start () 
	{	
		mass = 1f;
		maxSpeed = Random.Range (5, 12);
		maxRange = 10;
		fov = Mathf.PI / 4.0f;
		timeShot = 0f;
		mode = 1;

		attack = false;

		cur_Ammunition = 10;
		cur_Fuel = 20;

		renderer.material.color = Color.blue;

		pointOne = new Vector3 (Random.Range (1,50),Random.Range (1,50),Random.Range (1,50));
		pointTwo = new Vector3 (Random.Range (1,50),Random.Range (1,50),Random.Range (1,50));

		patrol = pointOne;

		enemies = GameObject.FindGameObjectsWithTag ("Fighter");
		ammo = GameObject.FindGameObjectsWithTag ("Ammo");
		fuel = GameObject.FindGameObjectsWithTag ("Fuel");
	}

	Vector3 Seek(Vector3 targetPos)
	{
		Vector3 desiredVelocity;
		
		desiredVelocity = targetPos - transform.position;
		desiredVelocity.Normalize();
		desiredVelocity *= maxSpeed;
		return (desiredVelocity - velocity);
	}

	Vector3 Pursuit(Vector3 targetPos)
	{
		Vector3 toTarget = targetPos - transform.position;
		float dist = toTarget.magnitude;
		float time = dist / maxSpeed;
		
		Vector3 intercept_At = targetPos + (time * target.GetComponent<Brain>().velocity);
		
		return Seek(targetPos);
	}


	void FindTarget()
	{

		for(int i = 0;i<enemies.Length;i++)
		{
		if ((enemies[i].transform.position - transform.position).magnitude < maxRange) //Look for all targets in Range
		{
				if(enemies[i]!=gameObject)
				{
					target = enemies[i];
				
					Vector3 toTarget = (target.transform.position - transform.position);
					toTarget.Normalize();
					angle = (float) Mathf.Acos(Vector3.Dot(toTarget, transform.forward));
					if (angle < fov) //Only attack the one you can see.
					{
						attack=true;
						mode = 2;
					}
				}
		}
		}
	}

	void FindFuel()
	{
		for(int i =0; i<fuel.Length;i++)
		{
			if ((fuel[i].transform.position - transform.position).magnitude < (fuel[Random.Range (0,fuel.Length)].transform.position - transform.position).magnitude) //Look for something a little closer
			{
				if(fuel[i].GetComponent<Health>().status==1)
				{
				close_Fuel = fuel[i];
				i=fuel.Length;
					mode=4;
				}
			}
		}
	}

	void FindAmmo()
	{
		for(int i =0; i<ammo.Length;i++)
		{
			if ((ammo[i].transform.position - transform.position).magnitude < (ammo[Random.Range (0,ammo.Length)].transform.position - transform.position).magnitude) //Look for something a little closer
			{
				if(ammo[i].GetComponent<Ammo>().status==1)
				{
				close_Ammo = ammo[i];
				i=ammo.Length;
				}
			}
		}
	}

	void KillIt()
	{
		if (angle < fov)
		{
			if (timeShot > 0.25f)
			{
				if(cur_Ammunition>0)
				{
				GameObject lazer = new GameObject();
				lazer.AddComponent<Lazer>();
				lazer.transform.position = transform.position;
				lazer.transform.forward = transform.forward;
				timeShot = 0.0f;
				cur_Ammunition-=1;
				}
			}
		}
		if (angle > fov) //Lost Target, Return to patrol
		{
			attack=false;
			mode=1;
		}
	}

	void patrolPath() // Swap between two points
	{
			
			float epsilon = 5.0f;
			float dist = (patrol-transform.position).magnitude;
			if (dist < epsilon)
			{
					if(patrol == pointOne)
				{
					patrol = pointTwo;
				}
				else if(patrol == pointTwo)
				{
					patrol = pointOne;
				}
			}
	}

	void Update () 
	{
		timeShot += Time.deltaTime;

		cur_Fuel -= Time.deltaTime;

		patrolPath ();//check patrol path

		if (attack==false)
		{
			FindTarget ();
		}

	
		if (close_Ammo ==null)
		{
			FindAmmo ();
		}

		if(cur_Ammunition<=0)
		{
			FindAmmo ();
			mode=3;
		}
		if(cur_Fuel<=5)
		{
			FindFuel ();

		}
		
		if(attack==true)
		{
			KillIt();
		}

		if(mode==1)//patrol
		{
			force = Seek(patrol);
		}
		if(mode==2)//attack
		{
			force = Pursuit(target.transform.position);
		}
		if(mode==3)//seek ammunition
		{
			force = Seek (close_Ammo.transform.position);
		}
		if(mode==4)//seek fuel
		{
			force = Seek(close_Fuel.transform.position);
		}

		acceleration = force / mass;
		velocity += acceleration * Time.deltaTime;
		float speed = velocity.magnitude;
		if (speed > maxSpeed)
		{
			velocity.Normalize();            
			velocity *= maxSpeed;
		}
		transform.position += velocity * Time.deltaTime;

		if (speed > float.Epsilon)
		{
			transform.forward = velocity;
		}
		force = Vector3.zero;	
	}
}
