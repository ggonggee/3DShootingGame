using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Character/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    [Header("Jump")]
    public float JumpPower = 8f;
    public int MaxJumpCount = 2;

    [Header("Movement")]
    public float WalkSpeed = 5f;
    public float SprintSpeed = 9f;
    public float ClimbSpeed = 3f;

    [Header("Stamina")]
    public float MaxStamina = 5f;
    public float DrainPerSecondClimb = 1f;
    public float DrainPerSecondSprint = 1.5f;
    public float RecoverPerSecond = 2f;

    [Header("Roll")]
    public float RollSpeed = 15f;
    public float RollDuration = 0.3f;
    public float RollStaminaCost = 1.5f;
}