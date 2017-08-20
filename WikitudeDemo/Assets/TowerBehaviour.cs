using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour {

	public GameObject bullet;
	public Transform spawnPos;
	GameObject goob;

	void OnTriggerEnter(Collider obj)
	{
		if (obj.gameObject.tag == "goober" && goob == null) {
			goob = obj.gameObject;
			InvokeRepeating ("Shoot", 0, 1.0f);
		}
	}

	void Shoot()
	{
		Instantiate (bullet, spawnPos.position, spawnPos.rotation);
		this.GetComponent<AudioSource> ().Play ();
		if (goob.GetComponent<Move> ().dead) {
			goob = null;
			CancelInvoke ("Shoot");
		}
	}

	void OnTriggerExit(Collider obj)
	{
		if (obj.gameObject == goob)
		{
			goob = null;
			CancelInvoke ("Shoot");
		}
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (goob != null) {
			this.transform.LookAt (goob.transform.position);
		}
	}
}
