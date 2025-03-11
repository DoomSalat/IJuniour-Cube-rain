using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Ground : MonoBehaviour
{
	private Renderer _selfRenderer;

	private void Awake()
	{
		_selfRenderer = GetComponent<Renderer>();
	}

	public Material GetMaterial()
	{
		return _selfRenderer.material;
	}
}
