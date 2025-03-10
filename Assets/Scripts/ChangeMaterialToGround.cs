using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public class ChangeMaterialToGround : MonoBehaviour
{
	[SerializeField] private Renderer _renderer;
	[SerializeField] private Material _defaultMaterial;
	[Space]
	[SerializeField] private string _changeTag = "Ground";

	private GameObject _currentTuch;

	private void OnEnable()
	{
		_currentTuch = null;
		_renderer.material = _defaultMaterial;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != _changeTag)
			return;

		GameObject collidedObject = collision.gameObject;

		if (collidedObject != _currentTuch)
		{
			Renderer groundRenderer = collidedObject.GetComponent<Renderer>();

			if (groundRenderer != null)
			{
				_renderer.material = groundRenderer.material;
				_currentTuch = collidedObject;
			}
		}
	}
}