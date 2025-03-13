using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Utils;

namespace Player {
    public class PauseScreen : MonoBehaviour {
        [SerializeField] private Slider volumeSlider;

        private void Start() {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        }

        private void OnEnable() {
            Time.timeScale = 0f;
        }

        private void OnDisable() {
            Time.timeScale = 1f;
        }

        public void OnVolumeChanged(float value) {
            AudioListener.volume = value;
            PlayerPrefs.SetFloat("Volume", value);
            PlayerPrefs.Save();
        }

        public void OnSave() {
            var json = JsonUtility.ToJson(SerializeManager.Instance.Save(), true);
            Debug.Log(json, this);
            SaveToFile(json);
        }

        public void OnMainMenu() {
            OnSave();
            SceneManager.LoadScene("Scenes/Titlescreen");
        }

        public static void SaveToFile(string json) {
            var path = Path.Combine(Application.persistentDataPath, "savegame.json");
            File.WriteAllText(path, json);
            Debug.Log($"Saved to: {path}");
        }
    }
}