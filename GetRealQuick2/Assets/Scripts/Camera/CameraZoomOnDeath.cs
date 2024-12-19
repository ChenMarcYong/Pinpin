using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraZoomOnDeath : MonoBehaviour
{
    [Header("Cinemachine Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomTargetSize = 4f; // Taille orthographique cible pour le zoom
    [SerializeField] private float zoomDuration = 1.5f; // Durée du zoom en secondes
    [SerializeField] private Transform player; // Référence au joueur

    private bool isZooming = false;

    public void TriggerZoomOnDeath()
    {
        if (!isZooming && virtualCamera != null)
        {
            StartCoroutine(ZoomCoroutine());
        }
    }

    private IEnumerator ZoomCoroutine()
    {
        isZooming = true;

        // Récupérer la taille orthographique actuelle
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        // Assurez-vous que la caméra suit encore le joueur
        if (player != null)
        {
            virtualCamera.Follow = player;
        }

        // Accéder au Framing Transposer
        CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        float startXOffset = 0f;

        if (framingTransposer != null)
        {
            startXOffset = framingTransposer.m_TrackedObjectOffset.x;
        }

        // Lancer l'interpolation pour effectuer le zoom et ajuster X
        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;

            // Interpoler entre la taille actuelle et la taille cible
            float t = elapsedTime / zoomDuration;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, zoomTargetSize, t);

            // Interpoler la valeur de X vers 0
            if (framingTransposer != null)
            {
                framingTransposer.m_TrackedObjectOffset.x = Mathf.Lerp(startXOffset, 0, t);
            }

            yield return null;
        }

        // Assurer que la taille orthographique finale est bien atteinte
        virtualCamera.m_Lens.OrthographicSize = zoomTargetSize;

        // Assurer que X est bien à 0
        if (framingTransposer != null)
        {
            framingTransposer.m_TrackedObjectOffset.x = 0;
        }

        isZooming = false;
    }
}
