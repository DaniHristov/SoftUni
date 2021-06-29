// Use this file for your unit tests.
// When you are ready to submit, REMOVE all using statements to Festival Manager (entities/controllers/etc)
// Test ONLY the Stage class. 
namespace FestivalManager.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
	public class StageTests
    {
		[Test]
	    public void SongConstructorShouldWork()
	    {
			var timespan = new TimeSpan(0, 3, 30);
			var song = new Song("Beli noshti", timespan);

			Assert.AreEqual("Beli noshti", song.Name);
			Assert.AreEqual(timespan, song.Duration);
		}
		[Test]
		public void SongToStringMethod()
		{
			var timespan = new TimeSpan(0, 3, 30);
			var song = new Song("Beli noshti", timespan);

			var expected = $"{song.Name} ({song.Duration:mm\\:ss})";

			Assert.AreEqual(expected, song.ToString());
			
		}
		[Test]
		public void PerformerConstructorShouldWork()
		{
			var performer = new Performer("Dani", "Hristov", 21);


			Assert.AreEqual("Dani Hristov" , performer.FullName);
			Assert.AreEqual(21, performer.Age);
			Assert.AreEqual(0, performer.SongList.Count);

		}
		
		[Test]
		public void PerformerToStringMethodShouldWork()
		{
			var performer = new Performer("Dani", "Hristov", 21);

			var expected = "Dani Hristov";

			Assert.AreEqual(expected, performer.ToString());


		}

		[Test]
		public void StageConstructorShouldWork()
		{
			var stage = new Stage();

			Assert.AreEqual(0, stage.Performers.Count);
		}

		[Test]
		public void StageAddPerformerMethodShouldWork()
		{
			var stage = new Stage();
			var performer = new Performer("Dani", "Hristov", 21);

			stage.AddPerformer(performer);

			Assert.AreEqual(1, stage.Performers.Count);
		}

		[Test]
		public void StageAddPerformerMethodShouldThrowException()
		{
			var stage = new Stage();
			var performer = new Performer("Dani", "Hristov", 17);


			Assert.Throws<ArgumentException>(() =>
			stage.AddPerformer(performer));
		}

		[Test]
		public void StageShouldNotAddSong()
		{
			var stage = new Stage();
			var performer = new Performer("Dani", "Hristov", 17);
			var timespan = new TimeSpan(0, 0, 30);
			var song = new Song("Cocaine",timespan);

			Assert.Throws<ArgumentException>(() => stage.AddSong(song));
		}

		[Test]
		public void StageShouldAddSong()
		{
			var stage = new Stage();
			var performer = new Performer("Dani", "Hristov", 18);
			var timespan = new TimeSpan(0, 3, 30);
			var song = new Song("Cocaine", timespan);
			var songs = new List<Song>();

			stage.AddPerformer(performer);
			stage.AddSong(song);
			stage.AddSongToPerformer(song.Name, performer.FullName);


			Assert.AreEqual(1, performer.SongList.Count);
		}

		[Test]
		public void StageShouldAddSongToPerformer()
		{
			var stage = new Stage();
			var performer = new Performer("Dani", "Hristov", 18);
			var timespan = new TimeSpan(0, 3, 30);
			var song = new Song("Cocaine", timespan);
			stage.AddPerformer(performer);
			stage.AddSong(song);
			stage.AddSongToPerformer(song.Name, performer.FullName);

			Assert.AreEqual(1, performer.SongList.Count);
			Assert.AreEqual($"Cocaine (03:30) will be performed by Dani Hristov", $"{song} will be performed by {performer.FullName}");
		}

		[Test]
		public void StagePlayShouldWork()
		{
			var stage = new Stage();
			var performer = new Performer("Dani", "Hristov", 18);
			var timespan = new TimeSpan(0, 3, 30);
			var song = new Song("Cocaine", timespan);
			stage.AddPerformer(performer);
			stage.AddSong(song);
			stage.AddSongToPerformer(song.Name, performer.FullName);
			stage.Play();

			Assert.AreEqual($"{1} performers played {1} songs"
				, $"{stage.Performers.Count} performers played {stage.Performers.Sum(x=>x.SongList.Count)} songs");
		}

		[Test]
		public void ShouldThrowArgumentNullExceptionWhenAddPerformer()
		{
			var stage = new Stage();
			Performer performer = null;
			var timespan = new TimeSpan(0, 3, 30);
			var song = new Song("Cocaine", timespan);
			Assert.Throws<ArgumentNullException>(() => stage.AddPerformer(performer));


		}

		public void ShouldThrowArgumentNullExceptionWhenAddSong()
		{
			var stage = new Stage();
			Performer performer = null;
			var timespan = new TimeSpan(0, 3, 30);
			var song = new Song("Cocaine", timespan);
			song = null;
			Assert.Throws<ArgumentNullException>(() => stage.AddSong(song));


		}






	}
}