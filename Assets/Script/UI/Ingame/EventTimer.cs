using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private float eventCycle;


    private void Update()
    {
        timeText.text = ((int)Timer.Instance.GameTime).ToString();
    }

}
