using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class FbxObjectsManager
{

	public FbxDataNode objMainNode;
	string saveFileFolder;

	public FbxObjectsManager (FbxDataNode inputObjNode, string folder)
	{
		objMainNode = inputObjNode;
		saveFileFolder = folder;
	}

	// insert new objs into file
	public void EditTargetFile (string targetFilePath)
	{
		string sourceData = File.ReadAllText (targetFilePath);
		string newData = "";

		// find start of the Objects
		int startIndex = sourceData.IndexOf ("Objects:  {", StringComparison.Ordinal);
		// copy data into new
		newData = sourceData.Substring (0, startIndex);


		startIndex += ("Objects:  {").Length;

		StringReader reader = new StringReader (sourceData);

		// skip to start index
		for (int i = 0; i < startIndex; i++)
			reader.Read ();


		// find the end of the Objects {}
		int bracketBalancer = 1;
		int readCounter = 0;

		while (true) {
			char temp = (char)reader.Read ();
			++readCounter;

			if (temp == '{')
				bracketBalancer += 1;
			else if (temp == '}') {
				bracketBalancer -= 1;
				if (bracketBalancer == 0)
					break;
			}
		}

		// write custom datas
		newData += objMainNode.getResultData();

		// end the file
		newData += sourceData.Substring (startIndex + readCounter);

		File.WriteAllText (targetFilePath, newData);
	}

	public void AddAnimationCurveNode (string inputId, FbxAnimationCurveNodeType animCurveType, Vector3 initData)
	{
		string nodeName = "AnimationCurveNode";
		string curveTypeStr = "";

		if (animCurveType == FbxAnimationCurveNodeType.Translation)
			curveTypeStr = "T";
		else if (animCurveType == FbxAnimationCurveNodeType.Rotation)
			curveTypeStr = "R";
		else if (animCurveType == FbxAnimationCurveNodeType.Scale)
			curveTypeStr = "S";
		else if (animCurveType == FbxAnimationCurveNodeType.Visibility)
			curveTypeStr = "Visibility";
		
		string nodeData = inputId + ", \"AnimCurveNode::" + curveTypeStr + "\", \"\"";

		FbxDataNode animCurveNode = new FbxDataNode (nodeName, nodeData, 1);

		FbxDataNode propertiesNode = new FbxDataNode ("Properties70", "", 2);
		propertiesNode.addSubNode (new FbxDataNode ("P", "\"d|X\", \"Number\", \"\", \"A\"," + initData.x, 3));
		propertiesNode.addSubNode (new FbxDataNode ("P", "\"d|Y\", \"Number\", \"\", \"A\"," + initData.y, 3));
		propertiesNode.addSubNode (new FbxDataNode ("P", "\"d|Z\", \"Number\", \"\", \"A\"," + initData.z, 3));

		animCurveNode.addSubNode (propertiesNode);

		// release memory
		animCurveNode.saveDataOnDisk(saveFileFolder);

		objMainNode.addSubNode (animCurveNode);
	}

	public void AddAnimationCurve (string inputId, float[] curveData)
	{
		// prepare some data
		string keyValueFloatDataStr = "";
		string timeArrayDataStr = "";

		for (int i = 0; i < curveData.Length; i++) {
			if (i == 0) {
				keyValueFloatDataStr += curveData [i].ToString ();
				timeArrayDataStr += FbxHelper.getFbxSeconds (i, 60);
			} else {
				keyValueFloatDataStr += "," + curveData [i].ToString ();
				timeArrayDataStr += "," + FbxHelper.getFbxSeconds (i, 60);
			}
		}

		//AnimationCurve: 106102887970656, "AnimCurve::", "" 
		string nodeData = inputId + ", \"AnimCurve::\", \"\"";
		FbxDataNode curveNode = new FbxDataNode ("AnimationCurve", nodeData, 1);

		string dataLengthStr = curveData.Length.ToString ();

		curveNode.addSubNode ("Default", "0");
		curveNode.addSubNode ("KeyVer", "4008");

		FbxDataNode keyTimeNode = new FbxDataNode ("KeyTime", "*" + dataLengthStr, 2);
		keyTimeNode.addSubNode ("a", timeArrayDataStr);
		curveNode.addSubNode (keyTimeNode);

		FbxDataNode keyValuesNode = new FbxDataNode ("KeyValueFloat", "*" + dataLengthStr, 2);
		keyValuesNode.addSubNode ("a", keyValueFloatDataStr);
		curveNode.addSubNode (keyValuesNode);

		curveNode.addSubNode (";KeyAttrFlags", "Cubic|TangeantAuto|GenericTimeIndependent|GenericClampProgressive");

		FbxDataNode keyAttrFlagsNode = new FbxDataNode ("KeyAttrFlags", "*1", 2);
		keyAttrFlagsNode.addSubNode ("a", "24840");
		curveNode.addSubNode (keyAttrFlagsNode);

		FbxDataNode keyRefCountNode = new FbxDataNode ("KeyAttrRefCount", "*1", 2);
		keyRefCountNode.addSubNode ("a", dataLengthStr);
		curveNode.addSubNode (keyRefCountNode);

		// release memory
		curveNode.saveDataOnDisk(saveFileFolder);

		objMainNode.addSubNode (curveNode);
	}
}

public enum FbxAnimationCurveNodeType
{
	Translation,
	Rotation,
	Scale,
	Visibility
}
