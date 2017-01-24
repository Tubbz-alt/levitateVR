using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public Button menuButton;

	void Start () {
		Button btn = menuButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
		if(menuButton.name == "main")
			SceneManager.LoadScene (1);
		if(menuButton.name == "vr")
			SceneManager.LoadScene (2);
		if(menuButton.name == "cave")
			SceneManager.LoadScene (3);
	}

}