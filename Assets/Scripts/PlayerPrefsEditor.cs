using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class PlayerPrefsEditor : Editor {
    [MenuItem("MyMenu/Delete PlayerPrefs")]
    static void DoSomething() {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs deleted successfully");
    }
}
#endif