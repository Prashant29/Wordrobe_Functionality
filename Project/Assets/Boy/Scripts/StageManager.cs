using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {

	public GameObject characterPrefab;
	public Transform spawnPos;

	public static GameObject myCharacter;

	// Use this for initialization
	void Start () {

		// instantiate girl character on the spawn position
		myCharacter = Instantiate( characterPrefab, spawnPos.position, spawnPos.rotation ) as GameObject;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
