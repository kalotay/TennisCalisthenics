using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TennisCalisthenics
{
	public class Game
	{
		private readonly Scores _scores;

		public Game(Action<IPlayer> winnerListener)
		{
			_scores = new Scores(winnerListener);
		}

		public void Score(IPlayer player)
		{
			_scores.AddPointFor(player);
		}

	}

	public class Scores
	{
		private readonly Action<IPlayer> _winnerListener;
		private readonly List<IPlayer> _scores = new List<IPlayer>();

		public Scores(Action<IPlayer> winnerListener)
		{
			_winnerListener = winnerListener;
		}

		public void AddPointFor(IPlayer player)
		{
			_scores.Add(player);
			if (_scores.Count(x => x == player) == 4)
			{
				_winnerListener(player);
			}

		}
	}

	public interface IPlayer
	{
	}

	public class Server : IPlayer
	{
		
	}

	public class Receiver : IPlayer
	{
		
	}
}

namespace TennisCalisthenics.Tests.Unit
{
	[TestFixture]
	public class GameTests
	{
		private IPlayer _winner;

		[SetUp]
		public void ResetWinner()
		{
			_winner = null;
		}

		private void OnWinner(IPlayer winningPlayer)
		{
			_winner = winningPlayer;
		}
		
		[Test]
		[TestCase(typeof(Server))]
		[TestCase(typeof(Receiver))]
		public void Game_is_won_with_at_least_four_points_scored(Type playerType)
		{
			var player = playerType == typeof(Server) ? (IPlayer)new Server() : new Receiver();
			var game = new Game(OnWinner);

			game.Score(player);
			game.Score(player);
			game.Score(player);
			game.Score(player);

			Assert.That(_winner, Is.EqualTo(player));
		}

		[Test]
		public void Two_points_extra_are_needed_to_win_the_game()
		{
			var game = new Game(OnWinner);
			var server = new Server();
			var receiver = new Receiver();

			game.Score(server);
			game.Score(server);
			game.Score(server);
			game.Score(server);
			game.Score(receiver);
			game.Score(receiver);

			Assert.That(_winner, Is.EqualTo(server));
		}

	}
}
