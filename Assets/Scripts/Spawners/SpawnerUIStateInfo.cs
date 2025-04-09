using TMPro;
using UnityEngine;

public class SpawnerUIStateInfo : MonoBehaviour
{
	[SerializeField] private SpawnerBase _spawner;
	[Header("UI")]
	[SerializeField] private TextMeshProUGUI _totalSpawnedText;
	[SerializeField] private TextMeshProUGUI _createdObjectsText;
	[SerializeField] private TextMeshProUGUI _activeObjectsText;

	private void OnEnable()
	{
		_spawner.StateUpdate += UpdateUI;
	}

	private void OnDisable()
	{
		_spawner.StateUpdate -= UpdateUI;
	}

	private void UpdateUI(float totalSpawned, float countAll, float countActive)
	{
		if (_totalSpawnedText != null)
			_totalSpawnedText.text = $"Total Spawned: {totalSpawned}";

		if (_createdObjectsText != null)
			_createdObjectsText.text = $"Created Objects: {countAll}";

		if (_activeObjectsText != null)
			_activeObjectsText.text = $"Active Objects: {countActive}";
	}
}
