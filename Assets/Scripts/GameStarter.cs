using UnityEngine;
using Unity.Cinemachine;

public class GameStarter : MonoBehaviour
{
    public CinemachineCamera introCamera;
    public CinemachineCamera gameplayCamera;
    public GameObject standButton;
    public PlayerMovement playerMovement;

    public void StartGame()
    {
        //lower intro camera priority to switch to gameplay camera
        introCamera.Priority = 10;
        //raise gameplay camera priority to switch to it
        gameplayCamera.Priority = 20;
        //hide the stand button
        standButton.SetActive(false);
        //enable player movement
        playerMovement.enabled = true;
    }
}
