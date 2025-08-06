using System.Collections.Generic;
using UnityEngine;

namespace PrSuperSoldier
{
    [CreateAssetMenu(fileName = "StageTable", menuName = "SuperSoldier/Stage Table")]
    public class StageTableSO : ScriptableObject
    {
        public List<StageInfoSO> values;
    }
}
