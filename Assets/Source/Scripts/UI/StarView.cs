using CustomText;
using SanderSaveli.UDK.UI;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class StarView : MonoBehaviour
    {
        [SerializeField] private ImageColorByType _image;
        [Space]
        public Custom_ColorStyle _enableColor = Custom_ColorStyle.Yellow;
        public Custom_ColorStyle _disableColor = Custom_ColorStyle.Gray;

        public void SetEnable(bool isEnable)
        {
            _image.ChangeColor(isEnable ? _enableColor : _disableColor);
        }
    }
}
