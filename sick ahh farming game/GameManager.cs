using sick_ahh_farming_game.Services;

namespace sick_ahh_farming_game;

public static class GameManager
{
    public static GameService GameService { get; } = new GameService();
}