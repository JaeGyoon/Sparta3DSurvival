using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private Image indicatorImage;
    [SerializeField] private float flashSpeed = 0.2f;

    [SerializeField] private Coroutine coroutine;

    private void Awake()
    {
        indicatorImage = GetComponent<Image>();
    }

    void Start()
    {
        CharacterManager.Instance.Player.playerCondition.onTakeDamage += Flash;
    }

    private void Flash()
    {
        if ( coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        indicatorImage.enabled = true;
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(flashSpeed);

        indicatorImage.enabled = false;
    }
}
