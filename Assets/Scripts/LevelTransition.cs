using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Switch to next level
            SceneController.instance.NextLevel();
              
        }
    }
}
