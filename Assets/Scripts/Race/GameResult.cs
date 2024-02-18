namespace Race
{
    public static class GameResult
    {
        public static int GetLapRaceResult(int position, float time)
        {
            //todo: make up some interesting formula
            return position == 1 ? 100 : 100 / position;
        }

        public static int GetFreeRideResult(int score)
        {
            //todo: make up some interesting formula
            return score * 2;
        }
    }
}
    