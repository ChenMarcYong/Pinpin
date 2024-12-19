using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    [Header("Cinemachine Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Transition Settings")]
    [SerializeField] private float startSize = 1f; // Taille initiale
    [SerializeField] private float targetSize = 8f; // Taille finale
    [SerializeField] private float holdDuration = 0.5f; // Temps où la taille reste fixe
    [SerializeField] private float transitionDuration = 2f; // Durée de la transition après le délai

    private Coroutine transitionCoroutine;

    void Start()
    {
        if (virtualCamera != null)
        {
            // Assurez-vous de stopper d'autres transitions si elles existent
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);

            // Lancer la transition
            transitionCoroutine = StartCoroutine(TransitionOrthoSizeWithAcceleration());
        }
    }

    private IEnumerator TransitionOrthoSizeWithAcceleration()
    {
        CinemachineVirtualCamera cam = virtualCamera.GetComponent<CinemachineVirtualCamera>();
        if (cam == null) yield break;

        // Étape 1 : Rester sur la taille initiale pendant "holdDuration"
        cam.m_Lens.OrthographicSize = startSize;
        yield return new WaitForSeconds(holdDuration);

        // Étape 2 : Transition progressive avec accélération
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            // Utilisation d'une courbe exponentielle pour l'accélération (ease-in)
            float t = elapsedTime / transitionDuration; // Progression normalisée (0 à 1)
            t = Mathf.Pow(t, 2); // Appliquer une fonction d'accélération (quadratique)

            cam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null; // Attendre le frame suivant
        }

        // Assurer la valeur finale
        cam.m_Lens.OrthographicSize = targetSize;
    }
}
