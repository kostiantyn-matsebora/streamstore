namespace StreamStore.Testing
{
    public static class Monad
    {
        public static bool And(this bool left, bool right)
        {
            return left && right;
        }
    }
}
