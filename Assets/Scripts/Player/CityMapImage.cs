using UnityEngine;
using WatchAda.Quests;

namespace Player {
    public class CityMapImage : MonoBehaviour {
        public void CloseCityMap() {
            transform.parent.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}