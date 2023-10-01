using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace test_database
{
    [TestClass]
    public class Testing
    {
        [TestMethod]
        public void GetData_Zero()
        {
            var mock = new Mock<IRestClient>();
            mock.Setup(client => client.Get("your_url")).Returns(new ApiResponse { Data = "test_data" });

            var result = YourClass.GetData(mock.Object, 0);
            Assert.AreEqual("test_data", result);
        }

        [TestMethod]
        public void AdjustTimezone_Plus()
        {
            var dt = new DateTime(2023, 9, 25, 17, 26, 28);
            var result = YourClass.AdjustTimezone(dt, "+03:00");
            Assert.AreEqual(new DateTime(2023, 9, 25, 14, 26, 28), result);
        }


        [TestMethod]
        public void ParseLastSeenDate_ValidString()
        {
            var input = "2023-09-25T17:26:28.123456+03:00";
            var (result, timeInfo) = YourClass.ParseLastSeenDate(input);
            Assert.AreEqual(new DateTime(2023, 9, 25, 17, 26, 28, 123), result);
            Assert.AreEqual("+03:00", timeInfo);
        }

        [TestMethod]
        public void FormatLastSeen_ValidUser()
        {
            var user = new Dictionary<string, string>
            {
                { "nickname", "test_user" },
                { "lastSeenDate", "2023-09-25T17:26:28.123456+03:00" }
            };

            var (resultName, resultDiff) = YourClass.FormatLastSeen(user);
            Assert.AreEqual("test_user", resultName);
            Assert.IsTrue(resultDiff is TimeSpan);
        }


        [TestMethod]
        public void FormatTimeDiff_30sec()
        {
            var diff = TimeSpan.FromSeconds(29);
            var lan = "en-US";
            var result = YourClass.FormatTimeDiff(diff, lan);
            Assert.AreEqual("was seen just now", result);
        }


        [TestMethod]
        public void ReturnEn()
        {
            var mockInput = new Mock<IUserInput>();
            mockInput.Setup(input => input.GetInput()).Returns("1");

            var result = YourClass.ChooseLanguage(mockInput.Object);
            Assert.AreEqual("en-US", result);
        }

         [TestMethod]
        public void GetAllDataWhenCalled()
        {
            var mock = new Mock<IRestClient>();
            mock.Setup(client => client.Get("your_url")).Returns(new ApiResponse { Data = "data1" });

            var result = YourClass.GetAllData(mock.Object);
            CollectionAssert.AreEqual(new List<string> { "data1" }, result);
        }

        [TestMethod]
        public void PrintUserStatusIfValidData()
        {
            var mockConsole = new Mock<IConsole>();
            var userName = "test_user";
            var status = "was seen just now";

            YourClass.PrintUserStatus(userName, status, mockConsole.Object);

            mockConsole.Verify(console => console.WriteLine($"{userName} {status}"), Times.Once());
        }

    }
    
}