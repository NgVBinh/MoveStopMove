using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTowardEnemys : MonoBehaviour
{
    public Camera mainCamera;
    public Canvas arrowCanvas;
    public GameObject arrowPrefab;

    public List<TargetIndicator> targetIndicators = new List<TargetIndicator>();

    void Start()
    {
        
    }

    private void Update()
    {
        if(targetIndicators.Count > 0)
        {
            for(int i = 0; i < targetIndicators.Count; i++)
                targetIndicators[i].UpdateTargetIndicator();
        }
    }

    public void AddTargetIndicator(GameObject target)
    {
        TargetIndicator indicator = Instantiate(arrowPrefab, arrowCanvas.transform).GetComponent<TargetIndicator>();
        indicator.InitialTargetIndicator(target,mainCamera, arrowCanvas,this);
        targetIndicators.Add(indicator);
    }

    public void RemoveTargetIndicator(TargetIndicator target)
    {
        targetIndicators.Remove(target);
   
    }
}
