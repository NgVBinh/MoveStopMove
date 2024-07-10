using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    private Vector2 inputVector;
    private Vector2 startPoint;
    //private bool isDragging = false;

    //private Player player;

    [SerializeField] private GameObject instrucPannel;
    private void Start()
    {
       // player = FindObjectOfType<Player>();
        
    }

    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragPosition = eventData.position;
        inputVector = (dragPosition - startPoint).normalized;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        //isDragging = false;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPoint = eventData.position;
        if (instrucPannel.activeSelf)
        {
            instrucPannel.SetActive(false);
        }
        //inputVector = (startPoint - new Vector2(player.transform.position.x,player.transform.position.z)).normalized;
        //isDragging = true;
        //OnDrag(eventData);
    }

    private void OnDisable()
    {
        inputVector = Vector2.zero;
    }
}
