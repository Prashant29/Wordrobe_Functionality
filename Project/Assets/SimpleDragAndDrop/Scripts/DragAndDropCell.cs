﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Rendering;

/// <summary>
/// Every item's cell must contain this script
/// </summary>
[RequireComponent(typeof(Image))]
public class DragAndDropCell : MonoBehaviour, IDropHandler
{
    public enum CellType                                                    // Cell types
    {
        Swap,                                                               // Items will be swapped between any cells
        DropOnly,                                                           // Item will be dropped into cell
        DragOnly                                                            // Item will be dragged from this cell
    }

    public enum TriggerType                                                 // Types of drag and drop events
    {
        DropRequest,                                                        // Request for item drop from one cell to another
        DropEventEnd,                                                       // Drop event completed
        ItemAdded,                                                          // Item manualy added into cell
        ItemWillBeDestroyed                                                 // Called just before item will be destroyed
    }

    public class DropEventDescriptor                                        // Info about item's drop event
    {
        public TriggerType triggerType;                                     // Type of drag and drop trigger
        public DragAndDropCell sourceCell;                                  // From this cell item was dragged
        public DragAndDropCell destinationCell;                             // Into this cell item was dropped
        public DragAndDropItem item;                                        // Dropped item
        public bool permission;                                             // Decision need to be made on request
    }

	[Tooltip("Functional type of this cell")]
    public CellType cellType = CellType.Swap;                               // Special type of this cell
	[Tooltip("Sprite color for empty cell")]
    public Color empty = new Color();                                       // Sprite color for empty cell
	[Tooltip("Sprite color for filled cell")]
    public Color full = new Color();                                        // Sprite color for filled cell
	[Tooltip("This cell has unlimited amount of items")]
    public bool unlimitedSource = false;                                    // Item from this cell will be cloned on drag start

	private DragAndDropItem myDadItem;										// Item of this DaD cell

    void OnEnable()
    {
        DragAndDropItem.OnItemDragStartEvent += OnAnyItemDragStart;         // Handle any item drag start
        DragAndDropItem.OnItemDragEndEvent += OnAnyItemDragEnd;             // Handle any item drag end
		UpdateMyItem();
		//UpdateBackgroundState();
    }

    void OnDisable()
    {
        DragAndDropItem.OnItemDragStartEvent -= OnAnyItemDragStart;
        DragAndDropItem.OnItemDragEndEvent -= OnAnyItemDragEnd;
        StopAllCoroutines();                                                // Stop all coroutines if there is any
    }

    /// <summary>
    /// On any item drag start need to disable all items raycast for correct drop operation
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragStart(DragAndDropItem item)
    {
        //item.GetComponent<Transform>().SetAsLastSibling();
        UpdateMyItem();
		if (myDadItem != null)
        {
			myDadItem.MakeRaycast(false);                                  	// Disable item's raycast for correct drop handling
			if (myDadItem == item)                                         	// If item dragged from this cell
            {
                // Check cell's type
                switch (cellType)
                {
                    case CellType.DropOnly:
                        DragAndDropItem.icon.SetActive(false);              // Item can not be dragged. Hide icon
                        break;
                }
            }
        }
    }

    /// <summary>
    /// On any item drag end enable all items raycast
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragEnd(DragAndDropItem item)
    {
       

        UpdateMyItem();
		if (myDadItem != null)
        {
       // Debug.Log("MY dataItem:" + myDadItem.name);
			myDadItem.MakeRaycast(true);                                  	// Enable item's raycast
        }
		//UpdateBackgroundState();
    }

