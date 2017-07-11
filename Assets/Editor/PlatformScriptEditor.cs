using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlatformScript))]
public class PlatformScriptEditor : Editor
{
	public PlatformScript script;

	public override void OnInspectorGUI()
	{
		script = (PlatformScript)target;

		DrawDefaultInspector();

		if(GUILayout.Button("Link all from here upwards"))
		{
			script.SetDegree(script.degree, script);
			script.ClickMe = false;
		}

		if(GUILayout.Button("Reset this platform"))
		{
			script.degree = 0;
			script.isGround = false;
			script.top.Clear();
			script.bottom.Clear();
			script.ClickMe = false;
		}

		if(GUI.changed)
			serializedObject.ApplyModifiedProperties();
	}
}
