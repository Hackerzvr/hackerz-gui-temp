using BepInEx;
using UnityEngine;
using Photon.Pun;
using GorillaNetworking;

// This is a Gorilla Tag mod template for building a custom GUI using BepInEx.
// It includes tab support and common modding utilities like room control and movement tweaks.

[BepInPlugin("com.yourname.guitemplate", "My GUI Template", "1.0.0")]
public class MyCoolModTemplate : BaseUnityPlugin
{
    // Room code used for joining a specific room
    private string roomCode = "";

    // Position and size of the GUI window
    private Rect Window = new Rect(100f, 100f, 300f, 400f);

    // Dragging variables
    private bool dragging = false;
    private Vector2 dragstart;

    // Tabs for different mod categories
    private enum Tab { Visual, Photon, Movement }
    private Tab currentTab = Tab.Visual;

    // Current background color of the GUI
    private Color backgroundColor = new Color(0.1f, 0.1f, 0.1f); // Dark gray

    // Draw GUI every frame
    public void OnGUI()
    {
        GUI.backgroundColor = backgroundColor;
        Window = GUI.Window(1337, Window, DoWindow, "My Mod Menu");
        Dragging();
    }

    // Handles window rendering and tab switching
    private void DoWindow(int id)
    {
        GUILayout.BeginVertical();

        // Tab buttons
        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(currentTab == Tab.Visual, "Visual", GUI.skin.button)) currentTab = Tab.Visual;
        if (GUILayout.Toggle(currentTab == Tab.Photon, "Photon", GUI.skin.button)) currentTab = Tab.Photon;
        if (GUILayout.Toggle(currentTab == Tab.Movement, "Movement", GUI.skin.button)) currentTab = Tab.Movement;
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // Render tab content
        switch (currentTab)
        {
            case Tab.Visual: VisualTab(); break;
            case Tab.Photon: PhotonTab(); break;
            case Tab.Movement: MovementTab(); break;
        }

        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    // Enables dragging the window
    private void Dragging()
    {
        if (Event.current.type == EventType.MouseDown && Window.Contains(Event.current.mousePosition))
        {
            dragging = true;
            dragstart = Event.current.mousePosition - new Vector2(Window.x, Window.y);
        }
        else if (Event.current.type == EventType.MouseUp)
        {
            dragging = false;
        }

        if (dragging)
        {
            Window.position = Event.current.mousePosition - dragstart;
        }
    }

    // Visual tab: background color changer
    private void VisualTab()
    {
        GUILayout.Label("Background Color:");
        if (GUILayout.Button("Dark Gray")) backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        if (GUILayout.Button("Black")) backgroundColor = Color.black;
        if (GUILayout.Button("Red")) backgroundColor = Color.red;
        if (GUILayout.Button("Blue")) backgroundColor = Color.blue;
            if (GUILayout.Button("Rainbow"))
    {
        float skibidi = Mathf.Repeat(Time.realtimeSinceStartup * 0.2f, 1f);
        backgroundColor = Color.HSVToRGB(skibidi, 1f, 1f);
    }
}

    // Photon tab: connect, disconnect, quit
    private void PhotonTab()
    {
        GUILayout.Label("Enter Room Code:");
        roomCode = GUILayout.TextField(roomCode, 20);

        if (GUILayout.Button("Join Room") && !string.IsNullOrEmpty(roomCode))
        {
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomCode, 0);
        }

        if (GUILayout.Button("Disconnect"))
        {
            PhotonNetwork.Disconnect();
        }

        if (GUILayout.Button("Quit Game"))
        {
            Application.Quit();
        }
    }

    // Movement tab: speed boost and reset
    private void MovementTab()
    {
        if (GUILayout.Button("Enable Speed Boost"))
        {
            var player = GorillaLocomotion.GTPlayer.Instance;
            if (player != null)
            {
                player.maxJumpSpeed = 9f;
                player.jumpMultiplier = 8f;
            }
        }

        if (GUILayout.Button("Reset Speed"))
        {
            var player = GorillaLocomotion.GTPlayer.Instance;
            if (player != null)
            {
                player.maxJumpSpeed = 4.5f;
                player.jumpMultiplier = 1f;
            }
        }
    }
}
