using Microsoft.AspNetCore.Authorization;

namespace HammerDrop_Auction_app
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission)  :base(policy: permission.ToString())
        {
          
        }
    }
}
