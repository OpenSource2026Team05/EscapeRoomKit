using UnityEngine;
using UnityEngine.SceneManagement;

namespace EscapeRoomKit
{
    public class Event_LoadScene : MonoBehaviour
    {
        [Header("Scene")]
        public string sceneName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 3)
            {
                GoToScene(sceneName);
            }
        }

        public void GoToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
