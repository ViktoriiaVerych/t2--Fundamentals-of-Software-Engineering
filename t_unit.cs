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
    }
    
}