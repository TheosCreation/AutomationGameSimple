using UnityEngine;
using System.Collections.Generic;

public class ConveyorMoveItems : MonoBehaviour
{
    public List<CollectableItem> itemsOnConveyor = new List<CollectableItem>();
    private HashSet<CollectableItem> itemsSet = new HashSet<CollectableItem>();
    public float conveyorMoveSpeed = 1.5f;
    private TileInfo tileInfo;

    private void Start()
    {
        tileInfo = GetComponent<TileInfo>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectableItem item = collision.GetComponent<CollectableItem>();
        if (item != null)
        {
            if (!itemsSet.Contains(item))
            {
                itemsOnConveyor.Add(item);
                itemsSet.Add(item);

                Vector3 itemPosition = item.transform.position;
                if (tileInfo.direction == Vector3.right)
                {
                    item.ResetVerticalVelocity();
                    itemPosition.y = Mathf.Floor(itemPosition.y) + 0.5f;
                }
                else if (tileInfo.direction == Vector3.left)
                {
                    item.ResetVerticalVelocity();
                    itemPosition.y = Mathf.Floor(itemPosition.y) + 0.5f;
                }
                else if (tileInfo.direction == Vector3.up)
                {
                    item.ResetHorizontalVelocity();
                    itemPosition.x = Mathf.Floor(itemPosition.x) + 0.5f;
                }
                else if (tileInfo.direction == Vector3.down)
                {
                    item.ResetHorizontalVelocity();
                    itemPosition.x = Mathf.Floor(itemPosition.x) + 0.5f;
                }
                item.transform.position = itemPosition;
                // Disable item pickup while on conveyor
                item.ableToPickup = false;
            }
        }
    }

    private void Update()
    {
        // Move each item along the conveyor belt
        for (int i = itemsOnConveyor.Count - 1; i >= 0; i--)
        {
            CollectableItem item = itemsOnConveyor[i];
            if (item == null)
            {
                itemsOnConveyor.RemoveAt(i);
                itemsSet.Remove(item);
                continue;
            }

            Vector3 currentVelocity = item.rb.velocity;

            Vector3 desiredVelocity = tileInfo.direction.normalized * conveyorMoveSpeed; 
            Vector3 velocityDifference = desiredVelocity - currentVelocity;

            currentVelocity += velocityDifference * Time.deltaTime;

            item.rb.velocity = currentVelocity;
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CollectableItem item = collision.GetComponent<CollectableItem>();
        if (item != null)
        {
            // Enable item pickup once off the conveyor
            item.ableToPickup = true;

            // Remove the item from the list of items being moved by the conveyor belt
            itemsOnConveyor.Remove(item);
            itemsSet.Remove(item);
        }
    }
}
