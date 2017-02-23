using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatTime : MonoBehaviour {

	float floatTime = 0f;
	public Text countText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(UDPReceive.isFloating){
		floatTime += Time.deltaTime;
		
		SetCountText ();
		}

	}

	void SetCountText ()
	{
		countText.text = floatTime.ToString("F2");
//		if (count >= 12)
//		{
//			winText.text = "You Win!";
//		}
	}

}
