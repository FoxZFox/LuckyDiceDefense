using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int Level { get; protected set; }
    public int Cost { get; protected set; }
    [SerializeField] private List<AbilityData> abilityDatas;
    private void Attack()
    {

    }
    private void Start()
    {
        Debug.Log("Base class");
    }
    private void Update()
    {

    }
    private void UseAbility()
    {

    }
}
