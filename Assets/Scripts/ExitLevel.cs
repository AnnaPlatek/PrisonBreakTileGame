using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    BoxCollider2D doorCollider;
    [SerializeField] float LoadLevelDelay = 3f;

    private void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (doorCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            //TODO show button to click
            if (Input.GetKeyDown("return"))
            {
                StartCoroutine(LoadNextLevel());
                //TODO: Particles + music + level complete
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(LoadLevelDelay);
        var currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel + 1);
    }
}
