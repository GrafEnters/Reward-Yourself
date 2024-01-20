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

    private static Loader _instance;

    public static ColorsConfig ColorsConfig;
    public static TasksManager TasksManager;
    public static UITasksPanel UITasksPanel;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        _isOnLoading = SceneManager.GetActiveScene().name == "LoadingScene";
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine() {
        ColorsConfig = _colorsConfig;
        SetLoadingPanel(true);
        TasksManager = FindObjectOfType<TasksManager>();
        yield return TasksManager.Loading();

        if (_isOnLoading) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
            while (!asyncLoad.isDone) {
                yield return null;
            }
        }

        UITasksPanel = FindObjectOfType<UITasksPanel>();
        yield return UITasksPanel.Loading();
        if (_isOnLoading) {
            SceneManager.UnloadSceneAsync("LoadingScene");
        }

        SetLoadingPanel(false);
    }

    private void SetLoadingPanel(bool isActive) {
        if (_loadingPanel) {
            _loadingPanel.SetActive(isActive);
        }
    }
}