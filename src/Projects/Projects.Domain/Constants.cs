
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Projects.Domain
{
    public static class Constants
    {
        // TaskItem name max length is 255
        public static readonly ushort TaskItemNameMaxLength = 255;

        public static readonly ushort TaskDescriptionMaxLength = 4000;
    }
}
