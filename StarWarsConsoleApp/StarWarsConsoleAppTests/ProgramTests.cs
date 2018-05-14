using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarWarsConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsConsoleApp.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void MainTest()
        {

        }

        [TestMethod()]
        public void CalculateStops_Test_Pass_Weeks()
        {
            // Test the result of consumables with weeks
            // Test values from Y-wing

            string consumables = "1 week";
            string starhipMglt = "80";
            int dist = 1000000;

            // Process function
            string result = Program.CalculateStops(consumables, starhipMglt, dist);

            Assert.IsTrue(result == "74");
        }

        [TestMethod()]
        public void CalculateStops_Test_Pass_Years()
        {
            // Test the result of consumables with years
            // Test values from Death Star

            string consumables = "3 years";
            string mglt = "10";
            int dist = 1000000;

            // Process function
            string result = Program.CalculateStops(consumables, mglt, dist);

            Assert.IsTrue(result == "3");
        }

        [TestMethod()]
        public void CalculateStops_Test_Pass_Days()
        {
            // Test the result of consumables with days
            // Test values from TIE Advanced x1

            string consumables = "5 days";
            string mglt = "105";
            int dist = 1000000;

            // Process function
            string result = Program.CalculateStops(consumables, mglt, dist);

            Assert.IsTrue(result == "79");
        }


        [TestMethod()]
        public void CalculateStops_Test_Pass_Months()
        {
            // Test the result of consumables with months
            // Test values from Millenium falcon

            string consumables = "2 months";
            string mglt = "75";
            int dist = 1000000;

            // Process function
            string result = Program.CalculateStops(consumables, mglt, dist);

            Assert.IsTrue(result == "9");
        }

        [TestMethod()]
        public void CalculateStops_Test_Missing_Values()
        {
            // Test the result of a record with no MGLT values
            // Test values from Naboo fighter

            string consumables = "7 days";
            string mglt = "unknown";
            int dist = 1000000;

            // Process function
            string result = Program.CalculateStops(consumables, mglt, dist);

            Assert.IsTrue(result == "Unknown");
        }
    }
}
