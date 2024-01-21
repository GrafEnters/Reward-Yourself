using System;
using System.Collections;
using UnityEngine;

public class Ringtone : MonoBehaviour {
    [SerializeField]
    private float _fadeDuration, _baseVolume, _pitchStep;

    [SerializeField]
    private AudioSource _source;

    private bool _isPlaying;
    private Coroutine _coroutine;

    public void Play(bool isFade = true) {
        if (_isPlaying) {
            return;
        }

        TryStopCoroutine();
        _source.Play();
        if (isFade) {
            _source.pitch = 1;
            _coroutine = StartCoroutine(FadeSource(0, 1, _fadeDuration, OnFadeInEnds));
        } else {
            ChangeVolume(1);
        }

        _isPlaying = true;
    }

    public void Stop(bool isFade = true) {
        if (!_isPlaying) {
            return;
        }

        TryStopCoroutine();
        if (isFade) {
            _coroutine = StartCoroutine(FadeSource(1, 0, _fadeDuration, () => _source.Stop()));
        } else {
            _source.Stop();
        }

        _isPlaying = false;
    }

    private IEnumerator FadeSource(float from, float to, float dur, Action callback = null) {
        ChangeVolume(from);
        float time = 0;
        while (time < dur) {
            time += Time.deltaTime;
            ChangeVolume(Mathf.Lerp(from, to, time));
            yield return new WaitForEndOfFrame();
        }

        if (callback != null) {
            callback.Invoke();
        }
    }

    private void OnFadeInEnds() {
        _coroutine = StartCoroutine(PitchSource(1, 4));
    }

    private IEnumerator PitchSource(float from, float to) {
        _source.pitch = from;
        while (_source.pitch < to) {
            yield return new WaitForSeconds(_source.clip.length / 2);
            _source.pitch += _pitchStep;
        }
    }

    private void ChangeVolume(float percent) {
        _source.volume = _baseVolume * percent;
    }

    private void TryStopCoroutine() {
        if (_coroutine != null) {
            StopCoroutine(_coroutine);
        }
    }

    private void OnDestroy() {
        TryStopCoroutine();
    }
}