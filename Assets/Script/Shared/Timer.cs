using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // 시간관리 매니저 -> 보스 소환 이벤트줘야함 SO에서 이벤트 타임 받아야함
    public static Timer Instance {  get; private set; }
    private bool _isTimerRun = true;
    private bool _isFirst = true;
    private float _timer;
    private float _realTime;
    private float _keepTime;
    private float _tickTime;

    public float GameTime => _timer;
    public float RealTime => _realTime;
    public float TickTime => _tickTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Update()
    {
        _realTime += Time.deltaTime;
        if (_tickTime + 0.2f <= _realTime)
        {
            _tickTime = _realTime;
        }
        
        // 플레이 타이머 작동 여부
        if (!_isTimerRun)
        {
            // 보스 출현시 시간 고정
            if (_isFirst)
            {
                _keepTime = _timer / 60;
                _timer = _keepTime;
                _isFirst = false;
            }
            return;
        }
        _timer += Time.deltaTime;
    }

    public void IsTimeStop(bool stop)
    {
        Time.timeScale = stop ? 0.0f : 1.0f;
    }

    public void IsBossRound(bool alive)
    {
        _isTimerRun = !alive;
        /* 어차피 보스가 죽던 출현하던 _isFirst는 true로 초기화를 해야하지만 예외처리를 하고 싶을 경우 주석해제
        if (_isTimerRun)
        {
            _isFirst = true;
        }
        */
        _isFirst = true;
    }
}
