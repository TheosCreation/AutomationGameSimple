using UnityEngine;
using UnityEngine.UI;
using Items;
using System.Collections.Generic;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject mainPausePage;
    [SerializeField] private GameObject settingsPage;

    [SerializeField] private Texture2D mouseCursorSprite;

    [SerializeField] private List<HotbarSlot> hotbarSlots;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetDefaultCursor();
        SetupHotBar();
    }

    public void PauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
        inGameHUD.SetActive(!isPaused);
    }

    public void OpenMainPausePage()
    {
        mainPausePage.SetActive(true);
        settingsPage.SetActive(false);
    }

    public void OpenSettingsPage()
    {
        settingsPage.SetActive(true);
        mainPausePage.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.Instance.OpenScene("MainMenu");
    }
    
    public void QuitGame()
    {
        SceneManager.Instance.QuitApplication();
    }

    public void SetSelectedItem(Item item)
    {
        if (item == null)
        {
            SetDefaultCursor();
            return;
        }


        if (item is UseableItem useableItem)
        {
            Texture2D spriteTexture = GetSpriteTexture(item.PreviewSprite);
            // Make the hot spot the center of the texture for UseableItem
            Vector2 hotspot = new Vector2(0, spriteTexture.height / 2);
            Cursor.SetCursor(spriteTexture, hotspot, CursorMode.Auto);
        }
        else
        {
            SetDefaultCursor();
        }
    }

    public void SetDefaultCursor()
    {
        // Make the hot spot the center of the texture for UseableItem
        Vector2 hotspot = new Vector2(0, mouseCursorSprite.height /2);
        Cursor.SetCursor(mouseCursorSprite, hotspot, CursorMode.Auto);
    }

    private Texture2D GetSpriteTexture(Sprite sprite)
    {
        Rect rect = sprite.rect;
        Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        texture.SetPixels(sprite.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height));
        texture.Apply();
        return texture;
    }
    private void SetupHotBar()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            hotbarSlots[i].slotCount = i;
        }
    }
    public void UpdateHotbar(Dictionary<Item, int> itemsInHotBar)
    {
        int index = 0;
        foreach (var item in itemsInHotBar)
        {
            if (index < hotbarSlots.Count)
            {
                hotbarSlots[index].SetItem(item.Key, item.Value);
                index++;
            }
            else
            {
                break;
            }
        }

        // Clear remaining slots if there are more slots than items
        for (int i = index; i < hotbarSlots.Count; i++)
        {
            hotbarSlots[i].SetItem(null, 0);
        }
    }


}