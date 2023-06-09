using System.Linq.Expressions;
using LinqKit;
using Trivister.Core.Entities;

namespace Trivister.Common.Extensions;

public static class UserPredicate
{
    public static Expression<Func<ApplicationUser, bool>> GetUserByEmail(string email)
    {
        var predicate = PredicateBuilder.New<ApplicationUser>(true);
        predicate = predicate.And(x => x.Email == email);
        return predicate;

    }

    public static Expression<Func<UsersRole, bool>> GetUserById(Guid userId)
    {
        var predicate = PredicateBuilder.New<UsersRole>(true);
        predicate = predicate.And(x => x.UserId == userId);
        return predicate;
    }

    public static Expression<Func<ApplicationRole, bool>> GetUseRleByName(string name)
    {
        var predicate = PredicateBuilder.New<ApplicationRole>(true);
        predicate = predicate.And(a => a.Name.Trim().ToLower() == name.Trim().ToLower());
        return predicate;
    }
}