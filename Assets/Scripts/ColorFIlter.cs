using UnityEngine;

namespace KanjiYomi
{

    [System.Serializable]
    public struct ColorFilter
    {
        public Color color;
        [Range(-100f, 100f)]
        public float density;
    }
}
