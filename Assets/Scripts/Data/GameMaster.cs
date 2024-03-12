using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Transform text_area;
    private TMP_Text log;

    public static GameMaster _instance { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            log = text_area.GetComponent<TMP_Text>();
            log.text = "";

            _instance = this;
        }
    }

    private int Roll(int threshold, int modifier)
    {
        int result = UnityEngine.Random.Range(1, 100) + modifier;
        log.text = log.text + "\n" + $"Rolled {result} vs. {threshold}";
        Debug.Log($"Rolled {result} vs. {threshold}");
        if (result < threshold - 25) { return 0; }
        else if (result < threshold) { return 1; }
        else if (result < threshold + 50) { return 2; }
        else { return 3; }
    }

    private void Damage(CharacterInfo target, string name, int dmg, string hitResult="")
    {
        log.text = log.text + "\n" + $"{hitResult}{name} deals {dmg} damage to {target.char_name}.";
        Debug.Log($"{hitResult}{name} deals {dmg} damage to {target.char_name}.");
        target.TakeDamage(dmg);
    }

    public void AttackRoll(CharacterInfo target, string name, int acc, float damage, AtkType atkType, List<int> hitEffects=null, List<int> critEffects =null)
    {
        int dmg;
        string target_stat = "";
        if (atkType == AtkType.Carapace)
        {
            target_stat = "carapace";
        }
        else if (atkType == AtkType.Mucus)
        {
            target_stat = "mucus";
        }
        else if (atkType == AtkType.Ectoplasm)
        {
            target_stat = "ectoplasm";
        }
        
        switch (Roll(target.GetStatValueInt(target_stat), acc))
        {
            case 0:
                log.text = log.text + "\n" + $"Miss...";
                Debug.Log($"Miss...");
                break;
            case 1:
                dmg = Mathf.RoundToInt(damage * 0.5f);
                Damage(target, name, dmg, "Grazing Hit -- ");
                break;
            case 2:
                dmg = Mathf.RoundToInt(damage);
                Damage(target, name, dmg, "Hit -- ");
                if (hitEffects == null) break;
                foreach (int e in hitEffects)
                {
                    DataManager._instance.effect_list[e].ApplyModifiers(target);
                    Debug.Log("Applied effect " + DataManager._instance.effect_list[e].name);
                }
                break;
            case 3:
                dmg = Mathf.RoundToInt(damage * 1.5f);
                Damage(target, name, dmg, "Critical Hit! -- ");
                if (hitEffects != null)
                {
                    foreach (int e in hitEffects)
                    {
                        DataManager._instance.effect_list[e].ApplyModifiers(target);
                        Debug.Log("Applied effect " + DataManager._instance.effect_list[e].name);
                    }
                }
                if (critEffects != null)
                {
                    foreach (int e in critEffects)
                    {
                        DataManager._instance.effect_list[e].ApplyModifiers(target);
                        Debug.Log("Applied effect " + DataManager._instance.effect_list[e].name);
                    }
                }
                break;
            default: break;
        }
    }

    public void BasicResistRoll(CharacterInfo target, int intensity, Effect e)
    {

    }
}
