using System.Collections;
using UnityEngine;
using TMPro;

public class CompanionController : MonoBehaviour
{
    public enum CompanionState
    {
        Following,
        Investigating,
        WaitingForPlayer,
        Explaining,
        Idle
    }

    public CompanionState currentState = CompanionState.Following;

    [Header("References")]
    public Transform player;
    public Transform nest;

    [Header("Movement")]
    public float followDistance = 2.5f;
    public float moveSpeed = 3f;

    [Header("Floating")]
    public float floatHeight = 2f;
    public float floatAmount = 0.2f;
    public float floatSpeed = 1.5f;

    private Transform investigationTarget;

    [Header("Dialogue")]
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;

    void Update()
    {
        switch (currentState)
        {
            case CompanionState.Following:
                FollowPlayer();
                break;

            case CompanionState.Investigating:
                MoveToTarget();
                break;

            case CompanionState.WaitingForPlayer:
                WaitForPlayer();
                break;
        }
    }

    // ================= FOLLOW =================
    void FollowPlayer()
    {
        if (player == null) return;

        Vector3 targetXZ = player.position + player.right * followDistance;

        Vector3 current = transform.position;

        Vector3 newXZ = Vector3.Lerp(
            new Vector3(current.x, 0, current.z),
            new Vector3(targetXZ.x, 0, targetXZ.z),
            moveSpeed * Time.deltaTime
        );

        float newY = floatHeight + Mathf.Sin(Time.time * floatSpeed) * floatAmount;

        transform.position = new Vector3(newXZ.x, newY, newXZ.z);

        RotateTowards(player.position);
    }

    // ================= INVESTIGATE =================
    void MoveToTarget()
    {
        if (investigationTarget == null) return;

        Vector3 targetXZ = investigationTarget.position;

        Vector3 current = transform.position;

        Vector3 newXZ = Vector3.Lerp(
            new Vector3(current.x, 0, current.z),
            new Vector3(targetXZ.x, 0, targetXZ.z),
            moveSpeed * Time.deltaTime
        );

        float newY = floatHeight + Mathf.Sin(Time.time * floatSpeed) * floatAmount;

        transform.position = new Vector3(newXZ.x, newY, newXZ.z);

        // ✅ FIXED distance check (ignore Y)
        Vector3 flatPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatTarget = new Vector3(targetXZ.x, 0, targetXZ.z);

        if (Vector3.Distance(flatPos, flatTarget) < 1.5f)
        {
            currentState = CompanionState.WaitingForPlayer;
        }
    }

    // ================= WAIT =================
    void WaitForPlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < 2f)
        {
            // Prevent multiple coroutine calls
            if (currentState != CompanionState.Explaining)
            {
                StartCoroutine(ExplainWithDelay());
            }
        }
    }

    // ================= DIALOGUE =================
    IEnumerator ExplainWithDelay()
    {
        currentState = CompanionState.Explaining;

        RotateTowards(nest.position);

        yield return new WaitForSeconds(1.5f);

        ShowDialogue("It looks like the nest is broken. We need twigs, leaves, and dry grass.");
    }

    void ShowDialogue(string message)
    {
        if (dialogueUI == null || dialogueText == null) return;

        dialogueUI.SetActive(true);

        // ✅ FIX: Don't kill other coroutines
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string message)
    {
        dialogueText.text = "";

        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(3f);
        HideDialogue();
    }

    void HideDialogue()
    {
        dialogueUI.SetActive(false);
        currentState = CompanionState.Following;
    }

    // ================= ROTATION =================
    void RotateTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }

    // ================= EXTERNAL =================
    public void Investigate(Transform target)
    {
        investigationTarget = target;
        currentState = CompanionState.Investigating;
    }
}