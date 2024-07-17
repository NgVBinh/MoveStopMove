using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLevel : MonoBehaviour
{
    private Entity entity;

    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image myLevelImage;
    
    [SerializeField] private SkinnedMeshRenderer bodyMaterial;
    // Update is called once per frame

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        if(characterName != null) characterName.text = entity.characterName;
        myLevelImage.color =bodyMaterial.material.color;
    }

    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
