using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance;
    private static int _score;

    private Text _scoreNumText;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _scoreNumText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _scoreNumText.text = _score.ToString();
    }

    public void AddScore(int num)
    {
        _score += num;
    }
}