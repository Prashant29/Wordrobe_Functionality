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


    [Header("Enviournments")]
    public GameObject rainySeason;
    public GameObject winterSeason;

    public GameObject WarningObject;

    public Animator anim;

    public bool isSummer, isRainy, isSummerCompleted;

    public RectTransform handIndicator, clothIndicator;
    public Animator indicatorAnim;

    public Image[] topWear, bottomWear, footWear, headWear, rainWear;

    [Header("Sounds")]
    public AudioClip welcomeSpeech;
    public AudioClip demoSpeech, instruction, contAlsmost, coolSneakers, forBoots, forJacket, forPants, forRain, forSnow, forSpring, forSunny, 
    forWinterHat, forWrongChoice, greatCool, unbrella, success;

    public AudioSource bgSound, speechSound;

    public GameObject InstructionPanel;
    public GameObject skipButton;


    public GameObject successPanel;

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
       ResetEverything();
        handIndicator.gameObject.SetActive(false);
        clothIndicator.gameObject.SetActive(false);
       
    }

    void ResetEverything(){
         foreach (var top in topWear)
        {
            top.raycastTarget = true;
            top.gameObject.SetActive(true);
        }
        foreach (var b in bottomWear)
        {
            b.raycastTarget = false;
            b.gameObject.SetActive(true);

        }
        foreach (var h in headWear)
        {
            h.raycastTarget = false;
            h.gameObject.SetActive(true);

        }
        foreach (var f in footWear)
        {
            f.raycastTarget = false;
            f.gameObject.SetActive(true);

        }
        foreach (var r in rainWear)
        {
            r.raycastTarget = false;
            r.gameObject.SetActive(true);

        }
        handIndicator.anchoredPosition = new Vector2(304, 175f);
        clothIndicator.anchoredPosition = new Vector2(150, 175f);
    }

    public void StartGame()
    {
        
        ResetEverything();
        GameCanvas.SetActive(false);
        scene.SetActive(true);
        anim.SetTrigger("StartScene");
        StartCoroutine(LoadMyScene());
        isRainy |= isSummerCompleted;
        InstructionPanel.SetActive(false);
        if(isSummer){
            footWear[2].GetComponent<DragAndDropCell>().unlimitedSource = true;
            topWear[1].GetComponent<DragAndDropCell>().unlimitedSource = true;
            bottomWear[0].GetComponent<DragAndDropCell>().unlimitedSource = true;
        }
        else{
            topWear[1].GetComponent<DragAndDropCell>().unlimitedSource = false;
            bottomWear[0].GetComponent<DragAndDropCell>().unlimitedSource = false;

            footWear[2].GetComponent<DragAndDropCell>().unlimitedSource = false;
        }

        if(isRainy){
             foreach (var item in rainWear)
            {
                item.GetComponent<Image>().raycastTarget = true;
            }
        }else{
             foreach (var item in rainWear)
            {
                item.GetComponent<Image>().raycastTarget = false;
            }
        }
        handIndicator.anchoredPosition = new Vector2(304, 175f);
        clothIndicator.anchoredPosition = new Vector2(150, 175f);
        Debug.Log("IsRainy: " + isRainy);
        if(isRainy){
            GameObjectBehaviour(InstructionPanel, false, 3f);
            rainySeason.SetActive(true);
            StartRainyDay();
        }
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
        if(!isSummerCompleted){
            speechSound.clip = welcomeSpeech;
            speechSound.Play();
            InstructionPanel.SetActive(true);
        }
       
        GameCanvas.SetActive(true);
        
    }

    public void StartSunnyDay()
    {
        StartCoroutine(PlayInstructinSound(instruction));
        WarningObject.GetComponentInChildren<Text>().text = "Today it is Hot and Sunny.\nWhat should Jimmy wear?";
        WarningObject.SetActive(true);

        StartCoroutine(GameObjectBehaviour(WarningObject, false, 3f));
        
    }

    public void StartRainyDay(){
        StartCoroutine(PlayInstructinSound(forRain));
        WarningObject.GetComponentInChildren<Text>().text = "Today it is Rainy and Cloudy Day.\nWhat should Jimmy wear?";
        WarningObject.SetActive(true);

        StartCoroutine(GameObjectBehaviour(WarningObject, false, 3f));
    }

    IEnumerator PlayInstructinSound(AudioClip sound)
    {
        speechSound.clip = sound;
        speechSound.Play();
        yield return new WaitForSeconds(speechSound.clip.length);
        handIndicator.gameObject.SetActive(true);
        clothIndicator.gameObject.SetActive(true);
        foreach (var top in topWear)
        {
            top.raycastTarget = true;
        }
    }

    public IEnumerator GameObjectBehaviour(GameObject g, bool isActive, float timer = 1f)
    {
        yield return new WaitForSeconds(timer);
        g.SetActive(isActive);
        indicatorAnim.SetBool("Indicate", true);
        
    }

    public void AfterTopWear(){
        foreach (var top in topWear)
        {
            top.raycastTarget = false;
        }
        handIndicator.anchoredPosition = new Vector2(304, 66.1f);
        clothIndicator.anchoredPosition = new Vector2(150, 66.1f);
        foreach (var top in bottomWear)
        {
            top.raycastTarget = true;
        }
    }

     public void AfterBottomWear(){
        foreach (var top in bottomWear)
        {
            top.raycastTarget = false;
        }
        handIndicator.anchoredPosition = new Vector2(304, -218f);
        clothIndicator.anchoredPosition = new Vector2(150, 66.1f);
        foreach (var top in footWear)
        {
            top.raycastTarget = true;
        }
    }

     public void AfterFootWear(){
        foreach (var top in footWear)
        {
            top.raycastTarget = false;
        }
        handIndicator.anchoredPosition = new Vector2(304, 66.1f);
        clothIndicator.anchoredPosition = new Vector2(150, 66.1f);
        foreach (var top in bottomWear)
        {
            top.raycastTarget = true;
        }
    }

    public void PlayVideo(){

         GameCanvas.SetActive(false);
        scene.SetActive(true);
        anim.SetTrigger("video");
        StartCoroutine(LoadMyScene());
        Invoke("GameCanvasOn", 3.2f);
        speechSound.clip = demoSpeech;
        speechSound.Play();
    }

    void GameCanvasOn(){
        skipButton.SetActive(true);
       // InstructionPanel.SetActive(true);
        GameCanvas.SetActive(true);
    }

    public void BackToMainMenu(){
        skipButton.SetActive(false);

        
       StartCoroutine(LoadMyWelcomeScreen());
    }

    IEnumerator LoadMyWelcomeScreen()
    {
      
        while(StartCanvas.GetComponent<CanvasGroup>().alpha < 1)
        {
            yield return new WaitForSeconds(.05f);
            StartCanvas.GetComponent<CanvasGroup>().alpha += .1f;
        }
        anim.SetTrigger("VideoEnd");
        scene.SetActive(false);
        GameCanvas.SetActive(false);
        speechSound.Pause();
        StartCanvas.SetActive(true);
    }

    public void NextDay(){
        if(isSummerCompleted){
            anim.SetTrigger("StartScene");
            GameCanvas.SetActive(false);
            successPanel.SetActive(false);
        }

        StartGame();
    }
   
}
