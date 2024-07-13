using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLevel : MonoBehaviour
{

    [SerializeField]private Image myLevelImage;
    
    [SerializeField] private SkinnedMeshRenderer bodyMaterial;
    // Update is called once per frame

    private void Start()
    {
        myLevelImage.color =bodyMaterial.material.color;
    }

    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
