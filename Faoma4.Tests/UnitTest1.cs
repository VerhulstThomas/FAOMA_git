using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Faoma4.DAL;
//using Faoma4.BL;
using Faoma4;
using System.Data.Entity;



namespace Faoma4.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
            // Arrange
            var accountService = new AccountService();

            // Act                                 // account 90016
            bool result = accountService.DoLogin("niko@hotmail.com", "123Admin!");

            // Assert
            Assert.IsTrue(result);

        }
        public class AccountService
        {
            public bool DoLogin(string username, string password)
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    return false;

                return true;
            }
        }


        [TestMethod]
        public void TestMethod2()
        {
            // Arrange
            var accountService = new AccountService();
            // account 90016
            accountService.DoLogin("niko@hotmail.com", "123Admin!");

            // act
            //long test = tmpServerAccountsId.tmpServerAccountId;

            // Assert
            //Assert.IsNotNull(test);
        }



        [TestMethod]
        public void TestMethod3()
        {
            // Arrange
            var accountService = new AccountService();
            // account 90016
            accountService.DoLogin("niko@hotmail.com", "123Admin!");

            // act
            //string test = tempId.tmpId;

            // Assert
            //Assert.IsNotNull(test);
        }

        [TestMethod]
        public static void TestConnectionByCatchingAccounts()
        {
            //arrange
            FaomaModel db = new FaomaModel();
            //act
            //var accountList = db.serverAccount.ToListAsync();
            //assert
            //double lengt = accountList.t
            //Assert.AreNotEqual(0, accountList.)
            //Assert.IsNotNull(accountList);
            

        }

    }
}

        
    

