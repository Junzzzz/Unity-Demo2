using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public static DialogController Instance { get; private set; }

    private Animator _animator;
    private Text _text;
    private static readonly int CloseTrigger = Animator.StringToHash("close");

    private void Start()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
        _text = GetComponentInChildren<Text>();
        
        SetInactive();
    }

    public void Open(string text)
    {
        _text.text = text;
        gameObject.SetActive(true);
    }

    public void Close(string text)
    {
        if (_text.text.Equals(text))
        {
            Close();
        }
    }

    public void Close()
    {
        _animator.SetTrigger(CloseTrigger);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }
}