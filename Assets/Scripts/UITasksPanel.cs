using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITasksPanel : MonoBehaviour, ILoadable {
    [SerializeField]
    private UITaskLine _taskLinePrefab;

    [SerializeField]
    private Transform _linesHolder;

    [SerializeField]
    private UIEditTaskPanel _uiEditTaskPanel;

    [SerializeField]
    private Text _timeText;

    private Dictionary<Task, UITaskLine> _lines;

    public void AddNewTask() {
        Task task = new Task();
        Loader.TasksManager.AddTask(task);
        _uiEditTaskPanel.OpenWithTask(task);
        CreateTaskLine(task);
    }

    public void EditTask(Task task) {
        _lines[task].SetData(task);
    }

    private void CompleteTask(Task task) {
        Loader.TasksManager.CompleteTask(task);
        UpdateTimeText();
    }

    public IEnumerator Loading() {
        _lines = new Dictionary<Task, UITaskLine>();
        //TODO Prewarm Pool here

        TasksManager tskManager = Loader.TasksManager;
        foreach (var task in tskManager.Profile.Tasks) {
            CreateTaskLine(task);
            yield return new WaitForEndOfFrame();
        }

        UpdateTimeText();
    }

    private UITaskLine CreateTaskLine(Task task) {
        UITaskLine line = Instantiate(_taskLinePrefab, _linesHolder);
        line.InitLine(CompleteTask, _uiEditTaskPanel.OpenWithTask, DeleteTask);
        line.SetData(task);
        _lines.Add(task, line);
        return line;
    }

    private void DeleteTask(Task task) {
        _lines.Remove(task);
        Loader.TasksManager.DeleteTask(task);
    }

    public void UpdateTimeText() {
        int seconds = Loader.TasksManager.Profile.TimeAmount;
        _timeText.text = "<b>" + TimeUtils.GetStrFromSeconds(seconds) + "</b>";
    }
}