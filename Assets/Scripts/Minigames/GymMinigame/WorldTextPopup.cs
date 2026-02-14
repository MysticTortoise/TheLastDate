using UnityEngine;
using TMPro;

public class WorldTextPopup : MonoBehaviour
{
    public TMP_Text text;
    public float lifetime = 0.8f;
    public float floatSpeed = 1.0f;

    private float _t;
    private Color _startColor;

    void Awake()
    {
        if (text == null) text = GetComponentInChildren<TMP_Text>();
        if (text != null) _startColor = text.color;
    }

    public void SetText(string msg)
    {
        if (text != null) text.text = msg;
    }

    void Update()
    {
        _t += Time.deltaTime;

        // float up
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // fade out
        if (text != null)
        {
            float a = Mathf.Lerp(_startColor.a, 0f, _t / lifetime);
            text.color = new Color(_startColor.r, _startColor.g, _startColor.b, a);
        }

        if (_t >= lifetime)
            Destroy(gameObject);
    }
}
