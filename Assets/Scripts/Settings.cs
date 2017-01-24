using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour {

	// Use this for initialization
	void Awake () {

		if(SceneManager.GetActiveScene().name == "vr"){
			VRSettings.enabled = true;
			Debug.Log("VR enabled");
			}
		else{
			VRSettings.enabled = false;
			Debug.Log("VR disabled");
			}
	}
	
	// Update is called once per frame
	void Update () {

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
