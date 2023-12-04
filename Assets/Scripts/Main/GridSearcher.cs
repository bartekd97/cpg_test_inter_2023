namespace Main
{
    public abstract class GridSearcher
    {
        public Grid Grid { get; private set; }

        public abstract Cell TryGetNext();

        protected GridSearcher(Grid grid)
        {
            Grid = grid;
        }
    }
}