    /// <summary>
    /// Item is dropped in this cell
    /// </summary>
    /// <param name="data"></param>
    public void OnDrop(PointerEventData data)
    {
        Debug.Log("On Drop");
        if (DragAndDropItem.icon != null)
        {
            DragAndDropItem item = DragAndDropItem.draggedItem;
            DragAndDropCell sourceCell = DragAndDropItem.sourceCell;
            if (DragAndDropItem.icon.activeSelf == true)                    // If icon inactive do not need to drop item into cell
            {
                if ((item != null) && (sourceCell != this))
                {
                    DropEventDescriptor desc = new DropEventDescriptor();
                    switch (cellType)                                       // Check this cell's type
                    {
                        case CellType.Swap:                                 // Item in destination cell can be swapped
							UpdateMyItem();
                            switch (sourceCell.cellType)
                            {
                                case CellType.Swap:                         // Item in source cell can be swapped
                                    // Fill event descriptor
                                    desc.item = item;
                                    desc.sourceCell = sourceCell;
                                    desc.destinationCell = this;
                                    SendRequest(desc);                      // Send drop request
                                    StartCoroutine(NotifyOnDragEnd(desc));  // Send notification after drop will be finished
                                    if (desc.permission == true)            // If drop permitted by application
                                    {
										if (myDadItem != null)            // If destination cell has item
                                        {
                                            // Fill event descriptor
                                            DropEventDescriptor descAutoswap = new DropEventDescriptor();
											descAutoswap.item = myDadItem;
                                            descAutoswap.sourceCell = this;
                                            descAutoswap.destinationCell = sourceCell;
                                            SendRequest(descAutoswap);                      // Send drop request
                                            StartCoroutine(NotifyOnDragEnd(descAutoswap));  // Send notification after drop will be finished
                                            if (descAutoswap.permission == true)            // If drop permitted by application
                                            {
                                                SwapItems(sourceCell, this);                // Swap items between cells
                                            }
                                            else
                                            {
												PlaceItem(item, desc.destinationCell);            // Delete old item and place dropped item into this cell
                                            }
                                        }
                                        else
                                        {
											PlaceItem(item, desc.destinationCell);                // Place dropped item into this empty cell
                                        }
                                    }
                                    break;
                                default:                                    // Item in source cell can not be swapped
                                    // Fill event descriptor
                                    desc.item = item;
                                    desc.sourceCell = sourceCell;
                                    desc.destinationCell = this;
                                    SendRequest(desc);                      // Send drop request
                                    StartCoroutine(NotifyOnDragEnd(desc));  // Send notification after drop will be finished
                                    if (desc.permission == true)            // If drop permitted by application
                                    {
										PlaceItem(item, desc.destinationCell);                    // Place dropped item into this cell
                                    }
                                    break;
                            }
                            break;
                        case CellType.DropOnly:                             // Item only can be dropped into destination cell
                            // Fill event descriptor
                            desc.item = item;
                            desc.sourceCell = sourceCell;
                            desc.destinationCell = this;
                            SendRequest(desc);                              // Send drop request
                            StartCoroutine(NotifyOnDragEnd(desc));          // Send notification after drop will be finished
                            if (desc.permission == true)                    // If drop permitted by application
                            {
								PlaceItem(item, desc.destinationCell);                            // Place dropped item in this cell
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            if (item != null)
            {
                if (item.GetComponent<DragAndDropCell>() == null)   // If item have no cell after drop
                {
                    Debug.LogError("IS it from here: " + item.name + " ==>");
                    Destroy(item.gameObject);                               // Destroy it
                }
            }
			UpdateMyItem();
			//UpdateBackgroundState();
			sourceCell.UpdateMyItem();
			//sourceCell.UpdateBackgroundState();
        }
    }

	/// <summary>
	/// Put item into this cell.
	/// </summary>
	/// <param name="item">Item.</param>
	private void PlaceItem(DragAndDropItem item, DragAndDropCell targetCell)
	{
        Debug.LogError("Tag: " + item.tag);
        Debug.LogError("targetCell: " + targetCell.tag);
		if (item != null
        && (((item.CompareTag("TShirt_blue") || item.CompareTag("Red_jacket") || item.CompareTag("TShirt_yellow") ) && targetCell.CompareTag("TShirt"))// Condition for Top wear
        || ((item.CompareTag("Full_Pant") || item.CompareTag("Short_blue") || item.CompareTag("Short_skin") ) && targetCell.CompareTag("Pants")) // Condition for Bottom wear
        || ((item.CompareTag("Casual_Boot") || item.CompareTag("WinterBoot") || item.CompareTag("Slipper") ) && targetCell.CompareTag("Foot"))
        )
            )
        
		{
			//DestroyItem();                                            	// Remove current item from this cell
			myDadItem = null;
			DragAndDropCell cell = item.GetComponent<DragAndDropCell>();
			if (cell != null)
			{
				if (cell.unlimitedSource == true)
				{
					string itemName = item.name;
					item = Instantiate(item);                               // Clone item from source cell
					item.name = itemName;
				}
			}
			//item.transform.SetParent(transform, false);
			//item.transform.localPosition = Vector3.zero;
			item.MakeRaycast(true);
			myDadItem = item;

            DressFunction(item);


        }else{
            Debug.LogError("Item misMatched");
        }
            //UpdateBackgroundState();
        }

    /// <summary>
    /// Destroy item in this cell
    /// </summary>
    private void DestroyItem()
    {
		UpdateMyItem();
		if (myDadItem != null)
        {
            DropEventDescriptor desc = new DropEventDescriptor();
            // Fill event descriptor
            desc.triggerType = TriggerType.ItemWillBeDestroyed;
			desc.item = myDadItem;
            desc.sourceCell = this;
            desc.destinationCell = this;
            Debug.LogError("MyDadItem: "  + myDadItem.name);
            SendNotification(desc);                                         // Notify application about item destruction
			if (myDadItem != null)
			{
                Destroy(myDadItem.gameObject);
			}
        }
		myDadItem = null;
		//UpdateBackgroundState();
    }

    /// <summary>
    /// Send drag and drop information to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    private void SendNotification(DropEventDescriptor desc)
    {
        if (desc != null)
        {
            // Send message with DragAndDrop info to parents GameObjects
            gameObject.SendMessageUpwards("OnSimpleDragAndDropEvent", desc, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Send drag and drop request to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    /// <returns> result from desc.permission </returns>
    private bool SendRequest(DropEventDescriptor desc)
    {
        bool result = false;
        if (desc != null)
        {
            desc.triggerType = TriggerType.DropRequest;

            desc.permission = true;
            SendNotification(desc);
            result = desc.permission;
        }
        return result;
    }

    /// <summary>
    /// Wait for event end and send notification to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    /// <returns></returns>
    private IEnumerator NotifyOnDragEnd(DropEventDescriptor desc)
    {
        // Wait end of drag operation
        while (DragAndDropItem.draggedItem != null)
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.LogError("Item: " + desc.item.name);
        Debug.LogError("destinationCell: " + desc.destinationCell.name);
        Debug.LogError("Source: " + desc.sourceCell.name);
        desc.triggerType = TriggerType.DropEventEnd;
        SendNotification(desc);
    }

	/// <summary>
	/// Change cell's sprite color on item put/remove.
	/// </summary>
	/// <param name="condition"> true - filled, false - empty </param>
	public void UpdateBackgroundState()
	{
		Image bg = GetComponent<Image>();
		if (bg != null)
		{
			bg.color = myDadItem != null ? full : empty;
		}
	}

	/// <summary>
	/// Updates my item
	/// </summary>
	public void UpdateMyItem()
	{
		myDadItem = GetComponent<DragAndDropItem>();
	}

	/// <summary>
	/// Get item from this cell
	/// </summary>
	/// <returns> Item </returns>
	public DragAndDropItem GetItem()
	{
		return myDadItem;
	}

    /// <summary>
    /// Manualy add item into this cell
    /// </summary>
    /// <param name="newItem"> New item </param>
    public void AddItem(DragAndDropItem newItem)
    {
        if (newItem != null)
        {
            DropEventDescriptor desc = new DropEventDescriptor();
			PlaceItem(newItem, desc.destinationCell);
            // Fill event descriptor
            desc.triggerType = TriggerType.ItemAdded;
            desc.item = newItem;
            desc.sourceCell = this;
            desc.destinationCell = this;
            SendNotification(desc);
        }
    }

    /// <summary>
    /// Manualy delete item from this cell
    /// </summary>
    public void RemoveItem()
    {
        DestroyItem();
    }

	/// <summary>
	/// Swap items between two cells
	/// </summary>
	/// <param name="firstCell"> Cell </param>
	/// <param name="secondCell"> Cell </param>
	public void SwapItems(DragAndDropCell firstCell, DragAndDropCell secondCell)
	{
		if ((firstCell != null) && (secondCell != null))
		{
			DragAndDropItem firstItem = firstCell.GetItem();                // Get item from first cell
			DragAndDropItem secondItem = secondCell.GetItem();              // Get item from second cell
			// Swap items
			if (firstItem != null)
			{
				firstItem.transform.SetParent(secondCell.transform, false);
				firstItem.transform.localPosition = Vector3.zero;
				firstItem.MakeRaycast(true);
			}
			if (secondItem != null)
			{
				secondItem.transform.SetParent(firstCell.transform, false);
				secondItem.transform.localPosition = Vector3.zero;
				secondItem.MakeRaycast(true);
			}
			// Update states
			firstCell.UpdateMyItem();
			secondCell.UpdateMyItem();
			//firstCell.UpdateBackgroundState();
			//secondCell.UpdateBackgroundState();
		}
	}


    void DressFunction(DragAndDropItem item )
    {
        //Debug.LogError("Target Tag: " + destinationCell.tag);

        if(item.isUsed){
                ClothingManager.instance.WarningObject.GetComponentInChildren<Text>().text = "It's used cloth";
                ClothingManager.instance.WarningObject.SetActive(true);
                StartCoroutine(ClothingManager.instance.GameObjectBehaviour(ClothingManager.instance.WarningObject, false, 3f));
                ClothingManager.instance.speechSound.clip = ClothingManager.instance.forWrongChoice;
                ClothingManager.instance.speechSound.Play();
                return;
        }
        #region Top Wear
        if (item.CompareTag("TShirt_blue"))
        {
            Character.instance.upperNum = 1;
            Character.instance.InitDresses();
            ClothingManager.instance.AfterTopWear();
            item.isUsed = true;
        }
        if (item.CompareTag("TShirt_yellow"))
        {
            Character.instance.upperNum = 2;
            Character.instance.InitDresses();
             ClothingManager.instance.AfterTopWear();
             item.isUsed = true;
        }
        if (item.CompareTag("Red_jacket") )
        {
            if(ClothingManager.instance.isSummer){
                ClothingManager.instance.WarningObject.GetComponentInChildren<Text>().text = "It's very hot outside!!!";
                ClothingManager.instance.WarningObject.SetActive(true);
                StartCoroutine(ClothingManager.instance.GameObjectBehaviour(ClothingManager.instance.WarningObject, false));
                ClothingManager.instance.speechSound.clip = ClothingManager.instance.forWrongChoice;
                ClothingManager.instance.speechSound.Play();
            }else{
                Character.instance.upperNum = 3;
                Character.instance.InitDresses();
                ClothingManager.instance.AfterTopWear();
                item.isUsed = true;

            }
           
            
        }

        #endregion

        #region Bottom Wear

        if (item.CompareTag("Full_Pant") )
        {
            if(!ClothingManager.instance.isSummer){
                Character.instance.downNum = 3;
                Character.instance.InitDresses();
                foreach (var top in ClothingManager.instance.bottomWear)
                {
                    top.raycastTarget = false;
                }
                item.isUsed = true;
                ClothingManager.instance.AfterBottomWear();
            }else{
                ClothingManager.instance.WarningObject.GetComponentInChildren<Text>().text = "It's very hot outside!!!";
                ClothingManager.instance.WarningObject.SetActive(true);
                StartCoroutine(ClothingManager.instance.GameObjectBehaviour(ClothingManager.instance.WarningObject, false));
            }
        }
        if (item.CompareTag("Short_skin") )
        {
            item.isUsed = true;

            Character.instance.downNum = 2;
            Character.instance.InitDresses();
            ClothingManager.instance.AfterBottomWear();
        }
         if (item.CompareTag("Short_blue") )
        {
            item.isUsed = true;

            Character.instance.downNum = 1;
            Character.instance.InitDresses();
            ClothingManager.instance.AfterBottomWear();
        }

        #endregion

        #region Foot Wear


        if(item.CompareTag("WinterBoot")){
            if(ClothingManager.instance.isRainy){
                Character.instance.shoeNum = 1;
                Character.instance.InitDresses();
                item.isUsed = true;

                foreach (var top in ClothingManager.instance.footWear)
                {
                    top.raycastTarget = false;
                }
                ResetOnEnd();

            }else{
                
                ClothingManager.instance.WarningObject.GetComponentInChildren<Text>().text = "It's very hot outside to wear that!!!";
                ClothingManager.instance.WarningObject.SetActive(true);
                StartCoroutine(ClothingManager.instance.GameObjectBehaviour(ClothingManager.instance.WarningObject, false));
            }
            if(ClothingManager.instance.isSummer){
                ClothingManager.instance.isSummerCompleted = true;
                ClothingManager.instance.isSummer = false;
            }
        }
        
        if(item.CompareTag("Slipper")){
            if(ClothingManager.instance.isSummer){
                item.isUsed = true;

                Character.instance.shoeNum = 2;
                Character.instance.InitDresses();
                foreach (var top in ClothingManager.instance.footWear)
                {
                    top.raycastTarget = false;
                }
            }
            else{
                ClothingManager.instance.WarningObject.GetComponentInChildren<Text>().text = "It's very Cold outside to wear that!!!";
                ClothingManager.instance.WarningObject.SetActive(true);
                StartCoroutine(ClothingManager.instance.GameObjectBehaviour(ClothingManager.instance.WarningObject, false));
            }
               if(ClothingManager.instance.isSummer){
                    ClothingManager.instance.isSummerCompleted = true;
                    ClothingManager.instance.isSummer = false;
                }
           ResetOnEnd();
           
        }

        if(item.CompareTag("Casual_Boot")){
                item.isUsed = true;

            Character.instance.shoeNum = 3;
            Character.instance.InitDresses();
            foreach (var top in ClothingManager.instance.footWear)
            {
                top.raycastTarget = false;
            }
            if(ClothingManager.instance.isSummer){
                ClothingManager.instance.isSummerCompleted = true;
                ClothingManager.instance.isSummer = false;
            }
           ResetOnEnd();

        }

        #endregion

        item.gameObject.SetActive(false);
        

    }


   void ResetOnEnd(){
        ClothingManager.instance.anim.SetTrigger("VideoEnd");
        Character.instance.downNum = 0;
        Character.instance.upperNum = 0;
        Character.instance.shoeNum = 0;
        Character.instance.hairNum = 0;
        Character.instance.InitDresses();
        ClothingManager.instance.successPanel.SetActive(true);
   }
}
