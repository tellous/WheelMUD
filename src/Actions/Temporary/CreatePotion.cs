//-----------------------------------------------------------------------------
// <copyright file="CreatePotion.cs" company="WheelMUD Development Team">
//   Copyright (c) WheelMUD Development Team.  See LICENSE.txt.  This file is 
//   subject to the Microsoft Public License.  All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using WheelMUD.Core;
using WheelMUD.Server;
using WheelMUD.Universe;

namespace WheelMUD.Actions
{
    /// <summary>A command that allows an admin to create a potion.</summary>
    [ExportGameAction(0)]
    [ActionPrimaryAlias("create potion", CommandCategory.Admin)]
    [ActionAlias("createpotion", CommandCategory.Admin)]
    [ActionDescription("Temporary test command. Creates a potion.")]
    [ActionSecurity(SecurityRole.fullAdmin)]
    public class CreatePotion : GameAction
    {
        /// <summary>List of reusable guards which must be passed before action requests may proceed to execution.</summary>
        private static readonly List<CommonGuards> ActionGuards = new List<CommonGuards>
        {
            CommonGuards.InitiatorMustBeAlive,
            CommonGuards.InitiatorMustBeConscious,
            CommonGuards.InitiatorMustBeBalanced,
            CommonGuards.InitiatorMustBeMobile
        };

        /// <summary>Executes the command.</summary>
        /// <param name="actionInput">The full input specified for executing the command.</param>
        public override void Execute(ActionInput actionInput)
        {
            if (!(actionInput.Controller is Session session)) return;
            
            var potionItem = new Thing(new PotionBehavior()
            {
                PotionType = "health",
                Modifier = 30,
                MaxSips = 50,
                SipsLeft = 50,
                Duration = new TimeSpan(0, 0, 15),
            })
            {
                Name = "A colourful potion",
                Description = "This colourful potion is bubbling slowly.",
                KeyWords = new List<string> { "potion", "colourful" }
            };

            actionInput.Controller.Thing.Parent.Children.Add(potionItem);

            var userControlledBehavior = actionInput.Controller.Thing.Behaviors.FindFirst<UserControlledBehavior>();
            userControlledBehavior.Controller.Write(new OutputBuilder(session.TerminalOptions).
                SingleLine("You create a colourful potion"));
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