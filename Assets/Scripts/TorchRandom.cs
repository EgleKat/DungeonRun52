using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchRandom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Animator>().SetFloat("timeOffset", Random.Range(0.9f, 1.1f));
	}
}
