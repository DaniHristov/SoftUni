using NUnit.Framework;
using System;

namespace BankSafe.Tests
{
    public class BankVaultTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ItemConstructorShouldSetValues()
        {
            var item = new Item("das", "asd");
            var expected = item;

            Assert.AreEqual("das",item.Owner);
            Assert.AreEqual("asd", item.ItemId);
        }

        [Test]
        public void BankVaultShoudAddValue()
        {
            var bankVault = new BankVault();
            var item = new Item("Dani", "12");
            bankVault.AddItem("A1", item);
            var vv bankVault.VaultCells.TryGetValue("A1", item,out var foni);
        }
    }
}