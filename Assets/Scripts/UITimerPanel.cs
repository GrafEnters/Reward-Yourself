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
        float angle = Mathf.Lerp(360, 45, (time + 0f) / _maxTime);
        _timerArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void StartAlarm() {
        Debug.Log("Alarm is not implemented");
    }

    public void StopTimerAndClose() {
        TryStopTimer();
        gameObject.SetActive(false);
        Loader.UITasksPanel.UpdateTimeText();
    }

    private void TryStopTimer() {
        if (_timerCoroutine != null) {
            StopCoroutine(_timerCoroutine);
        }

        Loader.TasksManager.SaveTime(_time);
    }

    private void OnApplicationFocus(bool focus) {
        if (focus) {
            DateTime afterPause = DateTime.Now;
            TimeSpan span = afterPause - _pausedTime;
            DecreaseTimer(span.Seconds);
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
}