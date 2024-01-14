using TMPro;
using UnityEngine;

public class UIEditTaskPanel : MonoBehaviour {
    private Task _task;

    [SerializeField]
    private TMP_InputField _descriptionInput;

    [SerializeField]
    private TMP_InputField _rewardInput;

    public void OpenWithTask(Task task) {
        _task = task;
        _descriptionInput.SetTextWithoutNotify(task.Description);
        _rewardInput.SetTextWithoutNotify(TimeUtils.GetShortStrFromSeconds(_task.Reward));
        gameObject.SetActive(true);
    }

    public void EndEditDescription(string str) {
        _task.Description = str;
    }

    public void EndEditReward(string str) {
        _task.Reward = int.Parse(str) * 60;
    }
    
    public void AddButton(int amount) {
        _task.Reward += amount;
        if (_task.Reward < 0) {
            _task.Reward = 0;
        }
        _rewardInput.SetTextWithoutNotify(TimeUtils.GetShortStrFromSeconds(_task.Reward));
    }

    public void EndEdit() {
        Loader.TasksManager.EndEditTask(_task);
        Loader.UITasksPanel.EditTask(_task);
        gameObject.SetActive(false);
    }
}