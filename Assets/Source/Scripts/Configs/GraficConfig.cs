using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    [CreateAssetMenu(fileName ="new Grafic Config", menuName = "Snake/Configs/Grafic Config")]
    public class GraficConfig : ScriptableObject
    {
        [Min(0)]
        [SerializeField] private float _distanceBetweenCells;
        [Header("Cell colors")]
        [SerializeField] private Color _cellEvenColor = Color.white; 
        [SerializeField] private Color _cellOddColor = Color.white; 

        public Color CellEvenColor => _cellEvenColor;
        public Color CellOddColor => _cellOddColor;
        public float DistanceBetweenCells => _distanceBetweenCells;
    }
}
