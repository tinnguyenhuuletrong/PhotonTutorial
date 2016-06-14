using UnityEngine;
using System.Collections;
using Bolt;

public class CubeBehaviour : Bolt.EntityEventListener<ICubeState>
{
    public GameObject[] Weapons;

    float resetColorTime;
    Renderer rendererComponent;
    
    //Equivalent of the Start method which exists in Unity, but it's called after the game object has been setup inside Bolt and exists on the network.
    public override void Attached()
    {
        rendererComponent = GetComponent<Renderer>();

        //Bind Properties
        state.SetTransforms(state.Transform, transform);

        //Setup Properties Owner
        if (entity.isOwner)
            OwnerSetup();

        //Register State Change Callback
        state.AddCallback("Color", OnColorChanged);
        state.AddCallback("WeaponIndex", OnWeaponIndexChange);
    }
    
    private void OwnerSetup()
    {
        //Color Random Owner only
        state.Color = new Color(Random.value, Random.value, Random.value);

        // NEW: on the owner we also want to setup the weapons, we randomize one weapon from the available ones and also ammo between 50 to 100
        for (int i = 0; i < state.WeaponArray.Length; ++i)
        {
            state.WeaponArray[i].ID = Mathf.Min(i, Weapons.Length - 1);
            state.WeaponArray[i].Ammo = Random.Range(50, 100);
        }

        // NEW: by default we don't have any weapon up, so set index to -1
        state.WeaponIndex = -1;
    }

    #region BotStateEventHandler

    void OnColorChanged()
    {
        Debug.Log("Color Change" + state.Color);

        rendererComponent.material.color = state.Color;
    }

    private void OnWeaponIndexChange()
    {
        Debug.Log("WeaponIndex Change" + state.WeaponIndex);

        for (int i = 0; i < Weapons.Length; ++i)
        {
            Weapons[i].SetActive(false);
        }

        if (state.WeaponIndex >= 0)
        {
            int objectId = state.WeaponArray[state.WeaponIndex].ID;
            Weapons[objectId].SetActive(true);
        }
    }

    #endregion



    // The "Owner" and this is where SimulateOwner will be called, and only called on that computer
    public override void SimulateOwner()
    {
        var speed = 4f;
        var movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { movement.z += 1; }
        if (Input.GetKey(KeyCode.S)) { movement.z -= 1; }
        if (Input.GetKey(KeyCode.A)) { movement.x -= 1; }
        if (Input.GetKey(KeyCode.D)) { movement.x += 1; }

        if (movement != Vector3.zero)
        {
            transform.position = transform.position + (movement.normalized * speed * BoltNetwork.frameDeltaTime);
        }

        //Flash Color Event. Sent by owner
        if (Input.GetKeyDown(KeyCode.F))
        {
            var flash = FlashColorEvent.Create(entity);
            flash.FlashColor = Color.red;
            flash.Send();
        }

        // NEW: Input polling for weapon selection
        if (Input.GetKeyDown(KeyCode.Alpha1)) state.WeaponIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) state.WeaponIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha0)) state.WeaponIndex = -1;
    }

    //On event Handler
    public override void OnEvent(FlashColorEvent evnt)
    {
        resetColorTime = Time.time + 0.2f;
        rendererComponent.material.color = evnt.FlashColor;
    }

    void Update()
    {
        //Update Flash Color
        if (resetColorTime < Time.time)
        {
            rendererComponent.material.color = state.Color;
        }
    }
}