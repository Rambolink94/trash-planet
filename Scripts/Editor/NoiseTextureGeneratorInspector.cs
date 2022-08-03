using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseTextureGenerator))]
public class NoiseTextureGeneratorInspector : Editor
{
	private NoiseTextureGenerator textureGenerator;

	private void OnEnable()
	{
		textureGenerator = target as NoiseTextureGenerator;
		Undo.undoRedoPerformed += RefreshCreator;
	}

	private void OnDisable()
	{
		Undo.undoRedoPerformed -= RefreshCreator;
	}

	private void RefreshCreator()
	{
		if (Application.isPlaying)
		{
			//textureGenerator.FillTexture();
		}
	}

	public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck() && Application.isPlaying)
        {
            //(target as NoiseTextureGenerator).FillTexture();
        }
    }
}
