﻿using UnityEngine;
using System.Collections;

namespace Polyworks
{
    public class ItemUtils : MonoBehaviour
    {
        public static bool GetIsWithinUsableDistance(CollectableItemData item, bool isLogOn = false)
        {
            _log("item.usableRange = " + item.usableRange, isLogOn);
            if (item.usableRange == null || item.usableRange.distance == 0)
            {
                _log("no usableRange, or distance = 0");
                return true;
            }

            UsableRange ur = item.usableRange;
            GameObject targetObject1 = GameObject.Find(ur.target1);
            GameObject targetObject2 = GameObject.Find(ur.target2);

            if (targetObject1 == null || targetObject2 == null)
            {
                _log("targetObject1 or targetObject2 are null", isLogOn);
                return false;
            }
            _log("ur.target1 = " + ur.target1
                + "\nur.target2 = " + ur.target2
                + "\ntargetObject1[ " + targetObject1.name + " ] = " + targetObject1.transform.position
                + "\ntargetObject2[ " + targetObject2.name + " ] = " + targetObject2.transform.position, isLogOn);

            Transform target1 = targetObject1.transform;
            Transform target2 = targetObject2.transform;

            var distance = Vector3.Distance(target1.position, target2.position);
            _log(" distance = " + distance + ", ur.distance = " + ur.distance, isLogOn);
            if (distance < ur.distance)
            {
                return true;
            }
            return false;
        }

        public static bool GetIsRequiredFlagOn(CollectableItemData item, bool isLogOn = false)
        {
            _log("GetIsRequiredFlagOn, item = " + item.name + ", requiredFlag = " + item.requiredFlag, isLogOn);
            if (item.requiredFlag == null || item.requiredFlag == "")
            {
                return true;
            }

            return Game.Instance.GetFlag(item.requiredFlag);
        }

        private static void _log(string message, bool isLogOn = false)
        {
            if (!isLogOn)
            {
                return;
            }
            Debug.Log(message);
        }
    }
}

