using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class FirstTouchDetector : MonoBehaviour
{
	private Rigidbody _rigidbody;
	private bool _isCollide;

	public event System.Action<Collision> Tuched;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void OnEnable()
	{
		_isCollide = false;

		ResetVelocity();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (_isCollide)
			return;

		_isCollide = true;
		Tuched?.Invoke(collision);
	}

	private void ResetVelocity()
	{
		_rigidbody.linearVelocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}
}
