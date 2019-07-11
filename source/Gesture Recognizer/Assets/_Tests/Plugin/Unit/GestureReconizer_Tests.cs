using System.Collections;
using System.Collections.Generic;
using GestureRecognizer;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Plugin.Unit
{
    public class GestureReconizer_Tests : Test
    {
        const string SquareName = "Square";
        const string LineName = "Line";
        const string ArrowName = "Arrow";
        private GesturesDataset gestureDataset;

        private Vector3[] SingleStrokeSquare
        {
            get { return new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 0) }; }
        }

        private List<Vector3[]> MultiStrokeSquare
        {
            get
            {
                List<Vector3[]> strokeList = new List<Vector3[]>();
                strokeList.Add(new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 0, 0) });
                strokeList.Add(new Vector3[] { new Vector3(1, 0, 0), new Vector3(1, 1, 0) });
                strokeList.Add(new Vector3[] { new Vector3(1, 1, 0), new Vector3(0, 1, 0) });
                strokeList.Add(new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 0, 0) });
                return strokeList;
            }
        }

        private Vector3[] SingleStrokeLine
        {
            get { return new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 3, 0) }; }
        }

        private List<Vector3[]> MultiStrokeLine
        {
            get
            {
                List<Vector3[]> strokeList = new List<Vector3[]>();
                strokeList.Add(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) });
                strokeList.Add(new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 2, 0) });
                strokeList.Add(new Vector3[] { new Vector3(0, 2, 0), new Vector3(0, 3, 0) });
                return strokeList;
            }
        }

        private Vector3[] SingleStrokeArrow
        {
            get { return new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 2, 0), new Vector3(-1, 1, 0), new Vector3(0, 2, 0), new Vector3(1, 1, 0) }; }
        }

        private List<Vector3[]> MultiStrokeArrow
        {
            get
            {
                List<Vector3[]> strokeList = new List<Vector3[]>();
                strokeList.Add(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 2, 0) });
                strokeList.Add(new Vector3[] { new Vector3(-1, 1, 0), new Vector3(0, 2, 0), new Vector3(1, 1, 0) });
                return strokeList;
            }
        }

        [SetUp]
        public void Setup()
        {
            gestureDataset = ScriptableObject.CreateInstance<GesturesDataset>();
        }

        [TearDown]
        protected override void Reset()
        {
            DestroyObject(gestureDataset);
        }

        [Test]
        public void SaveSingleStrokeGesture()
        {
            List<Vector3[]> listOfVector3Array = new List<Vector3[]>();

            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(SingleStrokeSquare, SquareName);

            Assert.That(gestureDataset.Gestures.Count == 1);
            Assert.That(gestureDataset.GetGesture(SquareName) != null);
            listOfVector3Array.Add(SingleStrokeSquare);
            Assert.That(IsListEquals(gestureDataset.GetGesture(SquareName).GetPointsVector3Lists(), listOfVector3Array));
        }

        [Test]
        public void SaveMultipleStrokeGesture()
        {
            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(MultiStrokeSquare, SquareName);

            Assert.That(gestureDataset.Gestures.Count == 1);
            Assert.That(gestureDataset.GetGesture(SquareName) != null);
            Assert.That(IsListEquals(gestureDataset.GetGesture(SquareName).GetPointsVector3Lists(), MultiStrokeSquare));
        }

        [Test]
        public void GetSingleGesture()
        {
            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(MultiStrokeSquare, SquareName);
            gestureDataset.AddGesture(MultiStrokeLine, LineName);
            gestureDataset.AddGesture(MultiStrokeArrow, ArrowName);

            Assert.That(gestureDataset.GetGesture(SquareName).name == SquareName);
            Assert.That(IsListEquals(gestureDataset.GetGesture(SquareName).GetPointsVector3Lists(), MultiStrokeSquare));
        }

        [Test]
        public void DeleteGesture()
        {
            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(SingleStrokeSquare, SquareName);
            gestureDataset.AddGesture(SingleStrokeLine, LineName);

            Assert.That(gestureDataset.Gestures.Count == 2);

            gestureDataset.DeleteGesture(SquareName);

            Assert.That(gestureDataset.Gestures.Count == 1);
            Assert.That(gestureDataset.GetGesture(SquareName) == null);
            Assert.That(gestureDataset.GetGesture(LineName) != null);

            gestureDataset.DeleteGesture(LineName);

            Assert.That(gestureDataset.Gestures.Count == 0);
            Assert.That(gestureDataset.GetGesture(SquareName) == null);
            Assert.That(gestureDataset.GetGesture(LineName) == null);

            gestureDataset.DeleteGesture(SquareName);
            gestureDataset.DeleteGesture(LineName);

            Assert.That(gestureDataset.Gestures.Count == 0);
        }

        [Test]
        public void ReplaceGesture()
        {
            List<Vector3[]> singleStrokeSquareList = new List<Vector3[]>();

            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(SingleStrokeSquare, SquareName);

            Assert.That(gestureDataset.Gestures.Count == 1);
            singleStrokeSquareList.Add(SingleStrokeSquare);
            Assert.That(IsListEquals(gestureDataset.GetGesture(SquareName).GetPointsVector3Lists(), singleStrokeSquareList));

            gestureDataset.AddGesture(MultiStrokeSquare, SquareName);

            Assert.That(gestureDataset.Gestures.Count == 1);
            Assert.That(!IsListEquals(gestureDataset.GetGesture(SquareName).GetPointsVector3Lists(), singleStrokeSquareList));
            Assert.That(IsListEquals(gestureDataset.GetGesture(SquareName).GetPointsVector3Lists(), MultiStrokeSquare));
        }

        [Test]
        public void DetectSingleStrokeGesture()
        {
            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(SingleStrokeSquare, SquareName);
            gestureDataset.AddGesture(SingleStrokeLine, LineName);
            gestureDataset.AddGesture(SingleStrokeArrow, ArrowName);

            Assert.That(gestureDataset.Recognize(SingleStrokeSquare) == SquareName);
            Assert.That(gestureDataset.Recognize(SingleStrokeLine) == LineName);
            Assert.That(gestureDataset.Recognize(SingleStrokeArrow) == ArrowName);
        }

        [Test]
        public void DetectMultipleStrokeGesture()
        {
            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(MultiStrokeSquare, SquareName);
            gestureDataset.AddGesture(MultiStrokeLine, LineName);
            gestureDataset.AddGesture(MultiStrokeArrow, ArrowName);

            Assert.That(gestureDataset.Recognize(MultiStrokeSquare) == SquareName);
            Assert.That(gestureDataset.Recognize(MultiStrokeLine) == LineName);
            Assert.That(gestureDataset.Recognize(MultiStrokeArrow) == ArrowName);
        }

        [Test]
        public void DetectSingleStrokeGestureAsAMultipleStroke()
        {
            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(SingleStrokeSquare, SquareName);
            gestureDataset.AddGesture(SingleStrokeLine, LineName);
            gestureDataset.AddGesture(SingleStrokeArrow, ArrowName);

            Assert.That(gestureDataset.Recognize(MultiStrokeSquare) == SquareName);
            Assert.That(gestureDataset.Recognize(MultiStrokeLine) == LineName);
            Assert.That(gestureDataset.Recognize(MultiStrokeArrow) == ArrowName);
        }

        [Test]
        public void DetectMultipleStrokeGestureAsASingleStroke()
        {
            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(MultiStrokeSquare, SquareName);
            gestureDataset.AddGesture(MultiStrokeLine, LineName);
            gestureDataset.AddGesture(MultiStrokeArrow, ArrowName);

            Assert.That(gestureDataset.Recognize(SingleStrokeSquare) == SquareName);
            Assert.That(gestureDataset.Recognize(SingleStrokeLine) == LineName);
            Assert.That(gestureDataset.Recognize(SingleStrokeArrow) == ArrowName);
        }

        [Test]
        [TestCase(0.05f)]
        [TestCase(0.10f)]
        [TestCase(0.15f)]
        public void DetectGestureWithNoise(float maxNoiseRange)
        {
            int timesToTestEachGesture = 5;

            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(MultiStrokeSquare, SquareName);
            gestureDataset.AddGesture(MultiStrokeLine, LineName);
            gestureDataset.AddGesture(MultiStrokeArrow, ArrowName);

            for (int testIndex = 0; testIndex < timesToTestEachGesture; testIndex++)
            {
                Vector3[] noiseSquare = AddNoiseToVectors(maxNoiseRange, SingleStrokeSquare);
                Assert.That(gestureDataset.Recognize(noiseSquare) == SquareName);
            }

            for (int testIndex = 0; testIndex < timesToTestEachGesture; testIndex++)
            {
                Vector3[] noiseLine = AddNoiseToVectors(maxNoiseRange, SingleStrokeLine);
                Assert.That(gestureDataset.Recognize(noiseLine) == LineName);
            }

            for (int testIndex = 0; testIndex < timesToTestEachGesture; testIndex++)
            {
                Vector3[] noiseArrow = AddNoiseToVectors(maxNoiseRange, SingleStrokeArrow);
                Assert.That(gestureDataset.Recognize(noiseArrow) == ArrowName);
            }
        }

        [Test]
        [TestCase(0.25f)]
        [TestCase(0.5f)]
        [TestCase(2f)]
        [TestCase(10f)]
        public void DetectScaledGesture(float amount)
        {
            Assert.That(gestureDataset.IsEmpty);

            gestureDataset.AddGesture(MultiStrokeSquare, SquareName);
            gestureDataset.AddGesture(MultiStrokeLine, LineName);
            gestureDataset.AddGesture(MultiStrokeArrow, ArrowName);

            Vector3[] scaledSquare = ScaledVectors(amount, SingleStrokeSquare);
            Assert.That(gestureDataset.Recognize(scaledSquare) == SquareName);

            Vector3[] scaledLine = ScaledVectors(amount, SingleStrokeLine);
            Assert.That(gestureDataset.Recognize(scaledLine) == LineName);

            Vector3[] scaledArrow = ScaledVectors(amount, SingleStrokeArrow);
            Assert.That(gestureDataset.Recognize(scaledArrow) == ArrowName);
        }

        private bool IsListEquals(List<Vector3[]> firstList, List<Vector3[]> secondList)
        {
            if (firstList.Count != secondList.Count)
                return false;

            for (int listIndex = 0; listIndex < firstList.Count; listIndex++)
            {
                if (firstList[listIndex].Length != secondList[listIndex].Length)
                    return false;

                for (int arrayIndex = 0; arrayIndex < firstList[listIndex].Length; arrayIndex++)
                {
                    if (firstList[listIndex][arrayIndex] != secondList[listIndex][arrayIndex])
                        return false;
                }
            }

            return true;
        }

        private Vector3[] ScaledVectors(float amount, Vector3[] source)
        {
            Vector3[] sourceCopy = new Vector3[source.Length];
            source.CopyTo(sourceCopy, 0);
            for (int vector3Index = 0; vector3Index < sourceCopy.Length; vector3Index++)
            {
                sourceCopy[vector3Index].Scale(new Vector3(amount, amount, amount));
            }
            return sourceCopy;
        }

        private Vector3[] AddNoiseToVectors(float maxNoiseRange, Vector3[] source)
        {
            Vector3[] sourceCopy = new Vector3[source.Length];
            source.CopyTo(sourceCopy, 0);
            for (int vector3Index = 0; vector3Index < sourceCopy.Length; vector3Index++)
            {
                sourceCopy[vector3Index].x += Random.Range(-maxNoiseRange, maxNoiseRange);
                sourceCopy[vector3Index].y += Random.Range(-maxNoiseRange, maxNoiseRange);
            }
            return sourceCopy;
        }
    }
}
