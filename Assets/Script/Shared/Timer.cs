using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // 시간관리 매니저 -> 보스 소환 이벤트줘야함 SO에서 이벤트 타임 받아야함
    public static Timer Instance {  get; private set; }
    private bool _isTimerRun = true;
    private float _timer;
    private float _realTime;
    private float _keepTime;
    private float _tickTime;
    private int _bossCount = 0;

    public float GameTime => _timer;
    public float RealTime => _realTime;
    public float TickTime => _tickTime;
    public event Action BossSpawn;
    public event Action<bool> BossDie;

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
            return;
        }
        _timer += Time.deltaTime;
    }

    public void IsTimeStop(bool stop)
    {
        Time.timeScale = stop ? 0.0f : 1.0f;
    }

    public void IsBossDie(bool last)
    {
        _isTimerRun = true;
        BossDie?.Invoke(last);
    }

    public void IsBossSpawn(float time)
    {
        _isTimerRun = false;
        BossSpawn?.Invoke();
        // 보스 출현시 시간 고정
        _keepTime = time;
        _timer = _keepTime;
        _bossCount++;
    }
}
