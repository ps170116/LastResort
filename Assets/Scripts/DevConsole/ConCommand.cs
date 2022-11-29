using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace DeveloperConsole
{

    public interface IConcommand
    {
        MethodInfo MethodInfo { get; set; }
    }

    public class ConCommand : Attribute, IConcommand
    {

        public static ConCommand instance;

        public string CommandName { get; set; }
        public string Description { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public ConCommand(string commandName, string description)
        {
            CommandName = commandName;
            Description = description;
        }
    }

}
