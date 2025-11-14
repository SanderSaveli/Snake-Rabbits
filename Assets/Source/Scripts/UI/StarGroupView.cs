using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class StarGroupView : MonoBehaviour
    {
        [SerializeField] private List<StarView> _starViews = new List<StarView>();

        public void ShowStars(int count)
        {
            int i = 1;
            foreach (var view in _starViews)
            {
                view.SetEnable(i <= count);
                i++;
            }
        }
    }
}
