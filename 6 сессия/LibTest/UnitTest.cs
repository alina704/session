using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VIN_LIB;
using REG_MARK_LIB;

namespace LibTest
{
    [TestClass]
    public class UnitTests
    {
        private VIN vin = new VIN();
        private RegMark reg = new RegMark();

        [TestMethod]
        public void CheckVINTest()
        {
            Assert.IsTrue(vin.CheckVIN("1HGCG2254WA015540"));
            Assert.IsTrue(vin.CheckVIN("1G4GD5EDXBF330171"));
            Assert.IsFalse(vin.CheckVIN("WD2YD2418253568"));
            Assert.IsFalse(vin.CheckVIN("JH4KA2532HC022031123"));
            Assert.IsFalse(vin.CheckVIN("2I8GP44LX5R216765"));
        }

        [TestMethod]
        public void GetTransportYearTest()
        {
            Console.WriteLine("1HGCG2254WA015540 -> " + vin.GetTransportYear("1HGCG2254WA015540"));
            Console.WriteLine("1G4GD5EDXBF330171 -> " + vin.GetTransportYear("1G4GD5EDXBF330171"));
            Console.WriteLine("WD2YD241825356884 -> " + vin.GetTransportYear("WD2YD241825356884"));
            Console.WriteLine("JH4KA2532HC022031 -> " + vin.GetTransportYear("JH4KA2532HC022031"));
            Console.WriteLine("2D8GP44LX5R216765 -> " + vin.GetTransportYear("2D8GP44LX5R216765"));
        }

        [TestMethod]
        public void GetVINCountryTest()
        {
            Console.WriteLine("1HGCG2254WA015540 -> " + vin.GetVINCountry("1HGCG2254WA015540"));
            Console.WriteLine("1G4GD5EDXBF330171 -> " + vin.GetVINCountry("1G4GD5EDXBF330171"));
            Console.WriteLine("WD2YD241825356884 -> " + vin.GetVINCountry("WD2YD241825356884"));
            Console.WriteLine("JH4KA2532HC022031 -> " + vin.GetVINCountry("JH4KA2532HC022031"));
            Console.WriteLine("2D8GP44LX5R216765 -> " + vin.GetVINCountry("2D8GP44LX5R216765"));
        }

        [TestMethod]
        public void CheckMarkTest()
        {
            Assert.IsTrue(reg.CheckMark("A999AA999"));
            Assert.IsTrue(reg.CheckMark("X123YO124"));
            Assert.IsTrue(reg.CheckMark("C899OT50"));
            Assert.IsFalse(reg.CheckMark("999AAA999"));
            Assert.IsFalse(reg.CheckMark("I999QU999"));
        }

        [TestMethod]
        public void GetNextMarkAfterTest()
        {
            Console.WriteLine("A999AA999 -> " + reg.GetNextMarkAfter("A999AA999"));
            Console.WriteLine("X123YO124 -> " + reg.GetNextMarkAfter("X123YO124"));
            Console.WriteLine("A123CB120 -> " + reg.GetNextMarkAfter("A123CB120"));
            Console.WriteLine("C899OT50 -> " + reg.GetNextMarkAfter("C899OT50"));
            Console.WriteLine("C899XX50 -> " + reg.GetNextMarkAfter("C899XX50"));
        }

        [TestMethod]
        public void GetCombinationsCountInRangeTest()
        {
            Console.WriteLine("A000AA24::A005AA24 -> " + reg.GetCombinationsCountInRange("A000AA24", "A005AA24"));
            Console.WriteLine("A000AA24::A064AA24 -> " + reg.GetCombinationsCountInRange("A000AA24", "A064AA24"));
            Console.WriteLine("A128AA24::A000AA24 -> " + reg.GetCombinationsCountInRange("A128AA24", "A000AA24"));
            Console.WriteLine("A000AA24::X000AA24 -> " + reg.GetCombinationsCountInRange("A000AA24", "X000AA24"));
            Console.WriteLine("A000AA24::X000XX24 -> " + reg.GetCombinationsCountInRange("A000AA24", "X999XX24"));
        }

        [TestMethod]
        public void GetNextMarkAfterInRangeTest()
        {
            string prevMark = "A000AA24";
            Console.WriteLine("Range: A000AA24..A100AA24");
            for (int i = 0; i < 101; ++i)
            {
                prevMark = reg.GetNextMarkAfterInRange(prevMark, "A000AA24", "A100AA24");
                Console.WriteLine(prevMark);
            }
        }
    }
}
