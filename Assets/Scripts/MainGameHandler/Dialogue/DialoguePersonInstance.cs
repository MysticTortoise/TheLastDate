
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialoguePersonInstance : MonoBehaviour
{
    public float targetXPosition;

    [SerializeField] private float MoveSpeed;
    [SerializeField] private float AlphaSpeed;

    private bool fadeIn = true;

    private RectTransform rectTransform;
    private Image image;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        rectTransform.anchoredPosition = new Vector2(targetXPosition, rectTransform.anchoredPosition.y);
    }

    public void Disappear()
    {
        fadeIn = false;
    }

    private void Update()
    {
        Vector2 pos = rectTransform.anchoredPosition;
        pos.x += TLDMath.CloseToZero(
            targetXPosition - pos.x,
            MoveSpeed * Time.deltaTime * Mathf.Sign(targetXPosition - pos.x)
        );
        rectTransform.anchoredPosition = pos;

        float targAlpha = (fadeIn ? 1 : 0);
        Color col = image.color;
        col.a += TLDMath.CloseToZero(
            targAlpha - col.a,
            AlphaSpeed * Time.deltaTime * Mathf.Sign(targAlpha - col.a)
        );

        if (col.a <= 0)
        {
            Destroy(gameObject);
        }
        
        image.color = col;
    }
}
