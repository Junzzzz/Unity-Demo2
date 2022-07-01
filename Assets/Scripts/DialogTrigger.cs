using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public string text;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DialogController.Instance.Open(text);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DialogController.Instance.Close(text);
        }
    }
}