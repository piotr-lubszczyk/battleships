using Battleships.Models;

var game = new Game();

while (true)
{
    if (game.IsFinished)
    {
        var shouldContinue = game.AskForContinue();

        if (shouldContinue)
            game = new Game();
        else
            return;
    }

    game.Play();
}
