using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

using Object = UnityEngine.Object;

namespace Tests
{
    public class Test
    {
        #region INTERNAL

        protected GameObject resource;

        protected float testMaxDuration = 25f;
        protected float testMinDuration = 0.2f;

        protected string testDefaultName = "Test Object";

        private float physicsPassWaitTime = 6f;

        #endregion

        #region INTIALIZATION

        [TearDown]
        protected virtual void Reset()
        {
            Object.Destroy(resource);
            ResetTimeScale();
        }

        #endregion

        #region BEHAVIOURS

        protected void InstantiateResource(string name)
        {
            resource = PrefabUtility.InstantiatePrefab(Resources.Load(name)) as GameObject;
        }

        protected void AddResource(string name)
        {
            (PrefabUtility.InstantiatePrefab(Resources.Load(name)) as GameObject).transform.SetParent(resource.transform);
        }

        protected T GetResource<T>(string name) where T : Object
        {
            return Resources.Load(name) as T;
        }

        protected IEnumerator TriggerEachFrame(float duration, Action frameAction)
        {
            float finishTime = Time.time + duration;
            while (Time.time < finishTime)
            {
                frameAction();
                yield return new WaitForEndOfFrame();
            }
        }

        protected IEnumerator TriggerEachFrame(float duration, Action frameAction, float interval, Action intervalAction)
        {
            float finishTime = Time.time + duration;
            float nextInterval = 0;

            while (Time.time < finishTime)
            {
                if (Time.time >= nextInterval)
                {
                    intervalAction();
                    nextInterval += interval;
                }

                frameAction();
                yield return new WaitForEndOfFrame();
            }
        }

        protected IEnumerator WaitForPhysicsPass()
        {
            yield return new WaitForSeconds(physicsPassWaitTime);
        }

        protected IEnumerator WaitOneFrame()
        {
            yield return null;
        }

        protected IEnumerator WaitForFrames(int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return WaitOneFrame();
            }
        }

        protected IEnumerator WaitOneSecond()
        {
            yield return new WaitForSeconds(1);
        }

        protected IEnumerator WaitTwentySeconds()
        {
            yield return new WaitForSeconds(20);
        }

        protected IEnumerator WaitFiveSeconds()
        {
            yield return new WaitForSeconds(5);
        }

        protected void SetSuperFastTimeScale()
        {
            Time.timeScale = 25f;
        }

        protected void SetFastTimeScale()
        {
            Time.timeScale = 5f;
        }

        protected void ResetTimeScale()
        {
            Time.timeScale = 1f;
        }

        protected T GetGameObject<T>(string name) where T : UnityEngine.Object
        {
            UnityEngine.Object[] objects = Resources.FindObjectsOfTypeAll(typeof(T));

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].name == name) return (T)objects[i];
            }

            return null;
        }

        protected GameObject GetGameObject(string name)
        {
            return GameObject.Find(name);
        }

        protected void DestroyObject(Object o)
        {
            Object.Destroy(o);
        }

        protected void ParentBaseResource(Transform transform)
        {
            transform.SetParent(resource.transform);
        }

        #endregion
    }
}
