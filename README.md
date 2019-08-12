# Unity Gesture Recognizer v1.0.2
This is a Unity implementation of [$Q Super-Quick Recognizer](http://depts.washington.edu/ilab/proj/dollar/qdollar.html)
# Installation
Just import the UnityPackage of the lastest version found on **builds** folder
# Usage
### Initialization
- To use it first you must create a new gesture dataset on **Project > Create > Gesture Recognizer > New Gesture Dataset**
- You must reference this ScriptableObject to populate it with information and do the recognition
### Add gesture
- To add a new gesture to recognize use **AddGesture** function found on the GestureDataset
    - You must fill this gesture dataset with a (string) name to identify it
    - For single stroke gestures you should use a Vector3[]
    - For multiple stroke gestures you should use List<Vector3[]>
### Delete gesture
- To remove a gesture use **DeleteGesture** function found on the GestureDataset
    - Provide the (string) name of the gesture to be erased
### Reconize gesture
- To recognize the gesture use **Recognize** function found on the GestureDataset
    - For single stroke gestures you should use a Vector3[]
    - For multiple stroke gestures you should use List<Vector3[]>
### View gestures
- To get a single gesture you can use the **GetGesture** function providing the (string) name
- You can get all the possible gestures using the **Gestures** propertie found on the GestureDataset
- To show a gesture you can use the **GetPointsVector3Lists** function found on the SerializableGesture and pass it to a TrailRenderer using **AddPositions** function
# Tips
- It is recommended to use a TrailRenderer to get the points of the gesture
    - To determine the amount of points to save you can set the **minVertexDistance**
     - Get the positions using the **GetPositions** function on the TrailRenderer
- You can save backups of the gesture dataset using presets
- See DemoScene for usage example
# Change log
- 1.0.2
	- Code cleaning and recalculation of patterns at start instead of doing it on the first stroke
- 1.0.1
	- Recalculation of patterns only when needed
- 1.0.0
	- First working version
