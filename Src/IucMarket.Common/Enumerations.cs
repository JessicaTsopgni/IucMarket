using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Common
{
    public enum RoleOptions
    {
        Admin,
        Other
    }

    public enum DeliveryPlaceOptions
    {
        Campus_Logbessou,
        Campus_Akwa,
    }
    public enum StateOptions
    {
        Rejected = -1,
        In_process,
        Validated,
        Delivered
    }
    public enum InteractionOptions
    {
        Rate,
        Like,
        Comment,
        Share
    }
}
