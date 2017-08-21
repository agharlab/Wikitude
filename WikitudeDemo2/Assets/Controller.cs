using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class Controller : MonoBehaviour {

	public InstantTracker Tracker;
	private float heightAboveGround = 1.0f;  // how height to have the object projected above the background
	private GridRenderer grid;
	private bool isTracking = false;
	public Button trackingControl;
	public GameObject allosaurusPrefab;

	public void Awake()
	{
		grid = GetComponent<GridRenderer> ();
	}

	public void ToggleTracker()
	{
		isTracking = !isTracking;

		if (isTracking) {
			Tracker.SetState (InstantTrackingState.Tracking);
		} else {
			Tracker.SetState (InstantTrackingState.Initializing);
		}
	}

	public virtual void onErrorLoading(int errorCode, string errorMessage)
	{
		Debug.LogError ("Error loading URL Resource!\nErrorCode: " + errorCode + "\nErrorMessage: " + errorMessage);
	}

	public virtual void OnTragetsLoaded()
	{
		Debug.Log ("Tragets loaded successfully.");
	}

	public void OnEnterFieldOfVision(string target)
	{
		Debug.Log ("Enter Field of Vision");
		SetScene (true);
	}

	public void OnExitFieldOfVision(string target)
	{
		Debug.Log ("Exit Field of Vision");
		SetScene (false);
	}

	public void OnStateChanged(InstantTrackingState newState)
	{
		Debug.Log ("State Changes to " + newState);

		Tracker.DeviceHeightAboveGround = heightAboveGround;
		if (newState == InstantTrackingState.Tracking)
		{
			trackingControl.GetComponent<Image> ().color = Color.green;
			isTracking = true;
		}
		else if (newState == InstantTrackingState.Initializing)
		{
			trackingControl.GetComponent<Image> ().color = Color.red;
			isTracking = false;
		}
	}

	public void OnInitializationStarted()
	{
		Debug.Log ("Initialization started\n");
	}

	public void OnInitializationStopped()
	{
		Debug.Log ("Initialization stopped\n");
	}


	private void SetScene(bool status)
	{
//		grid.enabled = status;
		grid.enabled = false;

		GameObject[] allosaurus = GameObject.FindGameObjectsWithTag ("allosaurus");
		foreach (GameObject a in allosaurus)
		{
			Renderer[] rends = a.GetComponentsInChildren<Renderer> ();
			foreach (Renderer r in rends)
				r.enabled = status;
		}

		GameObject[] bullets = GameObject.FindGameObjectsWithTag ("bullet");
		foreach (GameObject b in bullets)
		{
			b.GetComponent<Renderer> ().enabled = status;
		}
	}


	// Use this for initialization
	void Start ()
	{
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isTracking)
			return;

//		if (Input.GetTouch (0).tapCount == 2)
		if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			sphere.transform.position = Camera.main.transform.position;
			sphere.AddComponent<Rigidbody> ();
			sphere.GetComponent<Rigidbody> ().AddForce (Camera.main.transform.forward * 100);
			sphere.tag = "bullet";

			Destroy (sphere, 5);
		}
		else if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began)
		{
			var cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
			float touchPos;
			if (groundPlane.Raycast (cameraRay, out touchPos))
			{
				Vector3 position = cameraRay.GetPoint (touchPos);
				Instantiate (allosaurusPrefab, position, Quaternion.identity);
			}
		}
	}
}
