using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : MonoBehaviour, ILoadable {
    public Profile Profile;

    private void Save() {
        string json = JsonUtility.ToJson(Profile);
        PlayerPrefs.SetString("profile", json);
    }

    public IEnumerator Loading() {
        Load();
        yield break;
    }

    private void Load() {
        if (!PlayerPrefs.HasKey("profile")) {
            CreateProfile();
            Save();
            return;
        }

        string json = PlayerPrefs.GetString("profile");
        Profile = JsonUtility.FromJson<Profile>(json);
    }

    private void CreateProfile() {
        Profile = new Profile();
        Profile.Tasks = new List<Task>();
        Profile.CompletedTasks = new List<Task>();
    }

    public void AddTask(Task task) {
        Profile.Tasks.Add(task);
        Save();
    }

    public void DeleteTask(Task task) {
        Profile.Tasks.Remove(Profile.Tasks.Find(t => t.ID == task.ID));
        Save();
    }

    public void EndEditTask(Task task) {
        Save();
    }

    public void CompleteTask(Task task) {
        Profile.Tasks.Remove(task);
        Profile.CompletedTasks.Add(task);
        Profile.TimeAmount += task.Reward;
        Save();
    }

    public void SaveTime(int newTime) {
        Profile.TimeAmount = newTime;
        Save();
    }
}

[System.Serializable]
public class Profile {
    public int TimeAmount;
    public List<Task> Tasks;
    public List<Task> CompletedTasks;
}

[System.Serializable]
public class Task {
    public string ID;
    public string Description;
    public int Reward;

    public Task() {
        ID = DateTime.Now.ToString();
        Description = "";
        Reward = 60;
    }
}