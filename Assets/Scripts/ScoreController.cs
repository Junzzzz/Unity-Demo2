using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private Text _scoreNumText;
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        _scoreNumText = GetComponent<Text>();
        _score = 0;
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