using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour {

	public float FloatStrenght;
	public float RandomRotationStrenght;
	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {

		if(UDPReceive.isFloating)
			FloatStrenght = 9f;
		else
			FloatStrenght = 0f;

		transform.GetComponent<Rigidbody>().AddForce(Vector3.up * FloatStrenght);
		transform.Rotate(RandomRotationStrenght,RandomRotationStrenght,RandomRotationStrenght);
	}
}
