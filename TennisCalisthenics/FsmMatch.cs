using System;
using NUnit.Framework;

namespace TennisCalisthenics
{
	public interface IGame
	{
		void ReceiverScore();

		void ServerScore();
	}

	public class InitialGame: IGame
	{
		private readonly IWinnerListener _listener;

		public InitialGame(IWinnerListener listener)
		{
			_listener = listener;
		}

		public void ReceiverScore()
		{
		}

		public void ServerScore()
		{
		}
	}

	public interface IPoint
	{
		IPoint Increment();
	}

	public class Zero: IPoint
	{
		public IPoint Increment()
		{
			return new Fifteen();
		}
	}

	public class Fifteen : IPoint
	{
		public IPoint Increment()
		{
			return new Thirty();
		}
	}

	public class Thirty : IPoint
	{
		public IPoint Increment()
		{
			return new Fourty();
		}
	}

	public class Fourty : IPoint
	{
		public IPoint Increment()
		{
			throw new Exception("not sure what to do");
		}
	}

	public interface IWinnerListener
	{
		void ServerWon();
		void ReceiverWon();
	}

	[TestFixture]
	public class MatchTest: IWinnerListener
	{
		private enum Player
		{
			Server,
			Receiver
		}

		private Player? _winner;
		private IGame _game;

		[SetUp]
		public void ResetWinner()
		{
			_winner = null;
			_game = new InitialGame(this);
		}

		[Test]
		public void Server_scoring_four_times_means_server_wins()
		{
			_game.ServerScore();
			_game.ServerScore();
			_game.ServerScore();
			_game.ServerScore();

			Assert.That(_winner, Is.EqualTo(Player.Server));
		}

		[Test]
		public void Receiver_scoring_four_times_means_server_wins()
		{
			_game.ReceiverScore();
			_game.ReceiverScore();
			_game.ReceiverScore();
			_game.ReceiverScore();

			Assert.That(_winner, Is.EqualTo(Player.Receiver));
		}

		[Test]
		public void Incomplete_games_have_no_winner()
		{
			_game.ReceiverScore();
			_game.ServerScore();
			_game.ServerScore();
			_game.ReceiverScore();

			Assert.That(_winner, Is.Null);
		}

		public void ServerWon()
		{
			_winner = Player.Server;
		}

		public void ReceiverWon()
		{
			_winner = Player.Receiver;
		}
	}
}