using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public class ChangeMaterialToGround : MonoBehaviour
{
	[SerializeField] private Renderer _renderer;
	[SerializeField] private Material _defaultMaterial;

	private GameObject _currentTuch;

	private void OnEnable()
	{
		_currentTuch = null;
		_renderer.material = _defaultMaterial;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject == _currentTuch)
			return;

		if (collision.gameObject.TryGetComponent(out Ground ground))
		{
			_renderer.material = ground.GetMaterial();
			_currentTuch = collision.gameObject;
		}
	}
}