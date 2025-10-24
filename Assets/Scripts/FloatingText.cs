using UnityEngine;
using TMPro;  

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float lifetime = 1f;
    public float fadeSpeed = 2f;

    private TextMeshProUGUI text;  
    private Color startColor;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (text != null)
            startColor = text.color;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if (text != null)
        {
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(startColor.a, 0, Time.timeSinceLevelLoad * fadeSpeed);
            text.color = newColor;
        }
    }
}
