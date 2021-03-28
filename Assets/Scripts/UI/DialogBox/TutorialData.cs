using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialData {


    public static DialogBoxData GetControls(System.Action callback) {
        DialogBoxData data = new DialogBoxData {
            callback = callback,
            header = "CONTROLS",
            content = "Use Arrow Keys to Rotate\n" +
                        "Use SPACE bar key to dash in the direction\n" +
                        "Use the radar on top right to see how far is your next boobytrap"
        };

        return data;
    }
    public static DialogBoxData GetRadarDesc(System.Action callback) {
        DialogBoxData data = new DialogBoxData {
            callback = callback,
            header = "RADAR",
            content = "This shows how far your next next booby trap.\n" +
                        "For example, if North shows '4' then your next booby trap can be within 4 blocks \n"
        };
        return data;
    }

    public static DialogBoxData GetRespawnMessage(System.Action callback) {
        DialogBoxData data = new DialogBoxData {
            callback = callback,
            header = "RATTA",
            content = "I died...\n" +
                       "We'll have to try again. Maze has reset. Portal can be anywhere.. \n"
        };
        return data;
    }

    public static DialogBoxData GetStartMessage1(System.Action callback) {
        DialogBoxData data = new DialogBoxData {
            callback = callback,
            header = "RATTA",
            content = "Hello!, I am ratta. \nI am stuck in this maze since 5893 years 135 days and 13 hours\n"
        };
        return data;
    }
    public static DialogBoxData GetStartMessage2(System.Action callback) {
        DialogBoxData data = new DialogBoxData {
            callback = callback,
            header = "RATTA",
            content = "Many have tried to help me and failed.\n" +
                        "I hope you are up for a challenge"
        };
        return data;
    }
    public static DialogBoxData GetStartMessage3(System.Action callback) {
        DialogBoxData data = new DialogBoxData {
            callback = callback,
            header = "RATTA",
            content = "Beaware where you move me. This place is filled with booby traps!\n" +
                        "One wrong move and we will have to restart again :(\n"
        };
        return data;
    }
    public static DialogBoxData GetStartMessage4(System.Action callback) {
        DialogBoxData data = new DialogBoxData {
            callback = callback,
            header = "RATTA",
            content = "Portal to my world is somewhere in this maze\n" +
                        "My fate is in your hands now...Good look!\n"
        };
        return data;
    }
}
