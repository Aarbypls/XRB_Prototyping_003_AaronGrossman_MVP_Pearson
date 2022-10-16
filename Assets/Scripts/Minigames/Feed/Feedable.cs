using System;
using UnityEngine;

namespace Minigames.Feed
{
    public class Feedable : MonoBehaviour
    {
        [SerializeField] private FeedableType _feedableType;
        [SerializeField] private Feed _feed;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Food food))
            {
                other.gameObject.SetActive(false);
                _feed.RegisterFoodAndFeedable(food.GetFoodType(), _feedableType);
            }
        }
    }
}
