using UnityEngine;

public class Exploser : MonoBehaviour
{
	[SerializeField] private float _force = 500f;
	[SerializeField] private float _radius = 5f;

	[ContextMenu(nameof(Explode))]
	public void Explode()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

		foreach (Collider hit in colliders)
		{
			if (hit.TryGetComponent<Rigidbody>(out var hitedRigidbody))
			{
				hitedRigidbody.AddExplosionForce(_force, transform.position, _radius);
			}
		}

		Destroy(gameObject);
	}
}