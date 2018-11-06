using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony12;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;
namespace FontModThai
{
    public static class GameObjectConstant
    {
        // Token: 0x06000007 RID: 7 RVA: 0x000021FB File Offset: 0x000003FB
        public static bool Check(string name)
        {
            return GameObjectConstant.componentTitle.Contains(name);
        }

        // Token: 0x04000005 RID: 5
        private static List<string> componentTitle = new List<string>
        {
            "TipsReadText",
            "TipsMassageText",
            "MassageChooseText",
            "MassageText",
            "Text",
            "NameText",
            "InformationNameText",
            "InformationText"
        };
    }

    // Token: 0x02000002 RID: 2
    public static class Main
    {


        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            HarmonyInstance.Create(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            Main.settings = UnityModManager.ModSettings.Load<Main.Settings>(modEntry);
            Main.Logger = modEntry.Logger;
            modEntry.OnToggle = new Func<UnityModManager.ModEntry, bool, bool>(Main.OnToggle);
            modEntry.OnGUI = new Action<UnityModManager.ModEntry>(Main.OnGUI);
            modEntry.OnSaveGUI = new Action<UnityModManager.ModEntry>(Main.OnSaveGUI);
            return true;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020C4 File Offset: 0x000002C4
        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (!value)
            {
                return false;
            }
            Main.enabled = value;
            return true;
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000020D4 File Offset: 0x000002D4
        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginVertical("Box", new GUILayoutOption[0]);
            GUILayout.Label("ขนาดฟอนต์：เล็ก 1<->5 ใหญ่", new GUILayoutOption[0]);
            Main.settings.fontSizeChange = GUILayout.SelectionGrid(Main.settings.fontSizeChange, new string[]
            {
                "โดยปริยาย",
                "1",
                "2",
                "3",
                "4",
                "5"
            }, 6, new GUILayoutOption[0]);
            GUILayout.Label("รายละเอียด： ปรับขนาดฟอนต์", new GUILayoutOption[0]);
            GUILayout.EndVertical();
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002171 File Offset: 0x00000371
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Main.settings.Save(modEntry);
        }

        // Token: 0x04000001 RID: 1
        public static bool enabled;

        // Token: 0x04000002 RID: 2
        public static UnityModManager.ModEntry.ModLogger Logger;

        // Token: 0x04000003 RID: 3
        public static Main.Settings settings;

        // Token: 0x02000005 RID: 5
        public class Settings : UnityModManager.ModSettings
        {
            // Token: 0x06000009 RID: 9 RVA: 0x00002277 File Offset: 0x00000477
            public override void Save(UnityModManager.ModEntry modEntry)
            {
                UnityModManager.ModSettings.Save<Main.Settings>(this, modEntry);
            }

            // Token: 0x04000006 RID: 6
            public int fontSizeChange = 1;
        }
    }

 

    // Token: 0x02000003 RID: 3
    [HarmonyPatch(typeof(SetFont), "Start")]
    public static class SetFont_Patch
    {
        // Token: 0x06000005 RID: 5 RVA: 0x00002180 File Offset: 0x00000380
        private static void Postfix(SetFont __instance)
        {

            Font mfont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            if (!Main.enabled)
            {
                return;
            }
            string name = __instance.name;
            Text component = __instance.GetComponent<Text>();
            int change = component.fontSize + Main.settings.fontSizeChange;

            component.font = mfont;
            component.resizeTextForBestFit = true;

            if (GameObjectConstant.Check(name))
            {
                component.fontSize = change;
            }

            
        }

        // Token: 0x04000004 RID: 4
        private static List<double> fontSizeChange = new List<double>
        {
            1.0,
            1.25,
            1.5
        };
    }


}
