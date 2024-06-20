﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Drag and Drop item.
/// </summary>
public class DragAndDropItem : MonoBehaviour, IPointerUpHandler, IPointerDownHandler//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static string clicked = "";
	public static bool dragDisabled = false;										// Drag start global disable

	public static DragAndDropItem draggedItem;                                      // Item that is dragged now
	public static GameObject icon;                                                  // Icon of dragged item
	public static DragAndDropCell sourceCell;                                       // From this cell dragged item is

	public delegate void DragEvent(DragAndDropItem item);
	public static event DragEvent OnItemDragStartEvent;                             // Drag start event
	public static event DragEvent OnItemDragEndEvent;                               // Drag end event

	public static Canvas canvas;                                                   // Canvas for item drag operation
	public static string canvasName = "DragAndDropCanvas";                          // Name of canvas
	public static int canvasSortOrder = 100;                                        // Sort order for canvas



	public static bool isItemDropped = false, longPress = false;
	float totalDownTime = 0;
	const float clickDuration = 0.75f;
	bool  isPressing = false;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		if (canvas == null)
		{
			GameObject canvasObj = new GameObject(canvasName);
			canvas = canvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = canvasSortOrder;

		}
	}
    private void Start()
    {

	}
    private void Update()
    {
		if (isPressing && !longPress)
		{
			totalDownTime += Time.deltaTime;

			if (totalDownTime >= clickDuration)
			{
				totalDownTime = 0;
				longPress = true;
			}
		}
	}


    public void OnPointerDown(PointerEventData eventData)
	{
		isPressing = true;
		longPress = false;
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		Image myImage = GetComponent<Image>();
		myImage.color = new Color(1f, 1f, 1f, 1f);
		isItemDropped = true;
		clicked = transform.gameObject.name;
		isPressing = false;
	}


	/// <summary>
	/// This item started to drag.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (dragDisabled == false)
		{
			sourceCell = GetCell();                       							// Remember source cell
			draggedItem = this;                                                     // Set as dragged item
			// Create item's icon
			icon = new GameObject();
			icon.transform.SetParent(canvas.transform);
			icon.name = "Icon";
			Image myImage = GetComponent<Image>();			
			myImage.color = new Color(1f,1f,1f,0.5f);
			myImage.raycastTarget = false;                                          // Disable icon's raycast for correct drop handling

			Image iconImage = icon.AddComponent<Image>();
			iconImage.raycastTarget = false;
			iconImage.sprite = myImage.sprite;
			RectTransform iconRect = icon.GetComponent<RectTransform>();
			// Set icon's dimensions
			RectTransform myRect = GetComponent<RectTransform>();
			iconRect.pivot = new Vector2(0.5f, 0.5f);
			iconRect.anchorMin = new Vector2(0.5f, 0.5f);
			iconRect.anchorMax = new Vector2(0.5f, 0.5f);
			iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);

			if (OnItemDragStartEvent != null)
			{
				OnItemDragStartEvent(this);                                			// Notify all items about drag start for raycast disabling
			}
		}
	}

	/// <summary>
	/// Every frame on this item drag.
	/// </summary>
	/// <param name="data"></param>
	public void OnDrag(PointerEventData data)
	{
		if (icon != null)
		{
			icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor in screen pixels
		}
	}

	/// <summary>
	/// This item is dropped.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		ResetConditions();
	}

	/// <summary>
	/// Resets all temporary conditions.
	/// </summary>
	public void ResetConditions()
	{
		if (icon != null)
		{
			Destroy(icon);                                                          // Destroy icon on item drop
		}
		if (OnItemDragEndEvent != null)
		{
			OnItemDragEndEvent(this);                                       		// Notify all cells about item drag end
		}
		draggedItem = null;
		icon = null;
		sourceCell = null;
	}

	/// <summary>
	/// Enable item's raycast.
	/// </summary>
	/// <param name="condition"> true - enable, false - disable </param>
	public void MakeRaycast(bool condition)
	{
		Image image = GetComponent<Image>();
		if (image != null)
		{
			image.raycastTarget = condition;
			//if(condition != true)
			//	image.color = new Color(1f, 1f, 1f, 1f);
			//else
			//	image.color = new Color(1f, 1f, 1f, 0.5f);
		}
	}

	/// <summary>
	/// Gets DaD cell which contains this item.
	/// </summary>
	/// <returns>The cell.</returns>
	public DragAndDropCell GetCell()
	{
		return GetComponentInParent<DragAndDropCell>();
	}

	public void NotifyallItemsAboutDragStart()
    {
		if (OnItemDragStartEvent != null)
		{
			OnItemDragStartEvent(this);                                         // Notify all items about drag start for raycast disabling
		}
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		ResetConditions();
	}
}
