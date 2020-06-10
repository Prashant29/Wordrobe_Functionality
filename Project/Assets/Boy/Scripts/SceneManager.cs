using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public GameObject characterPrefab;
	public Transform spawnPos;

	GameObject playerCharacter;

	// Use this for initialization
	void Start () {

		// instantiate girl character on spawn position
		playerCharacter = Instantiate( characterPrefab, spawnPos.position, spawnPos.rotation ) as GameObject;


		StartCoroutine( "LoadSavedDresses" );

	}

	IEnumerator LoadSavedDresses() {

		yield return null;

		// load saved data and change body parts.
		playerCharacter.GetComponent<Character>().LoadSavedDresses();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
