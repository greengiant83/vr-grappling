using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AutoMass : EditorWindow
{
    [MenuItem("Window/Auto Mass")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AutoMass));
    }

    float? minSize = null;
    float? maxSize = null;

    string minVolumeTxt = "0";
    string maxVolumeTxt = "1";
    string minDragTxt = "0";
    string maxDragTxt = "1";
    string minMassTxt = "0.1";
    string maxMassTxt = "1";


    private void OnGUI()
    {
        GUILayout.Label("Min: " + minSize);
        GUILayout.Label("Max: " + maxSize);

        minVolumeTxt = EditorGUILayout.TextField("Min Volume", minVolumeTxt);
        maxVolumeTxt = EditorGUILayout.TextField("Max Volume", maxVolumeTxt);
        minDragTxt = EditorGUILayout.TextField("Min Drag", minDragTxt);
        maxDragTxt = EditorGUILayout.TextField("Max Drag", maxDragTxt);
        minMassTxt = EditorGUILayout.TextField("Min Mass", minMassTxt);
        maxMassTxt = EditorGUILayout.TextField("Max Mass", maxMassTxt);

        float minVolume, maxVolume;
        float minMass, maxMass;
        float minDrag, maxDrag;
        float.TryParse(minVolumeTxt, out minVolume);
        float.TryParse(maxVolumeTxt, out maxVolume);
        float.TryParse(minMassTxt, out minMass);
        float.TryParse(maxMassTxt, out maxMass);
        float.TryParse(minDragTxt, out minDrag);
        float.TryParse(maxDragTxt, out maxDrag);

        if (GUILayout.Button("Get Size"))
        {
            var bodies = Selection.GetFiltered<Rigidbody>(SelectionMode.Editable);
            foreach (var body in bodies)
            {
                var mesh = body.gameObject.GetComponent<MeshRenderer>();
                var volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
                if (volume > 0)
                {
                    if (!minSize.HasValue || volume < minSize) minSize = volume;
                    if (!maxSize.HasValue || volume > maxSize) maxSize = volume;
                }
            }
        }

        if(GUILayout.Button("Calculate"))
        {
            var bodies = Selection.GetFiltered<Rigidbody>(SelectionMode.Editable);
            foreach (var body in bodies)
            {
                var mesh = body.gameObject.GetComponent<MeshRenderer>();
                var volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
                if (volume > 0)
                {
                    body.drag = volume.Remap(maxVolume, minVolume, minDrag, maxDrag, true);
                    body.mass = volume.Remap(minVolume, maxVolume, minMass, maxMass, true);
                }
            }
        }
    }
}
