using System;
using UnityEngine;

namespace Minigames.Feed
{
    public class Feedable : MonoBehaviour
    {
        [SerializeField] private FeedableType _feedableType;
        [SerializeField] private Feed _feed;
        [SerializeField] private AudioSource _fedSound;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Food food))
            {
                other.gameObject.SetActive(false);

                if (_feed.gameObject.activeSelf)
                {
                    if (food.GetFoodType() == _feed.correctFoodType)
                    {
                        _fedSound.Play();
                    }
                    
                    _feed.RegisterFoodAndFeedable(food.GetFoodType(), _feedableType);
                }
            }
        }
    }
}
