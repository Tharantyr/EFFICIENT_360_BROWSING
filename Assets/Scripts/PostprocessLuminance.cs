using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostprocessLuminance : MonoBehaviour {
	public float multiplier = 1.0f;
	public Shader luminanceShader;

	private Material material;
    
	private void Awake () {
		material = new Material(luminanceShader);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Multiplier", multiplier);
		Graphics.Blit(source, destination, material);
	}
}
