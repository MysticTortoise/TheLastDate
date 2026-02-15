using TMPro;
using UnityEngine;

public class TextPopupSetTMP : MonoBehaviour
{
    public TMP_Text tmp;

    public void SetText(string value)
    {
        if (tmp != null) tmp.text = value;
    }
}
