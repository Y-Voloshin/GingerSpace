using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour {
	[SerializeField]
	GameObject FinishForm;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ExecuteFinish()
	{
		if (FinishForm != null)
			FinishForm.SetActive (true);
	}
}
