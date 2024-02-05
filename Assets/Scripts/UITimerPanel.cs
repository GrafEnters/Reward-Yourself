using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UITimerPanel : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _timeText;

    private int _time;
    private int _maxTime;
    private Coroutine _timerCoroutine;
    private DateTime _pausedTime;

    [SerializeField]
    private Transform _timerArrow;

    [SerializeField]
    private Ringtone _ringtone;

    [SerializeField]
    private float _timerToReset = 5;
    
    [SerializeField]
    private Transform _resetButton;

    public void OpenAndStartTimer() {
        _time = Loader.TasksManager.Profile.TimeAmount;
        _maxTime = _time;
        gameObject.SetActive(true);
        UpdateTimeText(_time);
        _timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine() {
        while (_time > 0) {
            yield return new WaitForSeconds(1);
            DecreaseTimer(1);
        }

        StartAlarm();
    }

    private void DecreaseTimer(int seconds) {
        _time -= seconds;
        if (_time < 0) {
            _time = 0;
        }

        UpdateTimeText(_time);
    }

    private void UpdateTimeText(int time) {
        _timeText.text = TimeUtils.GetStrFromSeconds(time);
        float angle = Mathf.Lerp(360, 45, _maxTime == 0 ? 1 : (time + 0f) / _maxTime);
        _timerArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void StartAlarm() {
        _ringtone.Play();
    }

    public void StopTimerAndClose() {
        TryStopTimer();
        _ringtone.Stop();
        gameObject.SetActive(false);
        Loader.UITasksPanel.UpdateTimeText();
    }

    private void TryStopTimer() {
        if (_timerCoroutine != null) {
            StopCoroutine(_timerCoroutine);
        }

        Loader.TasksManager.SaveTime(_time);
    }

#if UNITY_EDITOR

    private void OnApplicationFocus(bool focus) {
        if (focus) {
            DateTime afterPause = DateTime.Now;
            TimeSpan span = afterPause - _pausedTime;
            DecreaseTimer((int)span.TotalSeconds);
            Loader.TasksManager.SaveTime(_time);
            if (_time > 0) {
                OpenAndStartTimer();
            } else {
                StartAlarm();
            }
        } else {
            TryStopTimer();
            Loader.TasksManager.SaveTime(_time);
            _pausedTime = DateTime.Now;
        }
    }

#endif

    private Coroutine _resetTimerCoroutine;

    public void StartHoldingReset() {
        _resetTimerCoroutine = StartCoroutine(ResetTimer());
    }

    public void EndHoldingReset() {
        if (_resetTimerCoroutine == null) {
            return;
        }
        _resetButton.transform.localScale = Vector3.one;
        StopCoroutine(_resetTimerCoroutine);
        _resetTimerCoroutine = null;
    }

    private IEnumerator ResetTimer() {
        float timer = _timerToReset;

        Vector3 finVector = Vector3.one * 20;
        while (timer > 0) {
            float curPercent = 1 - timer / _timerToReset;
            _resetButton.localScale = Vector3.Slerp(Vector3.one, finVector, curPercent);
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }

        ResetTime();
    }

    private void ResetTime() {
        _time = 0;
        _resetButton.transform.localScale = Vector3.one;
        Loader.TasksManager.SaveTime(_time);
        StopTimerAndClose();
    }
}