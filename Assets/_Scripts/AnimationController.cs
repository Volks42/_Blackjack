using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Sprite newSprite;
    private Vector3 targetPosition;
    private Animator animator;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    //called from animation timeline at card flip
    public void ChangeSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    //updates data for animation
    public void AnimFunc(Sprite cardSprite, bool playerTurn, Vector3 targetVector)
    {
        GameLogic.AnimPlaying = true;
        newSprite = cardSprite;
        if (playerTurn)
        { animator.SetBool("PlayerDeal", true); }
        else
        { animator.SetBool("DealerDeal", true); }
        this.targetPosition = targetVector;
    }

    //called from animator timeline to trigger coroutine below
    void StartCoroutine()
    {
        if (targetPosition != Vector3.zero)
        {
            StartCoroutine(PlaceCard(targetPosition));
        }
    }

    IEnumerator PlaceCard(Vector3 target)
    {
        while (this.transform.position != target)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, 0.15f);
            yield return null;
        }
        GameLogic.AnimPlaying = false;
    }
}
