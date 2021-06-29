namespace Presents.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class PresentsTests
    {
        [Test]
        public void CreateShouldThrowExc()
        {
            var bag = new Bag();

            Present present = null;

            Assert.Throws<ArgumentNullException>(() => bag.Create(present));
            
        }

        [Test]
        public void CreateShouldThrowExcAgain()
        {
            var bag = new Bag();

            Present present = new Present("Dani",12);
            bag.Create(present);
            Assert.Throws<InvalidOperationException>(() => bag.Create(present));

        }


        [Test]
        public void CreateShouldWork()
        {
            var bag = new Bag();

            Present present = new Present("Dani", 12);
            Assert.AreEqual($"Successfully added present Dani.", bag.Create(present));

        }

        [Test]
        public void RemoveShouldWork()
        {
            var bag = new Bag();
            Present present = new Present("Dani", 12);
            bag.Create(present);
            Assert.AreEqual(true, bag.Remove(present));

        }

        [Test]
        public void RemoveShouldNotWork()
        {
            var bag = new Bag();
            Present present = new Present("Dani", 12);
            bag.Remove(present);
            Assert.AreEqual(false, bag.Remove(present));

        }

        [Test]
        public void GetPresentWithLeastMagicShouldWork()
        {
            var bag = new Bag();
            Present present = new Present("Dani", 12);
            var expected = new Present("Gosho", 11);
            bag.Create(present);
            bag.Create(expected);
            
            Assert.AreEqual(expected, bag.GetPresentWithLeastMagic());
        }

        [Test]
        public void GetPresentWithLeastMagicShouldNotWork()
        {
            var bag = new Bag();
            Present present = new Present("Dani", 12);
            var expected = new Present("Gosho", 11);

            Assert.Throws<InvalidOperationException>(() => bag.GetPresentWithLeastMagic());
        }

        [Test]
        public void GetPresentShouldWork()
        {
            var bag = new Bag();
            Present present = new Present("Dani", 12);
            bag.Create(present);


            Assert.AreEqual(present, bag.GetPresent(present.Name));
        }

        [Test]
        public void GetPresentShouldAlosWork()
        {
            var bag = new Bag();
            Present present = new Present("Dani", 12);
            bag.Create(present);

            Assert.AreEqual(null,bag.GetPresent("gosho"));
        }

        [Test]
        public void GetPresents()
        {
            
            var bag = new Bag();
            Present present = new Present("Dani", 12);
            bag.Create(present);
            Present present2 = new Present("Gosho", 14);
            bag.Create(present2);

            Assert.AreEqual(2, bag.GetPresents().Count);
        }
        [Test]
        public void GetPresentsShouldWork()
        {

            var bag = new Bag();

            Assert.AreEqual(0, bag.GetPresents().Count);
        }
    }
}
