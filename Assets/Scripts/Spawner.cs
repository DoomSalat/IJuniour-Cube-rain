using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
	[SerializeField] private PooledObject _prefab;
	[SerializeField] private BoxCollider _positionBox;
	[Space]
	[SerializeField][Min(0)] private float _rateTime = 2;
	[SerializeField] private Vector2Int _minMaxLiveTime = new Vector2Int(2, 5);
	[SerializeField][Min(1)] private int _poolMaxSize = 10;

	private ObjectPool<PooledObject> _pool;

	private void Awake()
	{
		InitializePool();
	}

	private void Start()
	{
		StartCoroutine(SpawnRate());
	}

	public void OnPoolTuch(PooledObject poolObj)
	{
		int liveTime = Random.Range(_minMaxLiveTime.x, _minMaxLiveTime.y);
		StartCoroutine(ReturnToPoolAfterTime(poolObj, liveTime));
	}

	private void InitializePool()
	{
		_pool = new ObjectPool<PooledObject>(
			createFunc: CreatePooledObject,
			actionOnGet: OnGetFromPool,
			actionOnRelease: OnReleaseToPool,
			actionOnDestroy: OnDestroyPooledObject,
			maxSize: _poolMaxSize
		);
	}

	private PooledObject CreatePooledObject()
	{
		return Instantiate(_prefab);
	}

	private void OnGetFromPool(PooledObject poolObj)
	{
		poolObj.transform.position = GetRandomPositionInBox();
		poolObj.TuchedReturn += OnPoolTuch;
		poolObj.gameObject.SetActive(true);
	}

	private void OnReleaseToPool(PooledObject poolObj)
	{
		poolObj.TuchedReturn -= OnPoolTuch;
		poolObj.gameObject.SetActive(false);
	}

	private void OnDestroyPooledObject(PooledObject poolObj)
	{
		poolObj.TuchedReturn -= OnPoolTuch;
		Destroy(poolObj.gameObject);
	}

	private IEnumerator SpawnRate()
	{
		var rateWait = new WaitForSeconds(_rateTime);

		while (true)
		{
			_pool.Get();

			yield return rateWait;
		}
	}

	private IEnumerator ReturnToPoolAfterTime(PooledObject poolObj, float time)
	{
		yield return new WaitForSeconds(time);

		_pool.Release(poolObj);
	}

	private Vector3 GetRandomPositionInBox()
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