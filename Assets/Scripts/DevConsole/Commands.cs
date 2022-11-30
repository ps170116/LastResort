using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DeveloperConsole
{
    public class Commands : MonoBehaviour
    {
        //This class is pure for running certain commands that are more generic.
        //example: help command to show all the commands in a list


        [ConCommand("Help", "shows information about specific command")]
        public static void Help(string command)
        {
            if (command == null || !DevConsole.commands.ContainsKey(command))
            {
                DevConsole.OutPutConsole("Please give an command to get info on");
                return;
            }

            if(DevConsole.commands.ContainsKey(command))
            {
                ConCommand conCommand = DevConsole.commands[command];
                DevConsole.OutPutConsole(conCommand.CommandName + ": " + conCommand.Description);
            }
        }
    }

}
