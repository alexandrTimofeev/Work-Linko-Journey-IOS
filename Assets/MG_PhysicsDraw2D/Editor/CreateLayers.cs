using UnityEngine;
using System.Collections;
using UnityEditor;


[InitializeOnLoad]
public static class LayerUtils
{
    static LayerUtils()
    {
        CreateLayer();
    }
    
    static void CreateLayer()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        SerializedProperty layers = tagManager.FindProperty("layers");
        
        if (layers == null || !layers.isArray)
        {
            Debug.LogWarning("Can't set up the layers.  It's possible the format of the layers and tags data has changed in this version of Unity.");
            Debug.LogWarning("Layers is null: " + (layers == null));
            return;
        }

        bool drawingLayer = false;

        for (int i = 0; i < layers.arraySize; i++)
        {
            SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
            if (layerSP.stringValue == "CurrentDrawing")
                drawingLayer = true;
        }
        
        for (int i = 8; i < layers.arraySize; i++)
        {
            SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
            if (drawingLayer == false && layerSP.stringValue == "")
            {
                layerSP.stringValue = "CurrentDrawing";
                drawingLayer = true;
            }
        }

        tagManager.ApplyModifiedProperties();
    }
}
