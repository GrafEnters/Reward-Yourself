using System;
using TMPro;
using UnityEngine;

public class UITaskLine : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _descriptionText;

    [SerializeField]
    private TextMeshProUGUI _rewardText;

    private Task _task;

    private Action<Task> _onComplete;
    private Action<Task> _onEdit;
    private Action<Task> _onDelete;

    public void InitLine(Action<Task> complete, Action<Task> edit, Action<Task> delete) {
        _onComplete = complete;
        _onEdit = edit;
        _onDelete = delete;
    }

    public void SetData(Task task) {
        _task = task;
        _descriptionText.text = _task.Description;
        _rewardText.text = TimeUtils.GetShortStrFromSeconds(_task.Reward);
    }

    public void Complete() {
        _onComplete.Invoke(_task);
    }

    public void Edit() {
        _onEdit.Invoke(_task);
    }

    public void Delete() {
        _onDelete.Invoke(_task);
        Destroy(gameObject);
    }
}