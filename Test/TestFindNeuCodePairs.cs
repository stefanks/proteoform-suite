﻿using NUnit.Framework;
using ProteoformSuiteInternal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestFixture]
    public class TestFindNeuCodePairs
    {
        [OneTimeSetUp]
        public void setup()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [Test]
        public void testFindRawNeuCodePairsMethod()
        {
            //reading in test excel file, process raw components before testing neucode pairs.
            Lollipop.raw_experimental_components.Clear();
            Func<string, IEnumerable<Component>> componentReader = c => new ExcelReader().read_components_from_xlsx(c);
            Lollipop.deconResultsFileNames = new System.ComponentModel.BindingList<string>();
            Lollipop.deconResultsFileNames.Add("UnitTestFiles\\noisy.xlsx");
            Lollipop.neucode_labeled = true;
            Lollipop.process_raw_components(componentReader);
            List<NeuCodePair> neucode_pair = new List<NeuCodePair>();

            //testing intensity ratio
            neucode_pair = Lollipop.raw_neucode_pairs.Where(i => i.id_heavy == 220 && i.id_light == 219).ToList();
            Assert.AreEqual(2.0839922540849636, neucode_pair[0].intensity_ratio);

            //testing K-count
            Assert.AreEqual(18, neucode_pair[0].lysine_count);

            //testing that only overlapping charge states go into intensity ratio
            neucode_pair = Lollipop.raw_neucode_pairs.Where(i => i.id_heavy == 216 && i.id_light == 215).ToList();
            Assert.AreEqual(2.0056389907830128d, neucode_pair[0].intensity_ratio);

            //testing that if Neucode-light is "heavier", K value still correctly calculated
            neucode_pair = Lollipop.raw_neucode_pairs.Where(i => i.id_heavy == 217 && i.id_light == 218).ToList();
            Assert.AreEqual(15, neucode_pair[0].lysine_count);

            //test that pair w/ out of bounds I-ratio is marked unaccepted
            neucode_pair = Lollipop.raw_neucode_pairs.Where(i => i.id_heavy == 222 && i.id_light == 221).ToList();
            Assert.AreEqual(false, neucode_pair[0].accepted);

            //test that pair w/ out of bounds K-count is marked unaccepted
            neucode_pair = Lollipop.raw_neucode_pairs.Where(i => i.id_heavy == 224 && i.id_light == 223).ToList();
            Assert.AreEqual(false, neucode_pair[0].accepted);
        }

    }
}
