
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DialoguePersonInstance : MonoBehaviour
{
    public float targetXPosition;

    [SerializeField] private float MoveSpeed;
    [SerializeField] private float AlphaSpeed;

    private bool fadeIn = true;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
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
    }
}
