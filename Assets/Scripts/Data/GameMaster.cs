using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster _instance { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    private int Roll(int threshold, int modifier)
    {
        int result = Random.Range(1, 100) + modifier;
        if (result < threshold - 25) { return 0; }
        else if (result < threshold) { return 1; }
        else if (result < threshold + 50) { return 2; }
        else { return 3; }
    }

    public void AttackRoll(CharacterInfo target, string name, int acc, float damage, AtkType atkType)
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
        
        switch (Roll((int)target.GetStatValue(target_stat), acc))
        {
            case 0:
                Debug.Log("Miss...");
                break;
            case 1:
                dmg = Mathf.RoundToInt(damage * 0.5f);
                Debug.Log($"Grazing Hit. {name} deals {dmg} damage to {target.name}.");
                target.TakeDamage(dmg);
                break;
            case 2:
                dmg = Mathf.RoundToInt(damage);
                Debug.Log($"Hit. {name} deals {dmg} damage to {target.name}.");
                target.TakeDamage(dmg);
                break;
            case 3:
                dmg = Mathf.RoundToInt(damage * 1.5f);
                Debug.Log($"Critical Hit! {name} deals {dmg} damage to {target.name}.");
                target.TakeDamage(dmg);
                break;
            default: break;
        }
    }
}
