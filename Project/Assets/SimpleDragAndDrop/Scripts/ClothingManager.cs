using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class ClothingManager : MonoBehaviour
{
    public static ClothingManager instance = null;

    [Header("Screens")]
    public GameObject scene;
    public GameObject StartCanvas, GameCanvas;


    public GameObject WarningObject;

    public Animator anim;

    public bool isSummer, isRainy, isSummerCompleted;

    public RectTransform handIndicator, clothIndicator;
    public Animator indicatorAnim;

    public Image[] topWear, bottomWear, footWear, headWear, rainWear;

    public GameObject InstructionPanel;


    private void Awake()
    {
        if (instance == null)
            instance = this;

    }

    private void Start()
    {
        isSummer = true;
        isRainy = false;
        isSummerCompleted = false;
        foreach (var top in topWear)
        {
            top.raycastTarget = false;
        }
        foreach (var b in bottomWear)
        {
            b.raycastTarget = false;
        }
        foreach (var h in headWear)
        {
            h.raycastTarget = false;
        }
        foreach (var f in footWear)
        {
            f.raycastTarget = false;
        }
        foreach (var r in rainWear)
        {
            r.raycastTarget = false;
        }
        handIndicator.gameObject.SetActive(false);
        clothIndicator.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        GameCanvas.SetActive(false);
        scene.SetActive(true);
        anim.SetTrigger("StartScene");
        StartCoroutine(LoadMyScene());
        isRainy |= isSummerCompleted;
        InstructionPanel.SetActive(false);

        Debug.Log("IsRainy: " + isRainy);
    }

    IEnumerator LoadMyScene()
    {
       
        while(StartCanvas.GetComponent<CanvasGroup>().alpha > 0)
        {
            yield return new WaitForSeconds(.05f);
            StartCanvas.GetComponent<CanvasGroup>().alpha -= .1f;
        }
        StartCanvas.SetActive(false);
    }

    public void ActiveGameCanvas()
    {
        InstructionPanel.SetActive(true);
        GameCanvas.SetActive(true);
        
    }

    public void StartSunnyDay()
    {
        WarningObject.GetComponentInChildren<Text>().text = "Today it is Hot and Sunny.\nWhat should Ron wear?";
        WarningObject.SetActive(true);
        StartCoroutine(GameObjectBehaviour(WarningObject, false, 3f));
        
    }

    public IEnumerator GameObjectBehaviour(GameObject g, bool isActive, float timer = 1f)
    {
        yield return new WaitForSeconds(timer);
        g.SetActive(isActive);
        indicatorAnim.SetBool("Indicate", true);
        handIndicator.gameObject.SetActive(true);
        clothIndicator.gameObject.SetActive(true);
        foreach (var top in topWear)
        {
            top.raycastTarget = true;
        }
    }
   
}
