using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public Inventory inventory;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        inventory = GetComponent<Inventory>();
    }
}