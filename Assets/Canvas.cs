using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class Canvas2 : MonoBehaviour {
    [SerializeField] private QuestLogic ql;
    [SerializeField] private TextDisplay.TextDisplay td;
    public GameObject p;
    
    private void OnEnable() {
        if (td.notificationButton.gameObject.activeSelf) {
            p.gameObject.SetActive(true);
        } else {
            foreach (var quest in ql.questGroups[ql.activeGroup].subQuests) {
                if (quest.currentProgress != quest.requiredProgress) {
                    quest.currentProgress = 0;
                    foreach (var condition in quest.conditions) {
                        if (condition.IsMet()) {
                            quest.currentProgress++;
                        }
                    }

                    if (quest.currentProgress >= quest.requiredProgress && quest.textbaustein != 1000) {
                        td.Notification(quest.textbaustein);
                        p.gameObject.SetActive(true);
                        break;
                    }
                } 
            }
            ql.UpdateSideQuests();
            ql.UpdateMainQuest();
        }
    }

    private void OnDisable() {
        p.gameObject.SetActive(false);
    }
}
