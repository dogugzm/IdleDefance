using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Color baseColor;
    [SerializeField] Color offsetColor;
    [SerializeField] Color baseColorE;
    [SerializeField] Color offsetColorE;
    SpriteRenderer spriteRenderer;
    public int x, y;

    public bool hasBuilding = false;
    public bool isEnemySide = false;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(bool _isOffset, int _x, int _y)
    {
        if (isEnemySide)
        {
            spriteRenderer.color = _isOffset ? offsetColorE : baseColorE;
        }
        else
        {
            spriteRenderer.color = _isOffset ? offsetColor : baseColor;
        }
        x = _x; 
        y = _y;
    }

    private void OnMouseDown()
    {
        MouseClicked();
    }

    private void MouseClicked()
    {
        GridManager.TileClickEvent?.Invoke(x,y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
    }


}
