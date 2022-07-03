using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public string text;
    public bool status { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            status = true;
            DialogController.Instance.Open(text);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            status = false;
            DialogController.Instance.Close(text);
        }
    }
}