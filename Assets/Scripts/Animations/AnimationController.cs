using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public SpriteManager spriteManager;
    public GameObject StatusPrefab;

    private Queue<IEnumerator> temporaryQueue = new Queue<IEnumerator>(); // Temporary queue for incoming animations
    private Queue<IEnumerator> playingQueue = new Queue<IEnumerator>();   // Queue for animations being played
    public float enqueueCooldown = 0.1f;    // Delay between moving animations from temporaryQueue to playingQueue

    private void Start()
    {
        // Start the coroutine to transfer animations from temporaryQueue to playingQueue
        StartCoroutine(TransferAnimationsToPlayingQueue());
    }

    public void TriggerAnimation(string value, string iconName, bool isPlayer)
    {
        // Create the animation object
        GameObject statusObject;
        RectTransform statusOrigin;
        if (isPlayer)
        { statusOrigin = GameObject.Find("PlayerTickOrigin").GetComponent<RectTransform>(); }
        else
        { statusOrigin = GameObject.Find("EnemyTickOrigin").GetComponent<RectTransform>(); }

        statusObject = Instantiate(StatusPrefab, statusOrigin);
        statusObject.SetActive(false); // Set it inactive until the animation starts
        UnityEngine.UI.Image statusImage = statusObject.GetComponent<UnityEngine.UI.Image>();
        statusImage.sprite = spriteManager.GetSprite(iconName);

        TextMeshProUGUI statusText = statusObject.GetComponentInChildren<TextMeshProUGUI>();
        statusText.text = value;

        // Get the animator and create the animation coroutine
        Animator localAnimator = statusObject.GetComponent<Animator>();
        IEnumerator animationCoroutine = PlayAnimationAndWait(localAnimator, statusObject);

        // Add the animation to the temporary queue
        temporaryQueue.Enqueue(animationCoroutine);
    }
    private IEnumerator TransferAnimationsToPlayingQueue()
    {
        while (true)
        {
            // If there are animations in the temporary queue, move one to the playing queue
            if (temporaryQueue.Count > 0)
            {
                IEnumerator animationCoroutine = temporaryQueue.Dequeue();
                playingQueue.Enqueue(animationCoroutine);

                // Start the animation immediately
                StartCoroutine(playingQueue.Dequeue());
            }

            // Wait for the cooldown duration before transferring the next animation
            yield return new WaitForSeconds(enqueueCooldown);
        }
    }

    public IEnumerator PlayAnimationAndWait(Animator localAnimator, GameObject animatedObject)
    {
        string animationName = localAnimator.runtimeAnimatorController.animationClips[0].name;

        // Trigger the animation
        animatedObject.SetActive(true);
        localAnimator.Play(animationName);

        // Wait until the animation has finished
        AnimatorStateInfo stateInfo = localAnimator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = localAnimator.GetCurrentAnimatorStateInfo(0);
        }

        // Destroy the animated object after the animation finishes
        Destroy(animatedObject);
        Debug.Log($"Animation '{animationName}' finished!");
    }

    public static async Task TriggerPlayerAction(Card card)
    {
        CardWrapper PlayerCardWrapper = GameObject.Find("PlayerAction").GetComponent<CardWrapper>();
        PlayerCardWrapper.SetCard(card);
        PileController.SetCardDisplay(PlayerCardWrapper.gameObject);

        Vector3 startPosition = PlayerCardWrapper.gameObject.transform.position;

        await PlayerCardWrapper.gameObject.transform.DOMove(new Vector3(startPosition.x, 0, 0), 1f).AsyncWaitForCompletion();
        await Task.Delay(1000);

        PlayerCardWrapper.gameObject.transform.position = startPosition;
    }

    public static async Task TriggerEnemyAction(Card card)
    {
        CardWrapper EnemyCardWrapper = GameObject.Find("EnemyAction").GetComponent<CardWrapper>();
        EnemyCardWrapper.SetCard(card);
        PileController.SetCardDisplay(EnemyCardWrapper.gameObject);

        Vector3 startPosition = EnemyCardWrapper.gameObject.transform.position;

        await EnemyCardWrapper.gameObject.transform.DOMove(new Vector3(startPosition.x, 0, 0), 1f).AsyncWaitForCompletion();
        await Task.Delay(1000);

        EnemyCardWrapper.gameObject.transform.position = startPosition;
    }

    public static async Task TurnStartAnimation()
    {
        GameObject turnStart = GameObject.Find("TurnStart");
        UnityEngine.UI.Image turnStartImage = turnStart.GetComponent<UnityEngine.UI.Image>();
        // Fade in
        await turnStartImage.DOFade(1f, 1f).AsyncWaitForCompletion();

        // Wait 1 second
        await Task.Delay(1000);

        // Fade out
        await turnStartImage.DOFade(0f, 1f).AsyncWaitForCompletion();
    }
}