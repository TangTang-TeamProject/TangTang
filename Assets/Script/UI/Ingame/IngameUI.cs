using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private Button pauseBTN;
    [SerializeField]
    private bool isPause;

    private int beforeTime = 0;

    private void Awake()
    {
        pauseBTN.onClick.AddListener(PauseButtonClick);
    }

    private void Update()
    {
        int currentTime = (int)Timer.Instance.GameTime;

        if (beforeTime == currentTime)
            return;

        beforeTime = currentTime;
        MakeTimeText(currentTime);
    }

    void MakeTimeText(int _time)
    {
        int min = _time / 60;

        int sec = _time % 60;

        timeText.text = $"{min:00} : {sec:00}";
    }

    void PauseButtonClick()
    {
        if (isPause)
        {
            isPause = false;
            ResumeGame();
        }
        else
        {
            isPause = true;
            PauseGame();
        }
    }

    void ResumeGame()
    {
        Timer.Instance.IsTimeStop(false);
    }

    void PauseGame()
    {
        Timer.Instance.IsTimeStop(true);
    }
}
