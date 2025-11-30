using UnityEngine;

public class AmiView : MonoBehaviour
{
    public enum AmiState
    {
        Idle, //0
        Moving, //1
        Other, //2
    }
    public enum AmiPhase
    {
        Phase1,
        Phase2
    }
    [Header("Animations Keys")]
    [SerializeField] private string stateAnimationKey = "State";
    [SerializeField] private string phaseAnimationKey = "Phase";
    [SerializeField] private string deathAnimationKey = "Death";
    [SerializeField] private string hitAnimationKey = "Hit";
    [SerializeField] private string attackAnimationKey = "Attack";

    private AmiState currentState;
    private AmiPhase currentPhase;
    private Animator animator;
    private AmiController amiController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        amiController = GetComponent<AmiController>();
    }

    public void SetAmiIdle()
    {
        currentState = AmiState.Idle;
        UpdateAmiState();
    }
    public void SetAmiMoving()
    {
        currentState = AmiState.Moving;
        UpdateAmiState();
    }
    public void SetAmiAttacking()
    {
        animator.SetTrigger(attackAnimationKey);
        currentState = AmiState.Other;
        UpdateAmiState();
    }
    public void SetAmiHit()
    {
        animator.SetTrigger(hitAnimationKey);
        currentState = AmiState.Other;
        UpdateAmiState();
    }
    public void SetAmiDeath()
    {
        animator.SetTrigger(deathAnimationKey);
        currentState = AmiState.Other;
        UpdateAmiState();
    }
    private void UpdateAmiState()
    {
        animator.SetInteger(stateAnimationKey, (int)currentState);
    }
    public void UpdateAmiPhase()
    {
        currentPhase = AmiFightManager.Instance.currentPhase == 1 ? AmiPhase.Phase1 : AmiPhase.Phase2;
        animator.SetInteger(phaseAnimationKey, (int)currentPhase);
    }
}
