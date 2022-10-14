using EzySlice;
using UnityEngine;

namespace Minigames.Cut
{
    public class Slicer : MonoBehaviour
    {
        public Material materialAfterSlice;
        public LayerMask sliceMask;
        public bool isTouched;

        [SerializeField] private Cut _cut;

        private void Update()
        {
            if (isTouched == true)
            {
                isTouched = false;

                Collider[] objectsToBeSliced = Physics.OverlapBox(transform.position, new Vector3(1, 0.1f, 0.1f), transform.rotation, sliceMask);

                foreach (Collider objectToBeSliced in objectsToBeSliced)
                {
                    if (objectToBeSliced.gameObject.TryGetComponent(out Cuttable cuttable))
                    {
                        _cut.RegisterCut(cuttable._CuttableType);
                    }
                    
                    SlicedHull slicedObject = SliceObject(objectToBeSliced.gameObject, materialAfterSlice);

                    GameObject upperHullGameobject = slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialAfterSlice);
                    GameObject lowerHullGameobject = slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialAfterSlice);

                    upperHullGameobject.transform.position = objectToBeSliced.transform.position;
                    lowerHullGameobject.transform.position = objectToBeSliced.transform.position;

                    MakeItPhysical(upperHullGameobject);
                    MakeItPhysical(lowerHullGameobject);

                    Destroy(objectToBeSliced.gameObject);
                }
            }
        }

        private void MakeItPhysical(GameObject obj)
        {
            obj.AddComponent<MeshCollider>().convex = true;
            obj.AddComponent<Rigidbody>();
        }

        private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
        {
            return obj.Slice(transform.position, transform.up, crossSectionMaterial);
        }


    }
}
