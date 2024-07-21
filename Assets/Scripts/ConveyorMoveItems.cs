using UnityEngine;
using System.Collections.Generic;

public class ConveyorMoveItems : MonoBehaviour
{
    public List<CollectableItem> itemsOnConveyor = new List<CollectableItem>();
    private HashSet<CollectableItem> itemsSet = new HashSet<CollectableItem>();
    public float conveyorMoveSpeed = 1.5f;
    private TileInfo tileInfo;
    public float centeringSpeed = 2.0f; // Speed at which items move to the center

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

                // Disable item pickup while on the conveyor
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

            // Gradually move the item to the center of the conveyor
            Vector3 itemPosition = item.transform.position;
            Vector3 targetPosition = itemPosition;

            if (tileInfo.direction == Vector3.right || tileInfo.direction == Vector3.left)
            {
                targetPosition.y = Mathf.Floor(itemPosition.y) + 0.5f;
            }
            else if (tileInfo.direction == Vector3.up || tileInfo.direction == Vector3.down)
            {
                targetPosition.x = Mathf.Floor(itemPosition.x) + 0.5f;
            }

            item.transform.position = Vector3.MoveTowards(itemPosition, targetPosition, centeringSpeed * Time.deltaTime);

            // Move the item along the conveyor belt
            Vector3 currentVelocity = item.rb.velocity;
            Vector3 desiredVelocity = tileInfo.direction.normalized * conveyorMoveSpeed;
            Vector3 velocityDifference = desiredVelocity - currentVelocity;

            currentVelocity += velocityDifference * Time.deltaTime;
            item.rb.velocity = currentVelocity;
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
