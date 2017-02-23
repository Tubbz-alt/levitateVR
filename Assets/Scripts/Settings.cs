using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Settings : MonoBehaviour {

	GameObject rehabToggleObj;
	GameObject vrpnToggleObj;

	Toggle rehatoggle;
	Toggle vrpntoggle;

	bool rehapanel = true;

	// Use this for initialization
	void Awake () {

		//Makes the object target not be destroyed automatically when loading a new scene.
//		DontDestroyOnLoad(this.gameObject);
//		if (FindObjectsOfType(GetType()).Length > 1)
//		{
//			Destroy(this.gameObject);
//		}
//
//		if(SceneManager.GetActiveScene().name == "vr"){
//			VRSettings.enabled = true;
//			Debug.Log("VR enabled");
//			}
//		else{
//			VRSettings.enabled = false;
//			Debug.Log("VR disabled");
//			}
	}

	void Start () {

		// Data-source toggle buttons
		try{
			
			rehabToggleObj = GameObject.Find("rehabnet");
			vrpnToggleObj = GameObject.Find("vrpn");

			rehatoggle = rehabToggleObj.GetComponent<Toggle>();
			vrpntoggle = rehabToggleObj.GetComponent<Toggle>();

		}
		catch{
			
		}

	}
	
	// Update is called once per frame
	void Update () {

//		try{
//			rehatoggle.onValueChanged.AddListener((x) => rehapanel = x); //check if
//			//vrpntoggle.onValueChanged.AddListener((x) => rehapanel = x);
//			print(rehapanel);
//		}
//		catch{
//			print("no toggle");
//		}
//
		// move between different scens and main menu
		if(Input.GetKey(KeyCode.Escape)){
			
			if(SceneManager.GetActiveScene().name == "menu")
			{
				Application.Quit();
				Debug.Log("exit");
				}
			else{
				SceneManager.LoadScene(0);
				Debug.Log("main menu");
				}
			}
		}


}
