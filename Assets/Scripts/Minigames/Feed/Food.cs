using UnityEngine;

namespace Minigames.Feed
{
    public class Food : MonoBehaviour
    {
        [SerializeField] private FoodType _foodType;

        public FoodType GetFoodType()
        {
            return _foodType;
        }
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
