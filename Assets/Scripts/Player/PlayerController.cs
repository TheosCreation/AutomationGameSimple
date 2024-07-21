using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public Inventory inventory;
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        inventory = GetComponent<Inventory>();
    }
}