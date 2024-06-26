﻿namespace Accounts.Domain
{
    /// <summary>
    /// Used for many-to-many relationship between Team and User
    /// </summary>
    public class TeamMembers
    {
        public required Guid TeamId { get; set;  }

        public required Guid UserId { get; set; }
    }
}
