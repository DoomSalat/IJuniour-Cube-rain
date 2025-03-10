using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class PooledObject : MonoBehaviour
{
	private Rigidbody _selfRigidbody;
	private Spawner _spawner;

	private bool _isCollide;

	public void Initializate(Spawner spawner)
	{
		_spawner = spawner;
	}

	private void OnEnable()
	{
		_isCollide = false;

		if (_selfRigidbody == null)
			_selfRigidbody = GetComponent<Rigidbody>();

		_selfRigidbody.linearVelocity = Vector3.zero;
		_selfRigidbody.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (_isCollide)
			return;

		_isCollide = true;
		_spawner.TimerReturn(gameObject);
	}
}
