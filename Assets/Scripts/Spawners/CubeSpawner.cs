using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class CubeSpawner : Spawner<PooledCube>
{
	[SerializeField] private BoxCollider _positionBox;
	[SerializeField][Min(0)] private float _rateTime = 2;
	[SerializeField] private BombSpawner _bombSpawner;

	private void Start()
	{
		StartCoroutine(SpawnRate());
	}

	protected override void OnGetFromPool(PooledCube cube)
	{
		base.OnGetFromPool(cube);
		cube.StateReturned += _bombSpawner.SetLastCubeTransform;
	}

	protected override void OnReleaseToPool(PooledCube cube)
	{
		base.OnReleaseToPool(cube);
		cube.StateReturned -= _bombSpawner.SetLastCubeTransform;
	}

	private IEnumerator SpawnRate()
	{
		var rateWait = new WaitForSeconds(_rateTime);

		while (true)
		{
			Pool.Get();

			yield return rateWait;
		}
	}

	protected override Vector3 GetSpawnPosition()
	{
		const float HalfSize = 0.5f;

		Vector3 randomLocalPosition = new Vector3(
			Random.Range(-HalfSize, HalfSize),
			Random.Range(-HalfSize, HalfSize),
			Random.Range(-HalfSize, HalfSize)
		);

		randomLocalPosition = Vector3.Scale(randomLocalPosition, _positionBox.size);

		return _positionBox.transform.TransformPoint(_positionBox.center + randomLocalPosition);
	}
}