using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//mesh-materials\bones\
[RequireComponent (typeof(SkinnedMeshRenderer))]
public class AvatarGenerater : MonoBehaviour
{
	//换装部件
	public List<GameObject> EyeList = new List<GameObject> ();
	public List<GameObject> FaceList = new List<GameObject> ();
	public List<GameObject> HairList = new List<GameObject> ();
	public List<GameObject> PantsList = new List<GameObject> ();
	public List<GameObject> ShoesList = new List<GameObject> ();
	public List<GameObject> TopList = new List<GameObject> ();

	//最终获取的SkinnedMeshRenderer分为Mesh（通过CombineMeshes从CombineInstance获取）、Material、Transform（骨骼节点）三部分
	private List<CombineInstance> combineList = new List<CombineInstance> ();
	private List<Material> materialList = new List<Material> ();
	private List<Transform> boneList = new List<Transform> ();

	private SkinnedMeshRenderer smr;
	private Transform[] selfBones;

	void Start ()
	{
		smr = GetComponent<SkinnedMeshRenderer> ();
		Random.seed = (int)Time.time;
		selfBones = GetComponentsInChildren<Transform> ();

		Generate ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Generate ();
		}
	}

	void Reset ()
	{
		combineList.Clear ();
		materialList.Clear ();
		boneList.Clear ();
	}

	void Generate ()
	{
		Reset ();

		RandomSelect (EyeList);
		RandomSelect (FaceList);
		RandomSelect (HairList);
		RandomSelect (PantsList);
		RandomSelect (ShoesList);
		RandomSelect (TopList);

		Mesh tempMesh = new Mesh ();
		tempMesh.CombineMeshes (combineList.ToArray (), false, false);//拼装Mesh
		smr.sharedMesh = tempMesh;
		smr.materials = materialList.ToArray ();
		smr.bones = boneList.ToArray ();

	}

	void RandomSelect (List<GameObject> target)
	{
		if (target.Count > 0) {
			GameObject go = target [Random.Range (0, target.Count)];
			SkinnedMeshRenderer goSMR = go.GetComponent<SkinnedMeshRenderer> ();
			if (goSMR) {
				CombineInstance ci = new CombineInstance ();
				ci.mesh = goSMR.sharedMesh;
				combineList.Add (ci);
				materialList.AddRange (goSMR.materials);
				MatchBones (goSMR.bones);
			}
		}
	}

	void MatchBones (Transform[] bones)
	{
		foreach (Transform t in bones) {
			foreach (Transform s in selfBones) {
				if (s.name.Equals (t.name)) {
					boneList.Add (s);
					break;
				}
			}
		}
	}
}
