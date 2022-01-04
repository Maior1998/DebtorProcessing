using System;
using System.ComponentModel.DataAnnotations.Schema;

using DebtorsProcessing.DatabaseModel.Abstractions;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    public record SessionRefreshToken : BaseRefreshToken<UserSession>
    {

    }
}
