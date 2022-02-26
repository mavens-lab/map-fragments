using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimespanToText : MonoBehaviour
{
    [SerializeField] TMP_Text m_text;

    public void UpdateTextDisplay(AURORA_GameObject ago, AURORA.ClientAPI.AuroraObject ao, System.TimeSpan ts)
    {
        m_text.text = ts.TotalMilliseconds.ToString("0") + " ms";
    }
}