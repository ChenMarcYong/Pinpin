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

        // Lancer l'interpolation pour effectuer le zoom
        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;

            // Interpoler entre la taille actuelle et la taille cible
            float t = elapsedTime / zoomDuration;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, zoomTargetSize, t);

            yield return null;
        }

        // Assurer que la taille orthographique finale est bien atteinte
        virtualCamera.m_Lens.OrthographicSize = zoomTargetSize;

        isZooming = false;
    }
}
