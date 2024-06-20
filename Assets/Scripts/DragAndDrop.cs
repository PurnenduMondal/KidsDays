using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	DragAndDropItem dragAndDropItem;


	void Start()
    {
		dragAndDropItem = GetComponent<DragAndDropItem>();
    }

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (DragAndDropItem.dragDisabled == false)
		{
			DragAndDropItem.sourceCell = dragAndDropItem.GetCell();                                                 // Remember source cell
			DragAndDropItem.draggedItem = dragAndDropItem;                                                     // Set as dragged item
																									// Create item's DragAndDropItem.icon
			DragAndDropItem.icon = new GameObject();
			DragAndDropItem.icon.transform.SetParent(DragAndDropItem.canvas.transform);
			DragAndDropItem.icon.name = "Icon";
			Image myImage = GetComponent<Image>();
			myImage.color = new Color(1f, 1f, 1f, 0.5f);
			myImage.raycastTarget = false;                                          // Disable DragAndDropItem.icon's raycast for correct drop handling

			Image iconImage = DragAndDropItem.icon.AddComponent<Image>();
			iconImage.raycastTarget = false;
			iconImage.sprite = myImage.sprite;
			RectTransform iconRect = DragAndDropItem.icon.GetComponent<RectTransform>();
			// Set icon's dimensions
			RectTransform myRect = GetComponent<RectTransform>();
			iconRect.pivot = new Vector2(0.5f, 0.5f);
			iconRect.anchorMin = new Vector2(0.5f, 0.5f);
			iconRect.anchorMax = new Vector2(0.5f, 0.5f);
			iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);
			dragAndDropItem.NotifyallItemsAboutDragStart();
		}
	}

	/// <summary>
	/// Every frame on this item drag.
	/// </summary>
	/// <param name="data"></param>
	public void OnDrag(PointerEventData data)
	{
		if (DragAndDropItem.icon != null)
		{
			DragAndDropItem.icon.transform.position = GameObject.Find("PointerObj").transform.position;
			//DragAndDropItem.icon.transform.position = Input.mousePosition;                          // Item's DragAndDropItem.icon follows to cursor in screen pixels
		}
	}

	/// <summary>
	/// This item is dropped.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		dragAndDropItem.ResetConditions();
	}
}
