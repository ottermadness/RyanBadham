﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	//ITS FUEL!!

	GameObject[]giveToo;
	
	public int status;
	float timer;
	
	void Start () 
	{
		timer = 0;
		status = 1;
		renderer.material.color = Color.green;
		giveToo = GameObject.FindGameObjectsWithTag ("Fighter");
	}
	
	void OnTriggerEnter(Collider guy)
	{
		if(status == 1)
		{
			if(guy.gameObject.tag=="Fighter")
			{
				guy.GetComponent<Brain>().cur_Fuel=20;
				guy.GetComponent<Brain>().mode = 1;
				for(int i =0;i>giveToo.Length;i++)
				{
					giveToo[i].GetComponent<Brain>().close_Fuel=null;
				}
				renderer.material.color = Color.clear;
				status = 0;
				timer=0;
			}
		}
	}
	
	void Update()
	{
		timer += Time.deltaTime;
		
		if (timer == 10)
		{
			status = 1;
			renderer.material.color = Color.green;
		}
	}

}
