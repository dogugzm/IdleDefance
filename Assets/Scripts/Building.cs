using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

//TODO: inheritance for all building types.
public class Building : MonoBehaviour 
{
    [SerializeField] Button MoveButton;
    [SerializeField] Button OKButton;
    [SerializeField] Button RotateButton;


    [SerializeField] Color PreColor;
    [SerializeField] Color AfterColor;

    public static Action<int> BuilingApprovedAction;
    public static Action AddMoneyOnEveryXSecond;

    bool approved = false;
    [SerializeField]  SpriteRenderer spriteRenderer;
    public BuildingSO data;
    float timer;

    private void OnEnable()
    {
        AddEventToButton(MoveButton);
        OKButton.onClick.AddListener(BuildingApproved);
        RotateButton.onClick.AddListener(RotationClicked);

    }

    private void RotationClicked()
    {
        transform.DORotate(new Vector3(0,0,-90),0.5f,RotateMode.WorldAxisAdd).SetEase(Ease.OutElastic);
    }

    private void Start()
    {
        spriteRenderer.color = PreColor;     
    }
    
    private void BuildingApproved()
    {
        GridManager.Instance.SetTileAfterBuilding(transform.position, data.type);

        MoveButton.gameObject.SetActive(false);
        OKButton.gameObject.SetActive(false);
        RotateButton.gameObject.SetActive(false);
        BuilingApprovedAction?.Invoke(data.cost);
        approved = true;
    }

    private void Update()
    {
        if (!approved)
        {
            return;
        }

        switch (data.type)
        {
            case UnitTypes.SINGLE:
                timer += Time.deltaTime;
                if (timer>1)
                {
                    AddMoneyOnEveryXSecond.Invoke();
                    timer = 0;
                }
                break;
            case UnitTypes.MULTIPLE:
                break;
            case UnitTypes.L:
                break;
            default:
                break;
        }
    }


    void AddEventToButton(Button button)
    {
        // Check if the button already has an EventTrigger component
        EventTrigger eventTrigger = button.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            // If not, add it
            eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // Create an entry for the OnPointerDown event
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.EndDrag;

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.Drag;


        entry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data, button); });
        entry2.callback.AddListener((data) => { OnEndDrag((PointerEventData)data, button); });
        entry3.callback.AddListener((data) => { OnDrag((PointerEventData)data, button); });


        // Add the entry to the event trigger
        eventTrigger.triggers.Add(entry);
        eventTrigger.triggers.Add(entry2);
        eventTrigger.triggers.Add(entry3);
    }

    #region DRAG

    private void OnEndDrag(PointerEventData data, Button button)
    {
        Debug.Log("End Drag");
        spriteRenderer.color = AfterColor;

    }

    private void OnDrag(PointerEventData data, Button button)
    {
        // Convert screen space to world space
        Vector3 mousePos = Input.mousePosition;
        //mousePos.z = -Camera.main.transform.position.z;  // Adjust the Z coordinate based on the camera's distance
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 targetPos =  new Vector3(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y), 0);
        transform.DOMove(targetPos, 0.2f).SetEase(Ease.Flash);
    }

    private void OnBeginDrag(PointerEventData data, Button button)
    {
        Debug.Log("Begin Drag");
        spriteRenderer.color = PreColor;
    }

    #endregion

}
