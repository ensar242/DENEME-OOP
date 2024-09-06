using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.FeedbackSystem
{
   
    public class Feedback : MonoBehaviour
    { 
        [SerializeField] GameObject feedbackObject;
        public void CreateFeedBack()
            => Instantiate(feedbackObject, transform.position +new Vector3(0,0,-1), Quaternion.identity);
       
    }
}