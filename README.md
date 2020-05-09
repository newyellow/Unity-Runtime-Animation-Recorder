# Unity-Runtime-Animation-Recorder
  
  This project can make you recording animations in runtime with Unity, and can save into .anim or Maya .ma format.  
  Though Maya has its own physic simulator, but unity is much faster and can easily control detail movement through scripts.  
    
## Installation
  
  Copy just copy Unity Runtime Recorder folder into your Asset folder and it's ready to go.  
  If you want to see same sample you can also copy DemoAssets folder.
  
  
## How To Use

  Here is a short video demo.  
  [https://youtu.be/RAjU5KodE1w](https://youtu.be/RAjU5KodE1w)

## Unity Anim Saver
※ this function needs UnityEditor to work, so can only work in the Editor.

  1. Drag the UnityAnimationRecorder.cs script on any GameObject, and it will record all transforms in children.  
  2. Press "Set Save Path" button in the inspector, choose pick a folder and enter file name.
  3. Play the scene, and start/end recording by press the key you set in the inspector.
  4. When End Recording pressed, the .anim file will be generated.
  
## Maya Exporter

  Pretty much the same as Unity Anim Saver.  
  Additionally, you have to select an .ma file which contains all model information.  
  
  My script doesn't generate model informations for maya, it only record animation data and append them at the end of .ma file.
  If you want to export the meshes you make in Unity, you can try [Export2Maya](https://www.assetstore.unity3d.com/en/#!/content/17079) which is a nice plugin I use before.
  
### Export Humanoid Animations to Maya

  ※ This step is no longer needed, the script will solve this problem automatically. But since this section explains the problem and the solution, so not remove it.

  Unity and Maya treat spines differently, which sometimes cause issues when you exporting animation with SkinnedMesh.
    
  In Maya, there is an additional "Joint Orient". The actual spine rotation is the sum of transform rotation and joint orient. But since Unity doesn't have this attribute, the transform rotation values in Unity is alreay the sum value. While this plugin is recording the values in Unity, the joint orient values will add once more in Maya, cause the recorded result weird.  
  
  In order to record animation correct, we have to make every spines' "joint orient" values to (0,0,0). And here is a tutorial video if you not sure how to do it.
    
  [Export Humanoid Animation from Unity to Maya - Unity Runtime Animation Recorder](https://youtu.be/Ooxg-rFPTcM)  
  ※ Now the Maya Exporter script will set all spines' joint orient to (0,0,0), which solves this problem automatically.

## FBX Exporter
  
  2017-10-10 Implemented FBX exporter, works similar as Maya Exporter.  
  But for now, there is some memory issue, can't export too complex objects.  
    
  [Export to FBX Demo Video](https://www.youtube.com/watch?v=Hy2U0UYp6cA)
  
  Here are a few things to notice:
  
  1. The "source FBX file" has to be in ASCII format (you can choose ASCII or Binary while exporting FBX file)
  2. Make sure every nodes' name are exactly the same as the source file  
  (sometimes nodes' name got changed by Unity or 3D software)
  

## Dealing with Lag

  If you want to simulate with a big amount of objects, you might ecountered lag.  
  You just need to adjust the Time.timeScale value in the Project Setting (or by using ChangeTimeScale option in my script).  
    
  All physics in Unity will affect by timeScale setting.
  And if you want to modify the object animation through your own script, please use FixedUpdate instead of Update.  
  
## License
[MIT](https://github.com/newyellow/Unity-Runtime-Animation-Recorder/blob/master/LICENSE.md)
