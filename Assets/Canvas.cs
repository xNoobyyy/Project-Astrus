using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using WatchAda.Quests;

public class Canvas2 : MonoBehaviour {
    [SerializeField] private QuestLogic ql;
    [SerializeField] private TextDisplay.TextDisplay td;
    public GameObject p;

    private void OnEnable() {
        if (td.notificationButton.gameObject.activeSelf) {
            p.gameObject.SetActive(true);
        } else {
            foreach (var quest in ql.questGroups[ql.activeGroup].subQuests
                         .Where(quest => quest.currentProgress != quest.requiredProgress)) {
                quest.currentProgress = quest.conditions.Where(condition => condition.IsMet()).ToArray().Length;
                if (quest.currentProgress < quest.requiredProgress || quest.textbaustein == 1000) continue;

                td.Notification(quest.textbaustein);
                p.gameObject.SetActive(true);
                break;
            }

            ql.UpdateSideQuests();
            ql.UpdateMainQuest();
        }
    }

    private void OnDisable() {
        p.gameObject.SetActive(false);
    }
}