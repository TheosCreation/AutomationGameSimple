using Items;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public Item itemToAdd;

    [HideInInspector]public Rigidbody2D rb;

    public bool ableToPickup = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ableToPickup == false) return;

        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.inventory.AddItem(itemToAdd); 
            ableToPickup = false;
            Destroy(gameObject);
        }
    }

    public void ResetHorizontalVelocity()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }
    public void ResetVerticalVelocity()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
    }
}
