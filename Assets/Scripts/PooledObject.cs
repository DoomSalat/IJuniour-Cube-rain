using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class PooledObject : MonoBehaviour
{
	private Rigidbody _selfRigidbody;
	private Spawner _spawner;

	private bool _isCollide;

	private void Awake()
	{
		_selfRigidbody = GetComponent<Rigidbody>();
	}

	private void OnEnable()
	{
		_isCollide = false;

		_selfRigidbody.linearVelocity = Vector3.zero;
		_selfRigidbody.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (_isCollide)
			return;

		_isCollide = true;
		_spawner.StartTimerReturn(gameObject);
	}

	public void Initializate(Spawner spawner)
	{
		_spawner = spawner;
	}
}
