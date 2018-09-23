using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WildcardSelector : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    [MenuItem("Window/Wildcard Selector")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WildcardSelector));
    }

    private void OnGUI()
    {
        GUILayout.Label("RegEx Search", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Pattern", myString);

        if(GUILayout.Button("Select"))
        {
            var list = new List<GameObject>();
            var scene = SceneManager.GetActiveScene();
            var rootObject = scene.GetRootGameObjects();
            var criteria = new Regex(myString);

            foreach(GameObject item in rootObject)
            {
                matchObjects(list, item, criteria);
            }

            Selection.objects = list.ToArray();
        }
    }

    void matchObjects(List<GameObject> list, GameObject item, Regex criteria)
    {
        if(criteria.IsMatch(item.name))
        {
            list.Add(item);
        }

        foreach(Transform child in item.transform)
        {
            matchObjects(list, child.gameObject, criteria);
        }
    }
}
