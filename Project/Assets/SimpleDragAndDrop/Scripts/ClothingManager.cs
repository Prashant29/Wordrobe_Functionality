using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

public class ClothingManager : MonoBehaviour
{
    public static ClothingManager instance = null;
     

    public GameObject tshirt, shorts, l_boots, r_boots, hat, sunglass;

    [Header("Material")]
    public Material blue_Tshirt;
    public Material red_tshirt;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

     void Start()
    {
        tshirt.SetActive(false);
        shorts.SetActive(false);
        r_boots.SetActive(false);
        l_boots.SetActive(false);
     //   hat.SetActive(false);
    }
}
