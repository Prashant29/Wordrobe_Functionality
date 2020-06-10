using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	// to adjust button position
	int offsetX = 10;
	int offsetY = 70;

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI() {

		// create button to change hair
		if ( GUI.Button( new Rect( offsetX, offsetY, 40, 40 ), "<" ) ) {
			
			StageManager.myCharacter.GetComponent<Character>().ChangeDress( DressKind.hair, true );
		}
		
		if ( GUI.Button( new Rect( offsetX + 200, offsetY, 40, 40 ), ">" ) ) {
			
			StageManager.myCharacter.GetComponent<Character>().ChangeDress( DressKind.hair, false );
		}
		
		// create button to change upper body parts
		if ( GUI.Button( new Rect( offsetX, offsetY + 50, 40, 40 ), "<" ) ) {
			
			StageManager.myCharacter.GetComponent<Character>().ChangeDress( DressKind.upbody, true );
		}
		
		if ( GUI.Button( new Rect( offsetX + 200, offsetY + 50, 40, 40 ), ">" ) ) {
			
			StageManager.myCharacter.GetComponent<Character>().ChangeDress( DressKind.upbody, false );
			
		}
		
		// // create button to change bottoms
		if ( GUI.Button( new Rect( offsetX, offsetY + 100, 40, 40 ), "<" ) ) {
			
			StageManager.myCharacter.GetComponent<Character>().ChangeDress( DressKind.downbody, true );
		}
		
		if ( GUI.Button( new Rect( offsetX + 200, offsetY + 100, 40, 40 ), ">" ) ) {
			
			StageManager.myCharacter.GetComponent<Character>().ChangeDress( DressKind.downbody, false );
		}

		// create button to change glasses
		if ( GUI.Button( new Rect( offsetX, offsetY + 150, 40, 40 ), "<" ) ) {
			
			StageManager.myCharacter.GetComponent<Character>().ChangeDress( DressKind.glass, true );
		}
		
		if ( GUI.Button( new Rect( offsetX + 200, offsetY + 150, 40, 40 ), ">" ) ) {
			
			StageManager.myCharacter.GetComponent<Character>().ChangeDress( DressKind.glass, false );
		}


        if (GUI.Button(new Rect(offsetX, offsetY + 200, 40, 40), "<"))
        {

            StageManager.myCharacter.GetComponent<Character>().ChangeDress(DressKind.eyes, true);
        }

        if (GUI.Button(new Rect(offsetX + 200, offsetY + 200, 40, 40), ">"))
        {

            StageManager.myCharacter.GetComponent<Character>().ChangeDress(DressKind.eyes, false);
        }

        if (GUI.Button(new Rect(offsetX, offsetY + 250, 40, 40), "<"))
        {

            StageManager.myCharacter.GetComponent<Character>().ChangeDress(DressKind.shoe, true);
        }


        if (GUI.Button(new Rect(offsetX + 200, offsetY + 250, 40, 40), ">"))
        {

            StageManager.myCharacter.GetComponent<Character>().ChangeDress(DressKind.shoe, false);
        }



		// create labels to show the name of current selected body part
		string hairName = StageManager.myCharacter.GetComponent<Character>().GetCurrentDressName(DressKind.hair);
		string upperName = StageManager.myCharacter.GetComponent<Character>().GetCurrentDressName(DressKind.upbody);
		string downName = StageManager.myCharacter.GetComponent<Character>().GetCurrentDressName(DressKind.downbody);
		string glassName = StageManager.myCharacter.GetComponent<Character>().GetCurrentDressName(DressKind.glass);
        string EyesName = StageManager.myCharacter.GetComponent<Character>().GetCurrentDressName(DressKind.eyes);
        string ShoeName = StageManager.myCharacter.GetComponent<Character>().GetCurrentDressName(DressKind.shoe);
		GUI.Box( new Rect ( 60, offsetY, 140, 40 ), hairName );
		GUI.Box( new Rect ( 60, offsetY + 50, 140, 40 ), upperName );
		GUI.Box( new Rect ( 60, offsetY + 100, 140, 40 ), downName );
		GUI.Box( new Rect ( 60, offsetY + 150, 140, 40 ), glassName );
        GUI.Box(new Rect(60, offsetY + 200, 140, 40), EyesName);
        GUI.Box(new Rect(60, offsetY + 250, 140, 40), ShoeName);
		// create button to save current state and move to Scene2
       
		if ( GUI.Button( new Rect( 60, offsetY + 350, 140, 40 ), "Save Configuration" ) ) {

			StageManager.myCharacter.GetComponent<Character>().SaveCurrentDresses();
			Application.LoadLevel("Scene2");
		}
     
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
