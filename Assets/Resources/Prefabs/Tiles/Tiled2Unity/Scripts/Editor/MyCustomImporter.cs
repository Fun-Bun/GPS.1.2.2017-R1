using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tiled2Unity.CustomTiledImporter]
class MyCustomImporter : Tiled2Unity.ICustomTiledImporter
{
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		// Do nothing
	}

	public void CustomizePrefab(GameObject prefab)
	{
		// Go through every rederer in the prefab and assign its shader to something else
		foreach (var renderer in prefab.GetComponentsInChildren<MeshRenderer>())
		{
			Material material = renderer.sharedMaterial; // Might have to be renderer.Material instead?
			material.shader = Shader.Find("Unlit/Transparent");
		}
	}
}
