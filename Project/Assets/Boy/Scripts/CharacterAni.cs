using UnityEngine;
using System.Collections;

public class CharacterAni : MonoBehaviour {

	Animator anim;

	int aniNum = 1;

	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animator>();

	}

	// to adjust button position
// 	int offsetX = 10;
// 	int offsetY = 30;


	void OnGUI() {

		// create animation change button. 
		if ( GUI.Button( new Rect( 60, 370, 140, 40 ), "Change Animation" ) ) {
			
			if ( aniNum < 13 )
				aniNum++;
			else
				aniNum = 1;
			
			anim.SetInteger( "aniKind", aniNum );
			
		}
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
