using TMPro;
using UnityEngine;

public class Debuging : MonoBehaviour
{
    public static Debuging Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public static TMP_Text CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color color = default(Color), TextAnchor textAnchor = TextAnchor.MiddleCenter, VertexSortingOrder sortingOrder = VertexSortingOrder.Normal)
    {
        if (color == default(Color)) color = Color.white;  // Set default color to white if not specified

        GameObject gameObject = new GameObject("World_Text", typeof(TMP_Text));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;

        TMP_Text textObj = gameObject.GetComponent<TMP_Text>();
        textObj.text = text;
        textObj.fontSize = fontSize;
        textObj.color = color;
        textObj.alignment = TextAlignmentOptions.Center;
        textObj.geometrySortingOrder = sortingOrder;

        return textObj;
    }

}
