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

    /// <summary>
    /// Allows you to create a command
    /// first give the command name then a discription
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ConCommandAttribute : Attribute, IConcommand
    {

        public static ConCommandAttribute instance;

        public string CommandName { get; set; }
        public string Description { get; set; }

        public MethodInfo MethodInfo { get; set; }
        public Action action;


        public ConCommandAttribute(string commandName, string description)
        {
            CommandName = commandName;
            Description = description;
        }
    }

}
