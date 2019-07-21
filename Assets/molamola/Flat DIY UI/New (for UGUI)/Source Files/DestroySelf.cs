using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {

	// Use this for initialization
	public void OnDestroySelf() 
	{
		this.gameObject.SetActive(false);
	}
}
