namespace Main.Algorithm
{
    public abstract class GridSearcher
    {
        public Grid Grid { get; private set; }

        public abstract Cell FindNext();

        protected GridSearcher(Grid grid)
        {
            Grid = grid;
        }
    }
}
