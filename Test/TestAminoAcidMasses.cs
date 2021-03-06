﻿using NUnit.Framework;
using ProteoformSuiteInternal;

namespace Test
{
    [TestFixture]

    public class TestAminoAcidMasses
    {

        [Test]
        public void TestAminoAcidMassesConstructor()
        {
            var MassesList1 = new AminoAcidMasses(true, true);
            Assert.AreEqual(147.0353996, MassesList1.AA_Masses['M']);
            Assert.AreEqual(160.030649, MassesList1.AA_Masses['C']);

            var MassesList2 = new AminoAcidMasses(false, false);
            Assert.AreEqual(131.040485, MassesList2.AA_Masses['M']);
            Assert.AreEqual(103.009185, MassesList2.AA_Masses['C']);
        }

        [Test]
        public void TestAminoAcidMassesDefaultLysine()
        {
            var MassesList1 = new AminoAcidMasses(true, true);
            Assert.AreEqual(136.109162, MassesList1.AA_Masses['K']);
        }
    }
}
