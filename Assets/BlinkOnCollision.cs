using System.Collections;
using UnityEngine;

public class BlinkOnCollision : MonoBehaviour
{
    [SerializeField]
    private Material blinkMaterial;

    [SerializeField]
    private Vector3 blinkScale;

    [SerializeField]
    private float blinkDuration = 0.1f;

    [SerializeField]
    private int blinkCount = 2;

    private Renderer componentRenderer;
    private Material originalMaterial;

    private Vector3 originalScale;

    void Start()
    {
        componentRenderer = GetComponent<Renderer>();
        originalMaterial = componentRenderer.material;
        originalScale = transform.localScale;
    }

    public void CollisionBlink()
    {
        StartCoroutine(BlinkCoroutine());
    }
    private IEnumerator BlinkCoroutine()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            componentRenderer.material = blinkMaterial;
            //transform.localScale = blinkScale;
            yield return new WaitForSeconds(blinkDuration);
            componentRenderer.material = originalMaterial;
            //transform.localScale = originalScale;
            yield return new WaitForSeconds(blinkDuration);
        }
    }
}