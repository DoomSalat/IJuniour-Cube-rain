using UnityEngine;
using System.Collections;

public class MaterialFade : MonoBehaviour
{
	[SerializeField] private MeshRenderer[] _meshRenderers;
	private Material[] _instanceMaterials;
	private float[] _initialAlphas;
	private Coroutine _fadeCoroutine;

	private void Awake()
	{
		_instanceMaterials = new Material[_meshRenderers.Length];
		_initialAlphas = new float[_meshRenderers.Length];

		for (int i = 0; i < _meshRenderers.Length; i++)
		{
			if (_meshRenderers[i] != null)
			{
				_instanceMaterials[i] = new Material(_meshRenderers[i].material);
				_meshRenderers[i].material = _instanceMaterials[i];
				_initialAlphas[i] = _instanceMaterials[i].color.a;
			}
		}
	}

	private void OnEnable()
	{
		ResetAlpha();
	}

	private void OnDestroy()
	{
		for (int i = 0; i < _instanceMaterials.Length; i++)
		{
			if (_instanceMaterials[i] != null)
			{
				Destroy(_instanceMaterials[i]);
			}
		}
	}

	public void StartFade(float duration)
	{
		if (_fadeCoroutine != null)
		{
			StopCoroutine(_fadeCoroutine);
		}

		_fadeCoroutine = StartCoroutine(FadeAlpha(duration));
	}

	public void ResetAlpha()
	{
		if (_fadeCoroutine != null)
		{
			StopCoroutine(_fadeCoroutine);
			_fadeCoroutine = null;
		}

		for (int i = 0; i < _instanceMaterials.Length; i++)
		{
			if (_instanceMaterials[i] != null)
			{
				Color resetColor = _instanceMaterials[i].color;
				resetColor.a = _initialAlphas[i];
				_instanceMaterials[i].color = resetColor;
			}
		}
	}

	private IEnumerator FadeAlpha(float duration)
	{
		Color[] currentColors = new Color[_instanceMaterials.Length];
		float[] startAlphas = new float[_instanceMaterials.Length];

		for (int i = 0; i < _instanceMaterials.Length; i++)
		{
			if (_instanceMaterials[i] != null)
			{
				currentColors[i] = _instanceMaterials[i].color;
				startAlphas[i] = currentColors[i].a;
			}
		}

		float timeElapsed = 0f;

		while (timeElapsed < duration)
		{
			timeElapsed += Time.deltaTime;

			for (int i = 0; i < _instanceMaterials.Length; i++)
			{
				if (_instanceMaterials[i] != null)
				{
					float newAlpha = Mathf.Lerp(startAlphas[i], 0, timeElapsed / duration);
					currentColors[i].a = newAlpha;
					_instanceMaterials[i].color = currentColors[i];
				}
			}

			yield return null;
		}

		for (int i = 0; i < _instanceMaterials.Length; i++)
		{
			if (_instanceMaterials[i] != null)
			{
				currentColors[i].a = 0;
				_instanceMaterials[i].color = currentColors[i];
			}
		}
	}
}