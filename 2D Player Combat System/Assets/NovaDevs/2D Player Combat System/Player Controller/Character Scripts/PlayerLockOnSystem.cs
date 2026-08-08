using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLockOnSystem : MonoBehaviour
{
    /// <summary>
    /// Tracking Enemies 
    /// </summary>

    [SerializeField]
    private HashSet<DummySack> dummyHashSet;



    public GameObject enemyColliders;
    private DummySack firstElement;
    public bool lockOn;
    public int lockOnIndex = 0;


    /// <summary>
    /// Input Action Vars
    /// </summary>
    private float inputY;
    public InputActionAsset inputSystemsActions;
    private InputAction scrollAction;

    private void OnEnable()
    {
        inputSystemsActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputSystemsActions.FindActionMap("Player").Disable();
    }

    //Awake() is called as soon as the script has loaded in the scene or been instantiated.
    private void Awake()
    {
        scrollAction = inputSystemsActions.FindAction("Scroll");
        dummyHashSet = new HashSet<DummySack>();
    }
    public void MouseScrollWheelInput()
    {
        if (firstElement == null) 
        {
            return; 
        }
        else 
        {
            Vector2 vec = Mouse.current.scroll.ReadValue();
            inputY = vec.y;


            //Run Through HashSet
            //When element index is equal to LockOnIndex set Enemy LockOn Canvas to true
            foreach (var item in dummyHashSet)
            {
                //Debug.Log(inputY);
                if (inputY > 0.3f)
                {
                    lockOnIndex += 1;
                }

                if (inputY < 0f)
                {
                    lockOnIndex -= 1;
                }

                if (lockOnIndex < 0)
                {
                    lockOnIndex = 2;
                }


                if (lockOnIndex >= 3)
                {
                    lockOnIndex = 0;
                }

                if (lockOnIndex == item.ID)
                {
                    lockOn = true;
                    item.SetLockOnUI(true);
                }
                else if (lockOnIndex != item.ID)
                {
                    Debug.Log($"Lock On Switch ON? :: {lockOn}");
                    lockOn = false;
                    item.SetLockOnUI(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            //Create DummySack var to add to Hashset
            enemyColliders = collision.gameObject;
            var dummySack = enemyColliders.GetComponent<DummySack>();
            lockOn = true;

            //Add Enemy to HashSet
            dummyHashSet.Add(dummySack);

            //If the first element is empty add a new dummy and set lockOn to true and turn on canvas
            if (firstElement == null)
            {
                firstElement = dummySack;
                lockOn = true;
                firstElement.SetLockOnUI(true);
            }
            else //Run through the 
            {
                firstElement.SetLockOnUI(false);
                dummySack.SetLockOnUI(true);
                lockOn = true;
            }

                dummySack.SetLockOnUI(lockOn);
            Debug.Log($"\"Dummy Set contains {0} elements: \", {dummyHashSet.Count}");
        }
    }


    private void Update()
    {
        MouseScrollWheelInput();

    }
}