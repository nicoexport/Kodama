#if !UNITY_FLASH
using Plugins.LeanTween.Framework;
using UnityEngine;

namespace Plugins.LeanTween.Examples.Scripts
{
    public class GeneralEventsListeners : MonoBehaviour {
        private Vector3 towardsRotation;
        private float turnForLength = 0.5f;
        private float turnForIter = 0f;
        private Color fromColor;

        // It's best to make this a public enum that you use throughout your project, so every class can have access to it
        public enum MyEvents{ 
            CHANGE_COLOR,
            JUMP,
            LENGTH
        }

        private void Awake(){
            Framework.LeanTween.LISTENERS_MAX = 100; // This is the maximum of event listeners you will have added as listeners
            Framework.LeanTween.EVENTS_MAX = (int)MyEvents.LENGTH; // The maximum amount of events you will be dispatching

            fromColor = GetComponent<Renderer>().material.color;
        }

        private void Start () {
            // Adding Listeners, it's best to use an enum so your listeners are more descriptive but you could use an int like 0,1,2,...
            Framework.LeanTween.addListener(gameObject, (int)MyEvents.CHANGE_COLOR, changeColor);
            Framework.LeanTween.addListener(gameObject, (int)MyEvents.JUMP, jumpUp);
        }

        // ****** Event Listening Methods

        private void jumpUp( LTEvent e ){
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 300f);
        }

        private void changeColor( LTEvent e ){
            Transform tran = (Transform)e.data;
            float distance = Vector3.Distance( tran.position, transform.position);
            Color to = new Color(Random.Range(0f,1f),0f,Random.Range(0f,1f));
            Framework.LeanTween.value( gameObject, fromColor, to, 0.8f ).setLoopPingPong(1).setDelay(distance*0.05f).setOnUpdate(
                (Color col)=>{
                    GetComponent<Renderer>().material.color = col;
                }
            );
        }

        // ****** Physics / AI Stuff

        private void OnCollisionEnter(Collision collision) {
            if(collision.gameObject.layer!=2)
                towardsRotation = new Vector3(0f, Random.Range(-180, 180), 0f);
        }

        private void OnCollisionStay(Collision collision) {
            if(collision.gameObject.layer!=2){
                turnForIter = 0f;
                turnForLength = Random.Range(0.5f, 1.5f);
            }
        }

        private void FixedUpdate(){
            if(turnForIter < turnForLength){
                GetComponent<Rigidbody>().MoveRotation( GetComponent<Rigidbody>().rotation * Quaternion.Euler(towardsRotation * Time.deltaTime ) );
                turnForIter += Time.deltaTime;
            }

            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 4.5f);
        }

        // ****** Key and clicking detection

        private void OnMouseDown(){
            if(Input.GetKey( KeyCode.J )){ // Are you also pressing the "j" key while clicking
                Framework.LeanTween.dispatchEvent((int)MyEvents.JUMP);
            }else{
                Framework.LeanTween.dispatchEvent((int)MyEvents.CHANGE_COLOR, transform); // with every dispatched event, you can include an object (retrieve this object with the *.data var in LTEvent)
            }
        }
    }
}
#endif