using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameManager s_instance;

    public string PlaySceneName;

    private GameObject _player;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else if (!ReferenceEquals(s_instance, this))
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(_player);

        if (_player.TryGetComponent(out PlayerEventHandler playerEventHandler))
        {
            playerEventHandler.OnPlayerDeathEvent.AddListener(OnPlayerDeath);
        }
    }
    private void OnPlayerDeath()
    {
        SceneManager.LoadScene(PlaySceneName);
    }

}
