using SanderSaveli.UDK.UI;
using System.Collections.Generic;

namespace SanderSaveli.Snake
{
    public class LevelFiller : SlotFiller<LevelSlot, LevelData>
    {
        public List<LevelSlot> Slots => _slots;
    }
}
