using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IStatModifier
{
    void AddModifyStat(Dictionary<Stat, float> keyValuePairs, bool remove = false);
    IEnumerator ModifyDuration(Dictionary<Stat, float> keyValuePairs, bool percen, float duration);
}
