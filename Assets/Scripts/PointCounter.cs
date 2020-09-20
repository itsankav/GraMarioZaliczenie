using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{

    public Text pointText;
    private int points = 0;

    // Update is called once per frame
    void Update()
    {
        pointText.text = "score:" + points.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Box")
        {
            points += 1;
            Destroy(collision.gameObject);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
