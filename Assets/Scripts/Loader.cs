using System.Collections;
using UnityEngine;

public class Loader : MonoBehaviour {
    [SerializeField]
    private GameObject _loadingPanel;
    
    [SerializeField]
    private ColorsConfig _colorsConfig;

    public static ColorsConfig ColorsConfig;
    public static TasksManager TasksManager;
    public static UITasksPanel UITasksPanel;

    private void Awake() {
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine() {
        ColorsConfig = _colorsConfig;
        _loadingPanel.SetActive(true);
        TasksManager = FindObjectOfType<TasksManager>();
        yield return TasksManager.Loading();

        UITasksPanel = FindObjectOfType<UITasksPanel>();
        yield return UITasksPanel.Loading();
        
        _loadingPanel.SetActive(false);
    }
}