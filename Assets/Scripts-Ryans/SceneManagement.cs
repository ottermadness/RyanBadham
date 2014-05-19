using UnityEngine;
using System.Collections;

public class SceneManagement : MonoBehaviour {

	public GameObject ammo;
	public GameObject fighter;
	public GameObject health;

	public int ammo_Supply;
	public int fighter_Amount;
	public int health_Supply;

	void Start () 
	{

		for(int i = 0; i<fighter_Amount;i++)
		{
			Instantiate (fighter,new Vector3(Random.Range (1,50),Random.Range (1,50),Random.Range (1,50)),Quaternion.identity);
		}

		for(int i = 0; i<ammo_Supply;i++)
		{
			Instantiate (ammo,new Vector3(Random.Range (1,50),Random.Range (1,50),Random.Range (1,50)),Quaternion.identity);
		}

		for(int i = 0; i<fighter_Amount;i++)
		{
			Instantiate (health,new Vector3(Random.Range (1,50),Random.Range (1,50),Random.Range (1,50)),Quaternion.identity);
		}
	}
	

	void Update () 
	{
	
	}
}
