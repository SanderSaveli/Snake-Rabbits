using SanderSaveli.UDK.UI;
using System.Collections.Generic;

namespace SanderSaveli.Snake
{
    public class LevelFiller : SlotFiller<LevelSlot, LevelSaveData>
    {
        public List<LevelSlot> Slots => _slots;
    }
}
