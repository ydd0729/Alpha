using UnityEngine;
using UnityEngine.Serialization;

namespace Yd.StylizedHumanMaleV2
{
    [CreateAssetMenu(fileName = "Stylized Human Male V2 Config", menuName = "Scriptable Objects/Stylized Human Male V2 Config")]
    public class StylizedHumanMaleV2Config : ScriptableObject
    {
        public EyeMaterial eyeMaterial;

        public HairMesh hairMesh;
        public HairMaterial hairMaterial;

        public SkinMaterial skinMaterial;

        public TorsoMesh torsoMesh;
        public TorsoMaterial torsoMaterial;

        [FormerlySerializedAs("thighsMaterial")]
        public PantsMaterial pantsMaterial;

        public BootsMesh bootsMesh;
        public BootsMaterial bootsMaterial;

        public BrowsMesh browsMesh;
        public BrowsMaterial browsMaterial;

        public HelmetMesh helmetMesh;
        public HelmetMaterial helmetMaterial;

        public BeardMesh beardMesh;
        public BeardMaterial beardMaterial;

        public BeltMaterial beltMaterial;

        public CapeMesh capeMesh;
        public CapeMaterial capeMaterial;

        public GloveMaterial gloveMaterial;
    }
}