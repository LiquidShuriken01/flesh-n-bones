using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : ScriptableObject
{
    public float health;
    public float nerve;
    private List<Stat> stat_block = new List<Stat>();

    private void Awake()
    {
        
    }

    void Start()
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
