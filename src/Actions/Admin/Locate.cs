//-----------------------------------------------------------------------------
// <copyright file="Locate.cs" company="WheelMUD Development Team">
//   Copyright (c) WheelMUD Development Team.  See LICENSE.txt.  This file is 
//   subject to the Microsoft Public License.  All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using WheelMUD.Core;
using WheelMUD.Server;

namespace WheelMUD.Actions
{
    /// <summary>A command that allows an admin to locate an entity.</summary>
    [ExportGameAction(0)]
    [ActionPrimaryAlias("locate", CommandCategory.Admin)]
    [ActionAlias("whereis", CommandCategory.Admin)]
    [ActionAlias("where is", CommandCategory.Admin)]
    [ActionDescription("Locate the specified entity or item.")]
    [ActionSecurity(SecurityRole.fullAdmin | SecurityRole.minorAdmin)]
    public class Locate : GameAction
    {
        /// <summary>List of reusable guards which must be passed before action requests may proceed to execution.</summary>
        private static readonly List<CommonGuards> ActionGuards = new List<CommonGuards>
        {
            CommonGuards.RequiresAtLeastOneArgument
        };

        /// <summary>Executes the command.</summary>
        /// <param name="actionInput">The full input specified for executing the command.</param>
        public override void Execute(ActionInput actionInput)
        {
            if (!(actionInput.Controller is Session session)) return;
            
            var entity = GetPlayerOrMobile(actionInput.Controller.LastActionInput.Tail);

            actionInput.Controller.Write(entity != null
                ? new OutputBuilder(session.TerminalOptions).SingleLine(
                    $"You see {entity.Name} at {entity.Parent.Name}, id {entity.Parent.Id}")
                : new OutputBuilder(session.TerminalOptions).SingleLine(
                    $"You cant find {actionInput.Controller.LastActionInput.Tail}."));
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