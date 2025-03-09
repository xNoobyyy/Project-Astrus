using Utils;

namespace Logic.Events {
    public class PlayerAreaEnterEvent {
        public readonly AreaType AreaType;
        public readonly Area Area;

        public PlayerAreaEnterEvent(Area area) {
            AreaType = area.type;
            Area = area;
        }
    }
}