using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using UnityEngine.Profiling;

namespace DeveloperConsole
{
    public static class DevConsole
    {
        public static Dictionary<string, ConCommand> commands = new Dictionary<string, ConCommand>();
        public static List<ConCommand> commandHistory = new List<ConCommand>(); 

    
        /// <summary>
        /// Removes the first item of the list and returns an array of the args
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] GetArgs(string[] input, int maxArgs)
        {

         if (input.Length <= 1) return new string[0];

         List<string> args = new List<string>();

          for (int i = 1; i < input.Length; i++)
                {
                if (args.Count !< maxArgs)
                {
                    args.Add(input[i]);
                }

            }
          return args.ToArray();
        }

        public static object[] GetParameterType(string[] parameters)
        {
            if(parameters.Length == 0) return null;

            object[] result = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                result[i] = CastParameters.CastParameter(parameters[i]);
                Debug.Log(result[i].GetType());
            }
           

            return result;
        }

       

        public static void LoadCommands(Assembly[] assemblies)
        {
            OutPutConsole("Loading commands..");
            
            foreach (var command in FindAttribute<ConCommand>(assemblies))
            {
                if(!commands.ContainsKey(command.CommandName))
                {
                    commands.Add(command.CommandName, command);

                }
            }
            OutPutConsole("commands loaded!");

        }

        public static void OutPutConsole(string output)
        {
            ConsoleController.instance.textOutput.text += output + "\n";
        }


        /// <summary>
        /// find all the attributes
        /// </summary>
        /// <typeparam name="AttributeType"></typeparam>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static AttributeType[] FindAttribute<AttributeType>(Assembly[] assemblies) where AttributeType : Attribute, IConcommand
        {
            List<AttributeType> attributes = new List<AttributeType>();

            for (int i = 0; i < assemblies.Length; i++)
            {
                MethodInfo[] methods = assemblies[i]
                .GetTypes()
                .SelectMany(x => x.GetMethods())
                .Where(y => y.GetCustomAttributes().OfType<AttributeType>().Any())
                .ToArray();

                for (int x = 0; x < methods.Length; x++)
                {
                    AttributeType attribute = methods[x].GetCustomAttribute<AttributeType>();
                    attribute.MethodInfo = methods[x];
                    attributes.Add(attribute);
                }
                
            }
            return attributes.ToArray();

        }




        public static void RunCommand(string[] input)
        {
            string name = input[0];
            Debug.Log("RunCommand" + name);

            ConCommand command = GetConCommand(name);

            if (command != null)
            {
                string[] args = GetArgs(input, command.MethodInfo.GetParameters().Length);
                ExecuteCommand(command, GetParameterType(args));
            }
            else
            {
                OutPutConsole("unknown command: " + '"'  + name + '"');
                Debug.Log("Dont know command!");
            }

        }


        public static ConCommand GetConCommand(string name)
        {
            if(commands.ContainsKey(name))
            {
                return commands[name];
            }
            return null;
        }

        public static object ExecuteCommand(ConCommand command, params object[] args)
        {
            Debug.Log("Executecommand" + command.CommandName);

            var parameters = command.MethodInfo.GetParameters();

            if (args == null || args.Length < parameters.Length)
            {
                object[] tempArgs = new object[parameters.Length];

                for (int i = 0; i < tempArgs.Length; i++)
                {
                    if(args != null && i <= args.Length)
                    {
                        tempArgs[i] = args[i];
                    }
                    else
                    {
                        if (parameters[i].HasDefaultValue)
                        {
                            tempArgs[i] = parameters[i].DefaultValue;
                        }
                    }
                }
                args = tempArgs;
            }

            try
            {
                OutPutConsole(command.CommandName + "");
                command.MethodInfo.Invoke(null, args);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                OutPutConsole(e.Message);
            }
            return null;
        }

        
    }
}
