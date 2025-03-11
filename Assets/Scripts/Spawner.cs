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

	private ObjectPool<GameObject> _pool;

	private void Start()
	{
		InitializePool();
		StartCoroutine(SpawnRate());
	}

	public void StartTimerReturn(GameObject poolObj)
	{
		int liveTime = Random.Range(_minMaxLiveTime.x, _minMaxLiveTime.y);
		StartCoroutine(ReturnToPoolAfterTime(poolObj, liveTime));
	}

	private void InitializePool()
	{
		_pool = new ObjectPool<GameObject>(
			createFunc: CreatePooledObject,
			actionOnGet: OnGetFromPool,
			actionOnRelease: OnReleaseToPool,
			actionOnDestroy: OnDestroyPooledObject,
			maxSize: _poolMaxSize
		);
	}

	private GameObject CreatePooledObject()
	{
		PooledObject pooledObject = Instantiate(_prefab);
		pooledObject.Initializate(this);
		return pooledObject.gameObject;
	}

	private void OnGetFromPool(GameObject obj)
	{
		obj.transform.position = GetRandomPositionInBox();
		obj.SetActive(true);
	}

	private void OnReleaseToPool(GameObject obj)
	{
		obj.SetActive(false);
	}

	private void OnDestroyPooledObject(GameObject obj)
	{
		Destroy(obj);
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

	private IEnumerator ReturnToPoolAfterTime(GameObject obj, float time)
	{
		yield return new WaitForSeconds(time);

		_pool.Release(obj);
	}

	private Vector3 GetRandomPositionInBox()
	{
		const float halfSize = 0.5f;

		Vector3 randomLocalPosition = new Vector3(
			Random.Range(-halfSize, halfSize),
			Random.Range(-halfSize, halfSize),
			Random.Range(-halfSize, halfSize)
		);

		randomLocalPosition = Vector3.Scale(randomLocalPosition, _positionBox.size);

		return _positionBox.transform.TransformPoint(_positionBox.center + randomLocalPosition);
	}
}