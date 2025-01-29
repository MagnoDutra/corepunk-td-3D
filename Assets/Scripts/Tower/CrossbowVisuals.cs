using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    private Enemy myEnemy;

    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField]
    private float attackVisualDuration = .1f;

    [Header("Glowing Visuals")]
    [SerializeField] private float maxIntensity = 150;
    private float currentIntensity;

    [Space]
    [SerializeField] private MeshRenderer meshRenderer;
    private Material material;

    [Space]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [Header("Rotor Visuals")]
    [SerializeField] private Transform rotor;
    [SerializeField] private Transform rotorUnloaded;
    [SerializeField] private Transform rotorLoaded;

    [Header("Front Glow String")]
    [SerializeField] private LineRenderer frontString_L;
    [SerializeField] private LineRenderer frontString_R;

    [Space]
    [SerializeField] private Transform frontStartPoint_L;
    [SerializeField] private Transform frontStartPoint_R;
    [SerializeField] private Transform frontEndPoint_L;
    [SerializeField] private Transform frontEndPoint_R;

    [Header("Back Glow String")]
    [SerializeField] private LineRenderer backString_L;
    [SerializeField] private LineRenderer backString_R;

    [Space]
    [SerializeField] private Transform backStartPoint_L;
    [SerializeField] private Transform backStartPoint_R;
    [SerializeField] private Transform backEndPoint_L;
    [SerializeField] private Transform backEndPoint_R;

    [SerializeField] private LineRenderer[] lineRenderes;

    private void Awake()
    {
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;

        UpdateMaterialsOnLineRenderers();

        StartCoroutine(ChangeEmission(1));
    }

    private void UpdateMaterialsOnLineRenderers()
    {
        foreach (var lr in lineRenderes)
        {
            lr.material = material;
        }
    }

    private void Update()
    {
        UpdateEmissionColor();

        UpdateStrings();

        UpdateAttackVisualsIfNeeded();
    }

    private void UpdateAttackVisualsIfNeeded()
    {
        if (attackVisuals.enabled && myEnemy != null)
        {
            attackVisuals.SetPosition(1, myEnemy.CenterPoint());
        }
    }

    private void UpdateStrings()
    {
        UpdateStringVisual(frontString_R, frontStartPoint_R, frontEndPoint_R);
        UpdateStringVisual(frontString_L, frontStartPoint_L, frontEndPoint_L);
        UpdateStringVisual(backString_L, backStartPoint_L, backEndPoint_L);
        UpdateStringVisual(backString_R, backStartPoint_R, backEndPoint_R);
    }

    private void UpdateEmissionColor()
    {
        // Dividimos aqui para ter o valor de 0 a 1
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);
        emissionColor = emissionColor * Mathf.LinearToGammaSpace(currentIntensity);
        material.SetColor("_EmissionColor", emissionColor);
    }

    public void PlayReloadFX(float duration)
    {
        float newDuration = duration / 2;
        StartCoroutine(ChangeEmission(newDuration));
        StartCoroutine(UpdateRotorPosition(newDuration));
    }

    public void PlayAttackFX(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint, newEnemy));
    }

    private IEnumerator FXCoroutine(Vector3 startPoint, Vector3 endpoint, Enemy newEnemy)
    {
        myEnemy = newEnemy;

        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endpoint);

        yield return new WaitForSeconds(attackVisualDuration);

        attackVisuals.enabled = false;
    }

    private IEnumerator ChangeEmission(float duration)
    {
        float startTime = Time.time;
        float startIntensity = 0;

        while (Time.time - startTime < duration)
        {
            // Calcula a proporçao de quanto tempo passou desde o começo da coroutine de 0 a 150
            float tValue = (Time.time - startTime) / duration;
            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, tValue);
            yield return null;
        }

        currentIntensity = maxIntensity;
    }

    private IEnumerator UpdateRotorPosition(float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float tValue = (Time.time - startTime) / duration;
            rotor.position = Vector3.Lerp(rotorUnloaded.position, rotorLoaded.position, tValue);
            yield return null;
        }

        rotor.position = rotorLoaded.position;
    }

    private void UpdateStringVisual(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
