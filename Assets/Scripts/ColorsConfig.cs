using UnityEngine;

[CreateAssetMenu(fileName = "ColorsConfig", menuName = "MyScriptables/ColorsConfig", order = 0)]
public class ColorsConfig : ScriptableObject {
    [SerializeField]
    public Color Main, Secondary, Complete, Edit, Delete;

    public Color GetColor(Colors type) {
        return type switch {
            Colors.Main => Main,
            Colors.Secondary => Secondary,
            Colors.Complete => Complete,
            Colors.Edit => Edit,
            Colors.Delete => Delete,
            _ => Main
        };
    }
}

[System.Serializable]
public enum Colors {
    Main = 0,
    Secondary,
    Complete,
    Edit,
    Delete
}