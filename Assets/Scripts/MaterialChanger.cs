using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public class MaterialChanger : MonoBehaviour
{
	[SerializeField] private Renderer _renderer;
	[SerializeField] private Material _defaultMaterial;

	private bool _isTuch;

	private void OnEnable()
	{
		_isTuch = false;
		_renderer.material = _defaultMaterial;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (_isTuch)
			return;

		if (collision.gameObject.TryGetComponent(out Ground ground))
		{
			_isTuch = true;
			_renderer.material = ground.GetMaterial();
		}
	}
}