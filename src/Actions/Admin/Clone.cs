//-----------------------------------------------------------------------------
// <copyright file="Clone.cs" company="WheelMUD Development Team">
//   Copyright (c) WheelMUD Development Team.  See LICENSE.txt.  This file is 
//   subject to the Microsoft Public License.  All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using WheelMUD.Core;
using WheelMUD.Server;

namespace WheelMUD.Actions
{
    /// <summary>A command that allows an admin to clone an item.</summary>
    [ExportGameAction(0)]
    [ActionPrimaryAlias("clone", CommandCategory.Admin)]
    [ActionDescription("Clones an object.")]
    [ActionSecurity(SecurityRole.fullAdmin)]
    public class Clone : GameAction
    {
        /// <summary>List of reusable guards which must be passed before action requests may proceed to execution.</summary>
        private static readonly List<CommonGuards> ActionGuards = new List<CommonGuards>
        {
            CommonGuards.InitiatorMustBeAlive,
            CommonGuards.InitiatorMustBeConscious,
            CommonGuards.InitiatorMustBeBalanced,
            CommonGuards.InitiatorMustBeMobile,
            CommonGuards.RequiresAtLeastOneArgument
        };

        /// <summary>Executes the command.</summary>
        /// <param name="actionInput">The full input specified for executing the command.</param>
        public override void Execute(ActionInput actionInput)
        {
            if (!(actionInput.Controller is Session session)) return;
            
            var itemID = actionInput.Params[0];
            var parent = actionInput.Controller.Thing.Parent;
            var thing = parent.FindChild(t => t.Id == itemID);
            if (thing == null)
            {
                parent = actionInput.Controller.Thing;
                thing = parent.FindChild(t => t.Id == itemID);
                if (thing == null)
                {
                    actionInput.Controller.Write(new OutputBuilder(session.TerminalOptions).
                        SingleLine($"Cannot find {itemID}."));
                    return;
                }
            }

            var clonedThing = thing.Clone();
            parent.Add(clonedThing);
            var userControlledBehavior = actionInput.Controller.Thing.Behaviors.FindFirst<UserControlledBehavior>();
            userControlledBehavior.Controller.Write(new OutputBuilder(session.TerminalOptions).
                SingleLine($"You clone {thing.Id}. New item is {clonedThing.Id}."));
        }

        /// <summary>Checks against the guards for the command.</summary>
        /// <param name="actionInput">The full input specified for executing the command.</param>
        /// <returns>A string with the error message for the user upon guard failure, else null.</returns>
        public override string Guards(ActionInput actionInput)
        {
            return VerifyCommonGuards(actionInput, ActionGuards);
        }
    }
}