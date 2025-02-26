using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public void TimeOver()
    {
        SceneManager.LoadScene("GameScene");
    }
}
