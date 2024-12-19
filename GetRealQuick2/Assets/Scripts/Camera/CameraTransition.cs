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
    [SerializeField] private float holdDuration = 0.5f; // Temps o� la taille reste fixe
    [SerializeField] private float transitionDuration = 2f; // Dur�e de la transition apr�s le d�lai

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

        // �tape 1 : Rester sur la taille initiale pendant "holdDuration"
        cam.m_Lens.OrthographicSize = startSize;
        yield return new WaitForSeconds(holdDuration);

        // �tape 2 : Transition progressive avec acc�l�ration
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            // Utilisation d'une courbe exponentielle pour l'acc�l�ration (ease-in)
            float t = elapsedTime / transitionDuration; // Progression normalis�e (0 � 1)
            t = Mathf.Pow(t, 2); // Appliquer une fonction d'acc�l�ration (quadratique)

            cam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null; // Attendre le frame suivant
        }

        // Assurer la valeur finale
        cam.m_Lens.OrthographicSize = targetSize;
    }
}
