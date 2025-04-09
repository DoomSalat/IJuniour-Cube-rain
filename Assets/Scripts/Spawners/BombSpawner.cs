using UnityEngine;

public class BombSpawner : Spawner<PooledBomb>
{
	private Vector3 _lastCubePosition = Vector3.zero;
	private Quaternion _lastCubeRotation = Quaternion.identity;
	private Rigidbody _lastRigidbody;

	public void SetLastCubeTransform(Vector3 position, Quaternion rotation, Rigidbody rigidbody)
	{
		_lastCubePosition = position;
		_lastCubeRotation = rotation;
		_lastRigidbody = rigidbody;

		Pool.Get();
	}

	protected override Vector3 GetSpawnPosition()
	{
		return _lastCubePosition;
	}

	protected override Quaternion GetSpawnRotation()
	{
		return _lastCubeRotation;
	}

	protected override void OnGetFromPool(PooledBomb poolObj)
	{
		base.OnGetFromPool(poolObj);
		poolObj.OnSpawn(GetRandomLiveTime());
		poolObj.SetRigidbodyState(_lastRigidbody);
	}
}