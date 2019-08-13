using UnityEngine;

public class PostprocessBlur : MonoBehaviour {
	[Range(0.0f, 0.1f)]
	public float radius = 0.1f;
	public Shader blurShader; 
	private RenderTexture intermediateTexture = null;

	private Material material;
    
	private void Awake () {
		material = new Material(blurShader);
		intermediateTexture = new RenderTexture(1024, 1024, 24);
		intermediateTexture.Create();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination){
		material.SetFloat("_VertDist", 0);
		material.SetFloat("_HorDist", radius);
		Graphics.SetRenderTarget(intermediateTexture);
        GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
		Graphics.Blit(source, intermediateTexture, material);
		material.SetFloat("_VertDist", radius);
        material.SetFloat("_HorDist", 0);
		Graphics.SetRenderTarget(destination);
        GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
		Graphics.Blit(intermediateTexture, destination, material);
	}
}
