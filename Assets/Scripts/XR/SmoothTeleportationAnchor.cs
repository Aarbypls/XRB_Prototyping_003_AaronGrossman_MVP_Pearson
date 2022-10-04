using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XR
{
    public class SmoothTeleportationAnchor : BaseTeleportationInteractable
    {
        [SerializeField] private float _teleportSpeed = 6f;
        [SerializeField] private float _stoppingDistance = 0.1f;

        private Vector3 _teleportEnd;
        private bool _isTeleporting;
        private XRRig _rig;
        private XRCustomTeleportationProvider _customTeleportationProvider;

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            BeginTeleport(args.interactor);
        }

        private void BeginTeleport(XRBaseInteractor interactor)
        {
            _rig = interactor.GetComponentInParent<XRRig>();
            _customTeleportationProvider = _rig.GetComponent<XRCustomTeleportationProvider>();
            
            if (_customTeleportationProvider.isTeleporting)
            {
                return;
            }
            
            _customTeleportationProvider.TeleportBegin();
            var interactorPos = interactor.transform.localPosition;
            interactorPos.y = 0;
            _teleportEnd = transform.position - interactorPos;
            _isTeleporting = true;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isTeleporting)
            {
                _rig.transform.position = Vector3.MoveTowards(_rig.transform.position, _teleportEnd,
                    _teleportSpeed * Time.deltaTime);

                if (Vector3.Distance(_rig.transform.position, _teleportEnd) < _stoppingDistance)
                {
                    _isTeleporting = false;
                    _customTeleportationProvider.TeleportEnd();
                }
            }
        }
    }
}
