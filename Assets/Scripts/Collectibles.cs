using UnityEngine;

public class Collectibles : MonoBehaviour
{
    private static readonly int CollectTrigger = Animator.StringToHash("collect");

    public int score;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Collect()
    {
        GetComponent<Collider2D>().enabled = false;
        _animator.SetTrigger(CollectTrigger);
    }

    private void AddScore()
    {
        ScoreController.Instance.AddScore(score);
    }
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
}