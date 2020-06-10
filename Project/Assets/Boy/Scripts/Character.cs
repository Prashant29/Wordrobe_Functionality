using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// define dress types 
public enum DressKind {

	hair, upbody, downbody, glass,eyes,shoe
}

public class Character : MonoBehaviour {


	public static Character instance = null;
	// keys to save selected body parts information
	string KEY_HAIR = "hairNum";
	string KEY_UPPER = "upperNum";
	string KEY_DOWN = "downNum";
	string KEY_GLASS ="glassNum";
    string KEY_Eye = "EyeNum";
    string KEY_SHOE = "shoeNum";

	// create list to store all of body parts
	List<GameObject> arrHairs = new List<GameObject>();
	List<GameObject> arrUpbodies = new List<GameObject>();
	List<GameObject> arrDownbodies = new List<GameObject>();
	List<GameObject> arrGlassies = new List<GameObject>();
    List<GameObject> arrEyes = new List<GameObject>();
    List<GameObject> arrshoes = new List<GameObject>();

	// to manage Transform Components of each body parts
	Transform hairGroup;
	Transform upbodyGroup;
	Transform downGroup;
	Transform glassGroup;
    Transform eyesGroup;
    Transform shoeGroup;

	// to find out current selected body part count
	int hairNum = 0;
	public int upperNum = 0;
	public int downNum = 0;
	int glassNum = 0;
    int EyeNum = 0;
    public int shoeNum = 0;
	// Use this for initialization
	void Awake () {

		if (instance == null) instance = this;

		// to store all of the body parts
		hairGroup = transform.Find("hairGroup");
		upbodyGroup = transform.Find("upbodyGroup");
		downGroup = transform.Find("downBodyGroup");
		glassGroup = transform.Find("glassGroup");
        eyesGroup = transform.Find("eyesGroup");
        shoeGroup = transform.Find("shoeGroup");
	}
	
	void Start() {

		// store each body part to each list
		MakeDress( hairGroup, arrHairs );
		MakeDress( upbodyGroup, arrUpbodies );
		MakeDress( downGroup, arrDownbodies );
        MakeDress(eyesGroup, arrEyes);
        MakeDress(shoeGroup, arrshoes);

		arrGlassies.Add( null );

		MakeDress( glassGroup, arrGlassies );
		
//		hairNum = Random.Range( 0, arrHairs.Count );
//		upperNum = Random.Range( 0, arrUpbodies.Count );
//		skirtNum = Random.Range( 0, arrSkirts.Count );

		InitDresses();
	}



	// initialize basic body parts(basic dresses)
	public void InitDresses() {

		// hide all body parts and show selected body part
		ShowDress( arrHairs, hairNum );
		ShowDress( arrUpbodies, upperNum );
		ShowDress( arrDownbodies, downNum );
		ShowDress( arrGlassies, glassNum );
        ShowDress(arrEyes, EyeNum);
        ShowDress(arrshoes, shoeNum);
	}

	// store each body part to associated list 
	void MakeDress( Transform dressGroup, List<GameObject> dressList) {
		
		foreach( Transform dress in dressGroup ) {
			
			dressList.Add(dress.gameObject);
			dress.gameObject.SetActive(false);
			
		}
		
	}

	// hide all the parts and show selected part
	void ShowDress( List<GameObject> dressList, int dressNumber ) {

		Debug.Log("Dress Number: " + dressNumber);

		for (int i = 0; i < dressList.Count; i++) {
			

			if (dressList[i] != null )
				dressList[i].SetActive(false);
			
		}

		if (dressList[dressNumber] != null )
			dressList[dressNumber].SetActive(true);
	}

	// automatically change selected body parts.
	// if 'isBackward' is true, it'll show previous body part.
	public void ChangeDress( DressKind dKind, bool isBackward = false ) {

		switch( dKind ) {

			case DressKind.hair:
				ChangeNextDress( arrHairs, ref hairNum, isBackward );
				break;

			case DressKind.upbody:
			ChangeNextDress( arrUpbodies, ref upperNum, isBackward );
				break;

			case DressKind.downbody:
			ChangeNextDress( arrDownbodies, ref downNum, isBackward );
				break;

			case DressKind.glass:
			ChangeNextDress( arrGlassies, ref glassNum, isBackward );
			break;

            case DressKind.eyes:
            ChangeNextDress(arrEyes, ref EyeNum, isBackward);
            break;

            case DressKind.shoe:
            ChangeNextDress( arrshoes, ref shoeNum, isBackward);
            break;
		}



	}

	// show the body part from suggested list.
	void ChangeNextDress( List<GameObject> dressList, ref int dressNum, bool isBackward = false ) {

		if (isBackward) {

			if ( dressNum > 0 ) 
				dressNum--;
			else
				dressNum = dressList.Count - 1;

		} else {

			if ( dressNum < dressList.Count - 1) 
				dressNum++;
			else
				dressNum = 0;
		}

		ShowDress( dressList, dressNum );
	}

	// return current selected body part name 
	public string GetCurrentDressName( DressKind dKind ) {

		string dressName = string.Empty;

		switch( dKind ) {
			
			case DressKind.hair:
				dressName = arrHairs[hairNum].name;
				break;
				
			case DressKind.upbody:
				dressName = arrUpbodies[upperNum].name;
				break;
				
			case DressKind.downbody:
				dressName = arrDownbodies[downNum].name;
				break;

            case DressKind.eyes:
                dressName = arrEyes[EyeNum].name;
                break;

            case DressKind.shoe:

                dressName = arrshoes[shoeNum].name;
                break;

			case DressKind.glass:
				dressName = arrGlassies[glassNum] != null ? arrGlassies[glassNum].name : "empty" ;
				break;

         
		}

		return dressName;

	}

	// save each number of selected body part
	public void SaveCurrentDresses() {
	
		PlayerPrefs.SetInt( KEY_HAIR, hairNum );
		PlayerPrefs.SetInt( KEY_UPPER, upperNum );
		PlayerPrefs.SetInt( KEY_DOWN, downNum );
		PlayerPrefs.SetInt( KEY_GLASS, glassNum );
        PlayerPrefs.SetInt(KEY_Eye, EyeNum);
        PlayerPrefs.SetInt(KEY_SHOE, shoeNum);

	}

	// load each number of selected body part
	public void LoadSavedDresses() {

		hairNum = PlayerPrefs.GetInt( KEY_HAIR );
		upperNum = PlayerPrefs.GetInt( KEY_UPPER );
		downNum = PlayerPrefs.GetInt( KEY_DOWN );
		glassNum = PlayerPrefs.GetInt( KEY_GLASS );
        EyeNum = PlayerPrefs.GetInt(KEY_Eye);
        shoeNum = PlayerPrefs.GetInt(KEY_SHOE);

		InitDresses();

	}

	// Update is called once per frame
	void Update () {
		
	}
}
