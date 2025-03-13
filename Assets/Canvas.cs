using UnityEngine;
using WatchAda.Quests;

public class Canvas2 : MonoBehaviour {
    [SerializeField] private QuestLogic ql;
    [SerializeField] private TextDisplay.TextDisplay td;
    public GameObject p;

    private void OnEnable() {
        if (td.notificationButton.gameObject.activeSelf) {
            p.gameObject.SetActive(true);
        }
    }

    private void OnDisable() {
        p.gameObject.SetActive(false);
    }
}