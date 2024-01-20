using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {
    [SerializeField]
    private GameObject _loadingPanel;

    [SerializeField]
    private ColorsConfig _colorsConfig;

    [SerializeField]
    private bool _isOnLoading;
    
    public static ColorsConfig ColorsConfig;
    public static TasksManager TasksManager;
    public static UITasksPanel UITasksPanel;

    private void Awake() {
        _isOnLoading = SceneManager.GetActiveScene().name == "LoadingScene";
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine() {
        ColorsConfig = _colorsConfig;
        SetLoadingPanel(true);
        TasksManager = FindObjectOfType<TasksManager>();

        yield return TasksManager.Loading();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        UITasksPanel = FindObjectOfType<UITasksPanel>();
        yield return UITasksPanel.Loading();
        SceneManager.UnloadSceneAsync("LoadingScene");
        SetLoadingPanel(false);
    }

    private void SetLoadingPanel(bool isActive) {
        if (_loadingPanel) {
            _loadingPanel.SetActive(isActive);
        }
    }
}