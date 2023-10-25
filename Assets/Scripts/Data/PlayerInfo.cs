using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerInfo : ScriptableObject
{
    public float health;
    public float max_health;
    public float nerve;
    public float max_nerve;
    private List<Stat> stat_block = new List<Stat>();
    //[System.NonSerialized]
    //public UnityEvent<int> 

    private void OnEnable()
    {
        
    }

    public float GetStat(string statName)
    {
        foreach (Stat stat in stat_block)
        {
            if(stat.IsStatName(statName))
            {
                return stat.total_value;
            }
        }
        return 0;
    }
}
