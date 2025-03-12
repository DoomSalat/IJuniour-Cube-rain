using UnityEngine;

[RequireComponent(typeof(FirstTouchDetector), typeof(Renderer))]
public class MaterialChanger : MonoBehaviour
{
	[SerializeField] private Material _defaultMaterial;

	private FirstTouchDetector _tucher;
	private Renderer _renderer;

	private void Awake()
	{
		_tucher = GetComponent<FirstTouchDetector>();
		_renderer = GetComponent<Renderer>();
	}

	private void OnEnable()
	{
		_renderer.material = _defaultMaterial;
		_tucher.Tuched += OnTuchChangeMaterial;
	}

	private void OnDisable()
	{
		_tucher.Tuched -= OnTuchChangeMaterial;
	}

	private void OnTuchChangeMaterial(Collision collision)
	{
		if (collision.gameObject.TryGetComponent(out Ground ground))
		{
			_renderer.material = ground.GetMaterial();
		}
	}
}